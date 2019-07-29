using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorDirector.Engine
{
    public static class MathShortcuts
    {
        public static float DegToRad = (float)(Math.PI / 180);
        public static float RadToDeg = (float)(180 / Math.PI);
        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos(Vector2.Dot(a, b) / (a.Length() * b.Length()));
        }
        // TODO fix, use at your own risk
        public static float AngleBetween(Vector2 a, Vector2 b, Vector2 view)
        {
            float angle = AngleBetween(a, b);
            float angleBetweenA = AngleBetween(a, view);
            float angleBetweenB = AngleBetween(b, view);
            if (angleBetweenA > angle || angleBetweenB > angle)
            {
                // view is outside of our calculated angle
                return -1 * angle;
            }
            else
            {
                return angle;
            }
        }
        public static bool CounterClockwise(Vector2 A, Vector2 B, Vector2 C)
        {
            return (C.Y - A.Y) * (B.X - A.X) > (B.Y - A.Y) * (C.X - A.X);
        }
        public static bool Intersects(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
        {
            return CounterClockwise(aStart, bStart, bEnd) != CounterClockwise(aEnd, bStart, bEnd) && 
                CounterClockwise(aStart, aEnd, bStart) != CounterClockwise(aStart, aEnd, bEnd);
        }
    }
}
