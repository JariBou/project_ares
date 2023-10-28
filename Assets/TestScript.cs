using System;
using System.Collections;
using System.Collections.Generic;
using GraphicsLabor.Scripts;
using GraphicsLabor.Scripts.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectAres
{
    public class TestScript : MonoBehaviour
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public Vector3 d;
        public Color color;

        public float circleRadius;
        public int precision = 1;
        public Color circleColor;

        public List<Transform> points;
        
        // private void Start()
        // {
        //     Camera.onPostRender += OnCameraPostRender;
        // }
        //
        // private void OnCameraPostRender(Camera cam)
        // {
        //     if (cam == Camera.main)
        //     {
        //         Drawer.Instance.DrawLine(a, b, color);
        //     }
        // }
        void OnEnable()
        {
            Drawer2D.DrawCallback += DrawQuad;
            Drawer2D.DrawCallback += DrawCircle;
            Drawer2D.DrawCallback += DrawPolygon;
        }

        void OnDisable()
        {
            Drawer2D.DrawCallback -= DrawQuad;
            Drawer2D.DrawCallback -= DrawCircle;
            Drawer2D.DrawCallback -= DrawPolygon;
        }
        

        private void DrawQuad()
        {
            // Drawer2D.DrawWiredQuad(a, b, c, d, color);

            Drawer2D.DrawWiredTriangle(a, b, c, color);
            Drawer2D.DrawWiredTriangle(new Triangle(c, d, a, circleColor));
            
        }
        
        private void DrawCircle()
        {
            Drawer2D.DrawWiredCircle(Vector2.zero, circleRadius, circleColor, precision);
        }

        private void DrawPolygon()
        {
            List<Vector2> positions = new List<Vector2>(points.Count);
            foreach (Transform point in points)
            {
                positions.Add(point.position);
            }
            Drawer2D.DrawWiredPolygon(positions, Color.cyan);
        }
    }
}
