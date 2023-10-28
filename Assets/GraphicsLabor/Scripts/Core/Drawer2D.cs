using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace GraphicsLabor.Scripts.Core
{
    public class Drawer2D : MonoBehaviour
    {
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int Cull = Shader.PropertyToID("_Cull");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        private static Drawer2D Instance { get; set; }

        public static readonly Vector3 XMask = new(1, 0, 0);
        public static readonly Vector3 YMask = new(0, 1, 0);
        // public static readonly Vector3 ZMask = new(0, 0, 1); // Should only be used for 3D

        public static event Action DrawCallback;
        
        private Material _renderMaterial;

        #region Unity Setup

        private void Awake()
        {
            Instance ??= this;
        }
        
        private void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        }
        
        private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera locCamera)
        {
            OnDrawCallback();
        }

        private static void OnDrawCallback()
        {
            DrawCallback?.Invoke();
        }

        #endregion
        
        private static void CreateLineMaterial()
        {
            if (Instance._renderMaterial) return;
            
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Instance._renderMaterial = new Material(shader);
            Instance._renderMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            Instance._renderMaterial.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
            Instance._renderMaterial.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            Instance._renderMaterial.SetInt(Cull, (int)CullMode.Off);
            // Turn off depth writes
            Instance._renderMaterial.SetInt(ZWrite, 0);
        }

        public static Vector2 MaskVector2(Vector2 a, Vector2 mask)
        {
            return new Vector2(a.x * mask.x, a.y * mask.y);
        }

        // Should only be used for 3D
        /*public static Vector3 MaskVector3(Vector3 a, Vector3 mask)
        {
            return new Vector3(a.x * mask.x, a.y * mask.y, a.z * mask.z);
        }*/
        
        // Lines are already 2D and 3D
        #region Lines
        
        /// <summary>
        ///  Automatically ignores the list if length not even
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        public static void DrawLines(IEnumerable<Vector3> points, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINES);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            IEnumerable<Vector3> enumerable = points as Vector3[] ?? points.ToArray();
            for (int i = 0; i < enumerable.Count()/2; i++)
            {
                GL.Vertex(enumerable.ElementAt(i));
            }
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        ///  Automatically trims the list if length not even
        /// </summary>
        /// <param name="points"></param>
        /// <param name="colors">Colors of each point, length should be equal or greater than length of points</param>
        public static void DrawLines(List<Vector3> points, List<Color> colors)
        {
            if (points.Count % 2 != 0)
            {
                points.RemoveAt(points.Count-1);
            }

            if (colors.Count < points.Count)
                throw new ArgumentException($"Argument colors of size {colors.Count}, expected {points.Count}");
            
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINES);
            Instance._renderMaterial.SetPass(0);
            
            for (int i = 0; i < points.Count; i++)
            {
                GL.Color(colors[i]);
                GL.Vertex(points[i]);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        public static void DrawBrokenLine(List<Vector3> points, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            foreach (Vector3 point in points)
            {
                GL.Vertex(point);
            }
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="points"></param>
        /// <param name="colors">Colors of each point, length should be equal or greater than length of points</param>
        public static void DrawBrokenLine(List<Vector3> points, List<Color> colors)
        {
            if (colors.Count < points.Count)
                throw new ArgumentException($"Argument colors of size {colors.Count}, expected {points.Count}");
            
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            for (int i = 0; i < points.Count; i++)
            {
                GL.Color(colors[i]);
                GL.Vertex(points[i]);
            }
            
            GL.End();
            GL.PopMatrix();
        }

        public static void DrawLine(Vector3 a, Vector3 b, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINES);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(a);
            GL.Vertex(b);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawLine(Vector3 a, Vector3 b, Color colorA, Color colorB)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINES);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(colorA);
            GL.Vertex(a);
            GL.Color(colorB);
            GL.Vertex(b);
            
            GL.End();
            GL.PopMatrix();
        }
        
        #endregion

        #region Quads
        
        public static void DrawFilledQuad(Quad quad)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.QUADS);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(quad.GetColor);
            GL.Vertex(quad.PointA);
            GL.Vertex(quad.PointB);
            GL.Vertex(quad.PointC);
            GL.Vertex(quad.PointD);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawFilledQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.QUADS);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.Vertex(c);
            GL.Vertex(d);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawFilledQuad(Vector2 center, Vector2 size, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.QUADS);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(center - MaskVector2(size, XMask) / 2 + MaskVector2(size, YMask) / 2); // Top left
            GL.Vertex(center + MaskVector2(size, XMask) / 2 + MaskVector2(size, YMask) / 2); // Top Right
            GL.Vertex(center + MaskVector2(size, XMask) / 2 - MaskVector2(size, YMask) / 2); // Bottom Right
            GL.Vertex(center - MaskVector2(size, XMask) / 2 - MaskVector2(size, YMask) / 2); // Bottom Left
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawFilledQuads(IEnumerable<Quad> quads)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.QUADS);
            Instance._renderMaterial.SetPass(0);

            foreach (Quad quad in quads)
            {
                GL.Color(quad.GetColor);
                GL.Vertex(quad.PointA);
                GL.Vertex(quad.PointB);
                GL.Vertex(quad.PointC);
                GL.Vertex(quad.PointD);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredQuad(Quad quad)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(quad.GetColor);
            GL.Vertex(quad.PointA);
            GL.Vertex(quad.PointB);
            GL.Vertex(quad.PointC);
            GL.Vertex(quad.PointD);
            GL.Vertex(quad.PointA);
            
            GL.End();
            GL.PopMatrix();
        }
        public static void DrawWiredQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.Vertex(c);
            GL.Vertex(d);
            GL.Vertex(a);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredQuad(Vector2 center, Vector2 size, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(center - MaskVector2(size, XMask) / 2 + MaskVector2(size, YMask) / 2); // Top left
            GL.Vertex(center + MaskVector2(size, XMask) / 2 + MaskVector2(size, YMask) / 2); // Top Right
            GL.Vertex(center + MaskVector2(size, XMask) / 2 - MaskVector2(size, YMask) / 2); // Bottom Right
            GL.Vertex(center - MaskVector2(size, XMask) / 2 - MaskVector2(size, YMask) / 2); // Bottom Left
            GL.Vertex(center - MaskVector2(size, XMask) / 2 + MaskVector2(size, YMask) / 2); // Top left
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredQuads(IEnumerable<Quad> quads)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);

            foreach (Quad quad in quads)
            {
                GL.Color(quad.GetColor);
                GL.Vertex(quad.PointA);
                GL.Vertex(quad.PointB);
                GL.Vertex(quad.PointC);
                GL.Vertex(quad.PointD);
                GL.Vertex(quad.PointA);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        #endregion

        // For filled circles can use triangles with center point
        #region Circles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="precision">The number of points used to draw the sphere is 45*precision</param>
        public static void DrawWiredCircle(Circle circle, int precision = 1)
        {
            if (precision <= 0)
                throw new ArgumentException(
                    $"Invalid value for parameter precision (value of {precision}, expected more than 0)");
            
            int numberOfPoints = 45 * precision;
            
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            Vector3 firstPoint = Vector3.zero;
            GL.Color(circle.GetColor);
            for (int i = 0; i < numberOfPoints; ++i)
            {
                float a = i / (float)numberOfPoints;
                float angle = a * Mathf.PI * 2;
                GL.Vertex3(Mathf.Cos(angle) * circle.Radius + circle.Center.x, Mathf.Sin(angle) * circle.Radius + circle.Center.y, 0);
                if (i == 0)
                    firstPoint = new Vector3(Mathf.Cos(angle) * circle.Radius + circle.Center.x, Mathf.Sin(angle) * circle.Radius + circle.Center.y,
                        0);
            }
            GL.Vertex(firstPoint);
            
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="precision">The number of points used to draw the sphere is 45*precision</param>
        public static void DrawWiredCircle(Vector2 center, float radius, Color color, int precision = 1)
        {
            if (precision <= 0)
                throw new ArgumentException(
                    $"Invalid value for parameter precision (value of {precision}, expected more than 0)");
            if (radius < 0) 
                throw new ArgumentException(
                    $"Invalid value for parameter radius (value of {radius}, expected more than 0)");

            int numberOfPoints = 45 * precision;
            
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            Vector3 firstPoint = Vector3.zero;
            GL.Color(color);
            for (int i = 0; i < numberOfPoints; ++i)
            {
                float a = i / (float)numberOfPoints;
                float angle = a * Mathf.PI * 2;
                GL.Vertex3(Mathf.Cos(angle) * radius + center.x, Mathf.Sin(angle) * radius + center.y, 0);
                if (i == 0)
                    firstPoint = new Vector3(Mathf.Cos(angle) * radius + center.x, Mathf.Sin(angle) * radius + center.y,
                        0);
            }
            GL.Vertex(firstPoint);
            
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="circles"></param>
        /// <param name="precision">The number of points used to draw the sphere is 45*precision</param>
        public static void DrawWiredCircles(IEnumerable<Circle> circles, int precision = 1)
        {
            if (precision <= 0)
                throw new ArgumentException(
                    $"Invalid value for parameter precision (value of {precision}, expected more than 0)");
            
            int numberOfPoints = 45 * precision;
            
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            Vector3 firstPoint = Vector3.zero;

            foreach (Circle circle in circles)
            {
                GL.Color(circle.GetColor);
                for (int i = 0; i < numberOfPoints; ++i)
                {
                    float a = i / (float)numberOfPoints;
                    float angle = a * Mathf.PI * 2;
                    GL.Vertex3(Mathf.Cos(angle) * circle.Radius + circle.Center.x, Mathf.Sin(angle) * circle.Radius + circle.Center.y, 0);
                    if (i == 0)
                        firstPoint = new Vector3(Mathf.Cos(angle) * circle.Radius + circle.Center.x, Mathf.Sin(angle) * circle.Radius + circle.Center.y,
                            0);
                }
                GL.Vertex(firstPoint);
            }
            
            GL.End();
            GL.PopMatrix();
        }

        #endregion

        #region Polygon

        /// <summary>
        /// Automatically closes the polygon if needed
        /// </summary>
        /// <param name="polygon"></param>
        public static void DrawWiredPolygon(Polygon polygon)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(polygon.GetColor);
            foreach (Vector2 point in polygon.Points)
            {
                GL.Vertex(point);
            }
            
            if (polygon.Points[^1] != polygon.Points[0]) GL.Vertex(polygon.Points[0]);
            
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        /// Automatically closes the polygon if needed
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        public static void DrawWiredPolygon(List<Vector2> points, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);

            GL.Color(color);
            foreach (Vector2 point in points)
            {
                GL.Vertex(point);
            }
            
            if (points[^1] != points[0]) GL.Vertex(points[0]);
            
            GL.End();
            GL.PopMatrix();
        }
        
        /// <summary>
        /// Automatically closes the polygon if needed
        /// </summary>
        /// <param name="polygons"></param>
        public static void DrawWiredPolygons(IEnumerable<Polygon> polygons)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            
            Instance._renderMaterial.SetPass(0);

            foreach (Polygon polygon in polygons)
            {
                GL.Color(polygon.GetColor);
                foreach (Vector2 point in polygon.Points)
                {
                    GL.Vertex(point);
                }
            
                if (polygon.Points[^1] != polygon.Points[0]) GL.Vertex(polygon.Points[0]);
            }
            
            GL.End();
            GL.PopMatrix();
        }

        #endregion

        #region Triangles
        
        public static void DrawFilledTriangle(Triangle triangle)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.TRIANGLES);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(triangle.GetColor);
            GL.Vertex(triangle.PointA);
            GL.Vertex(triangle.PointB);
            GL.Vertex(triangle.PointC);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawFilledTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.TRIANGLES);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.Vertex(c);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawFilledTriangles(IEnumerable<Triangle> triangles)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.TRIANGLES);
            Instance._renderMaterial.SetPass(0);

            foreach (Triangle triangle in triangles)
            {
                GL.Color(triangle.GetColor);
                GL.Vertex(triangle.PointA);
                GL.Vertex(triangle.PointB);
                GL.Vertex(triangle.PointC);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredTriangle(Triangle triangle)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(triangle.GetColor);
            GL.Vertex(triangle.PointA);
            GL.Vertex(triangle.PointB);
            GL.Vertex(triangle.PointC);
            GL.Vertex(triangle.PointA);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);
            
            GL.Color(color);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.Vertex(c);
            GL.Vertex(a);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawWiredTriangles(IEnumerable<Triangle> triangles)
        {
            CreateLineMaterial();
            
            GL.PushMatrix();
            
            GL.Begin(GL.LINE_STRIP);
            Instance._renderMaterial.SetPass(0);

            foreach (Triangle triangle in triangles)
            {
                GL.Color(triangle.GetColor);
                GL.Vertex(triangle.PointA);
                GL.Vertex(triangle.PointB);
                GL.Vertex(triangle.PointC);
                GL.Vertex(triangle.PointA);
            }
            
            GL.End();
            GL.PopMatrix();
        }

        #endregion
    }
}
