using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Library
{
    public class Spline
    {

        private static Point2D[] _controlPointList;
        //private Texture2D _pixelControlPointTexture, _pixelLineTexture;
        private static List<Vector2> _controlPoints;
        //private int _selectedIndex = 0;

        const int ScaleFactor = 5;

        public Spline(Point2D[] pointList)
        {
            _controlPointList = pointList;
        }

        //Get the catmull-rom spline
        public Vector2[] GetCatmullRomSpline()
        {
            List<Vector2> _splinePoints = new List<Vector2>();
            // Previne que _splinePoints seja incrementado a cada chamada do método. 
            // É necessário limpar o lista pois ela é estática.
            if (_splinePoints != null)
                _splinePoints.Clear();

            // The number of points between the control Points
            int numPoints = 100;

            //For Catmull-Rom spline we need four control points to calculate the curve
            if (_controlPointList.Length < 4)
                throw new ArgumentException("The Catmull-Rom spline needs at least 4 control points");

            // Create the spline. Loops through the control Point list and add the line points into a new list.
            // The control point list must be subtracted by 3, so you'll get just the 2 necessary points to calculate the spline. Remember that the
            // lenght of an array is zero based, so, if you have an array of 4 elements, the length of that array is 5.
            for (int i = 0; i < _controlPointList.Length - 3; i++)
            {
                _controlPoints = new List<Vector2>();
                
                float distance = (float)Math.Sqrt((Math.Pow(_controlPointList[i + 1].x - _controlPointList[i + 2].x, 2) + Math.Pow(_controlPointList[i + 1].y - _controlPointList[i + 2].y, 2)));
                float stepSize = ScaleFactor / distance;
                
                for (int j = 0; j <= (distance / ScaleFactor); j++)
                    _splinePoints.Add(GetPointOnCurvePosition(_controlPointList[i], _controlPointList[i + 1], _controlPointList[i + 2], _controlPointList[i + 3], stepSize * j));
            }

            return (_splinePoints.ToArray());
        }

        public Vector2[] UpdateRomSpline(Point2D[] pointList)
        {
            List<Vector2> _splinePoints = new List<Vector2>();

            if (_splinePoints != null)
                _splinePoints.Clear();

            int numPoints = 100;

            if (pointList.Length < 4)
                throw new ArgumentException("The Catmull-Rom spline needs at least 4 control points");

            for (int i = 0; i < pointList.Length - 3; i++)
            {
                _controlPoints = new List<Vector2>();

                float distance = (float)Math.Sqrt((Math.Pow(pointList[i + 1].x - pointList[i + 2].x, 2) + Math.Pow(pointList[i + 1].y - pointList[i + 2].y, 2)));
                float stepSize = ScaleFactor / distance;

                for (int j = 0; j <= (distance / ScaleFactor); j++)
                    _splinePoints.Add(GetPointOnCurvePosition(pointList[i], pointList[i + 1], pointList[i + 2], pointList[i + 3], stepSize * j));
            }

            return (_splinePoints.ToArray());
        }

        //Calculate the point of the line curve, returns a vector2 to be used by whatever you want to.
        private Vector2 GetPointOnCurvePosition(Point2D p0, Point2D p1, Point2D p2, Point2D p3, float t)
        {
            Vector2 ret = new Vector2();

            float t2 = t * t;
            float t3 = t2 * t;

            ret.X = 0.5f * ((2.0f * p1.x) +
            (-p0.x + p2.x) * t +
            (2.0f * p0.x - 5.0f * p1.x + 4 * p2.x - p3.x) * t2 +
            (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);

            ret.Y = 0.5f * ((2.0f * p1.y) +
            (-p0.y + p2.y) * t +
            (2.0f * p0.y - 5.0f * p1.y + 4 * p2.y - p3.y) * t2 +
            (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);


            // normalizando os vetores
            int x = (int)Math.Floor(ret.X);
            int y = (int)Math.Floor(ret.Y);

            return (new Vector2(x, y));
        }
    }
}
