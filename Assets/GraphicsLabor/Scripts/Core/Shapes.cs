using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicsLabor.Scripts.Core
{
    
    [Serializable]
    public class Quad
    {
        [SerializeField] private Vector2 _pointA;
        [SerializeField] private Vector2 _pointB;
        [SerializeField] private Vector2 _pointC;
        [SerializeField] private Vector2 _pointD;

        private Vector2 _center;
        
        [SerializeField] private Color _color;
        
        public Vector2 PointA => _pointA;
        public Vector2 PointB => _pointB;
        public Vector2 PointC => _pointC;
        public Vector2 PointD => _pointD;
        public Color GetColor => _color;
        
        public Quad(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, Color color)
        {
            _pointA = pointA;
            _pointB = pointB;
            _pointC = pointC;
            _pointD = pointD;

            _color = color;

            _center = PointA + (PointC - PointA) / 2;
        }
        
        public Quad(Vector2 center, Vector2 size, Color color)
        {
            _pointA = center - Drawer2D.MaskVector2(size, Drawer2D.XMask) / 2 + Drawer2D.MaskVector2(size, Drawer2D.YMask) / 2; // Top left
            _pointB = center + Drawer2D.MaskVector2(size, Drawer2D.XMask) / 2 + Drawer2D.MaskVector2(size, Drawer2D.YMask) / 2; // Top Right
            _pointC = center + Drawer2D.MaskVector2(size, Drawer2D.XMask) / 2 - Drawer2D.MaskVector2(size, Drawer2D.YMask) / 2; // Bottom Right
            _pointD = center - Drawer2D.MaskVector2(size, Drawer2D.XMask) / 2 - Drawer2D.MaskVector2(size, Drawer2D.YMask) / 2; 
            
            _color = color;
        }

        public void ChangeColor(Color color)
        {
            _color = color;
        }
    }
    
    [Serializable]
    public class Circle
    {
        [SerializeField] private Vector2 _center;
        [SerializeField] private float _radius;
        
        [SerializeField] private Color _color;
        
        public Vector2 Center => _center;
        public float Radius => _radius;
        public Color GetColor => _color;

        public Circle(Vector2 center, float radius, Color color)
        {
            _center = center;
            _radius = radius;
            _color = color;
        }
        
        public void ChangeColor(Color color)
        {
            _color = color;
        } 
    }
    
    [Serializable]
    public class Polygon
    {
        [SerializeField] private List<Vector2> _points;
        
        [SerializeField] private Color _color;
        
        public List<Vector2> Points => _points;
        public Color GetColor => _color;

        public Polygon(List<Vector2> points, Color color)
        {
            _points = points;
            _color = color;
        }
        
        public void ChangeColor(Color color)
        {
            _color = color;
        } 
    }
    
    [Serializable]
    public class Triangle
    {
        [SerializeField] private Vector2 _pointA;
        [SerializeField] private Vector2 _pointB;
        [SerializeField] private Vector2 _pointC;
        
        [SerializeField] private Color _color;
        
        public Vector2 PointA => _pointA;
        public Vector2 PointB => _pointB;
        public Vector2 PointC => _pointC;
        public Color GetColor => _color;
        
        public Triangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Color color)
        {
            _pointA = pointA;
            _pointB = pointB;
            _pointC = pointC;

            _color = color;
        }

        public void ChangeColor(Color color)
        {
            _color = color;
        }
    }
}
