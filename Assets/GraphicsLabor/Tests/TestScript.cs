using System.Collections.Generic;
using GraphicsLabor.Scripts.Core;
using UnityEngine;

namespace ProjectAres.GraphicsLabor.Tests
{
    public class TestScript : MonoBehaviour
    {
        [Space, Header("Quad")] 
        public bool _drawQuad;
        public Quad _quad;
        public Color _quadBorderColor;
        public DrawMode _quadDrawMode;
        
        [Space, Header("Circle")]
        public bool _drawCircle;
        public Circle _circle;
        public Color _circleBorderColor;
        public DrawMode _circleDrawMode;

        [Space, Header("Triangle")]
        public bool _drawTriangle;
        public Triangle _triangle;
        public Color _triangleBorderColor;
        public DrawMode _triangleDrawMode;

        [Space, Header("Polygon")]
        public bool _drawPolygon;

        public List<Transform> _polygonPoints;
        public Polygon _polygon;
        public Color _polygonBorderColor;
        public DrawMode _polygonDrawMode;
        
        void OnEnable()
        {
            Drawer2D.DrawCallback += OnDraw;
        }

        void OnDisable()
        {
            Drawer2D.DrawCallback -= OnDraw;
        }

        private void OnDraw()
        {
            if (_drawQuad)
            {
                Drawer2D.DrawQuad(_quad, _quadDrawMode, _quadBorderColor);
            }
            if (_drawCircle)
            {
                Drawer2D.DrawCircle(_circle, _circleDrawMode, _circleBorderColor);
            }
            if (_drawTriangle)
            {
                Drawer2D.DrawTriangle(_triangle, _triangleDrawMode, _triangleBorderColor);
            }
            if (_drawPolygon)
            {
                GatherPolygonPoints();
                Drawer2D.DrawPolygon(_polygon, _polygonDrawMode, _polygonBorderColor);
            }
        }

        private void OnValidate()
        {
            GatherPolygonPoints();
        }

        private void GatherPolygonPoints()
        {
            _polygon.ResetPoints(_polygonPoints.Count);
            foreach (Transform polygonPoint in _polygonPoints)
            {
                _polygon.Points.Add(polygonPoint.position);
            }
        }
    }
}
