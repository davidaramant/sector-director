//// Copyright (c) 2018, Aaron Alexander and Matt Moseng
//// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

//using System;
//using ClipperLib;

//using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;

//namespace SectorDirector.MapGeneration
//{
//    public static class ShapeGenerator
//    {
//        public static Polygon GenerateRegularShape(int sides, int radius, IntPoint center)
//        {
//            if (sides < 3)
//                throw new ArgumentException("sides");

//            var points = new Polygon();
//            var step = 360.0f / sides;

//            var initialAngle = 0;
//            var angle = 0f;
//            for (double i = initialAngle; i < initialAngle + 360.0; i += step)
//            {
//                points.Add(DegreesToPoint(angle, radius, center));
//                angle += step;
//            }

//            return points;
//        }

//        private static IntPoint DegreesToPoint(float degrees, float radius, IntPoint origin)
//        {
//            var xy = new IntPoint();
//            var radians = degrees * Math.PI / 180.0;

//            xy.X = (int)(Math.Cos(radians) * radius + origin.X);
//            xy.Y = (int)(Math.Sin(-radians) * radius + origin.Y);

//            return xy;
//        }
//    }
//}