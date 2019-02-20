// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using static System.Math;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public static class GeometryExtensions
    {
        public static Point DivideBy(this Point p, int denominator) => new Point(p.X / denominator, p.Y / denominator);
        public static Point InvertY(this Point p, int height) => new Point(p.X, height - 1 - p.Y);

        public static Vector2 PerpendicularClockwise(this Vector2 v) => new Vector2(v.Y, -v.X);
        public static Vector2 PerpendicularCounterClockwise(this Vector2 v) => new Vector2(-v.Y, v.X);

        public static float SmallestSide(this Vector2 p) => Min(p.X, p.Y);
        public static float LargestSide(this Vector2 p) => Max(p.X, p.Y);

        public static int SmallestSide(this Point p) => Min(p.X, p.Y);
        public static int LargestSide(this Point p) => Max(p.X, p.Y);
    }
}