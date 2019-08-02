// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Drawing
{
    public static class LineClipping
    {
        public static bool CouldAppearOnScreen(ScreenBuffer buffer, Point p1, Point p2)
        {
            return
                (p1.X >= 0 || p2.X >= 0) &&
                (p1.X < buffer.Width || p2.X < buffer.Width) &&
                (p1.Y >= 0 || p2.Y >= 0) &&
                (p1.Y < buffer.Height || p2.Y < buffer.Height);
        }

        #region Cohen-Sutherland Clipping https://en.wikipedia.org/wiki/Cohen–Sutherland_algorithm

        [Flags]
        enum OutCode
        {
            Inside = 0,
            Left = 1 << 0,
            Right = 1 << 1,
            Bottom = 1 << 2,
            Top = 1 << 3,
        }

        static OutCode ComputeOutCode(Point p, Point bounds)
        {
            OutCode code = OutCode.Inside;

            if (p.X < 0)
                code |= OutCode.Left;
            else if (p.X > bounds.X)
                code |= OutCode.Right;
            if (p.Y < 0)
                code |= OutCode.Bottom;
            else if (p.Y > bounds.Y)
                code |= OutCode.Top;

            return code;
        }

        public static (bool shouldDraw, Point p0, Point p1) ClipToScreen(
            IScreenBuffer buffer,
            Point p0,
            Point p1)
        {
            OutCode outCode0 = ComputeOutCode(p0, buffer.Dimensions);
            OutCode outCode1 = ComputeOutCode(p1, buffer.Dimensions);
            bool accept = false;

            while (true)
            {
                if ((outCode0 | outCode1) == OutCode.Inside)
                {
                    accept = true;
                    break;
                }
                if ((outCode0 & outCode1) != OutCode.Inside)
                {
                    break;
                }

                int x = 0;
                int y = 0;

                OutCode outCodeOut = (outCode0 != OutCode.Inside) ? outCode0 : outCode1;

                if ((outCodeOut & OutCode.Top) == OutCode.Top)
                { 
                    x = (int)(p0.X + (p1.X - p0.X) * (buffer.Dimensions.Y - p0.Y) / ((double)p1.Y - p0.Y));
                    y = buffer.Dimensions.Y;
                }
                else if ((outCodeOut & OutCode.Bottom) == OutCode.Bottom)
                {
                    x = (int)(p0.X + (p1.X - p0.X) * -p0.Y / ((double)p1.Y - p0.Y));
                    y = 0;
                }
                else if ((outCodeOut & OutCode.Right) == OutCode.Right)
                {
                    y = (int)(p0.Y + (p1.Y - p0.Y) * (buffer.Dimensions.X - p0.X) / ((double)p1.X - p0.X));
                    x = buffer.Dimensions.X;
                }
                else if ((outCodeOut & OutCode.Left) == OutCode.Left)
                {
                    y = (int)(p0.Y + (p1.Y - p0.Y) * -p0.X / ((double)p1.X - p0.X));
                    x = 0;
                }

                if (outCodeOut == outCode0)
                {
                    p0 = new Point(x,y);
                    outCode0 = ComputeOutCode(p0, buffer.Dimensions);
                }
                else
                {
                    p1 = new Point(x,y);
                    outCode1 = ComputeOutCode(p1, buffer.Dimensions);
                }
            }
            return (accept, p0, p1);
        }

        #endregion
    }
}