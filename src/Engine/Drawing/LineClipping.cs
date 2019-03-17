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

        static OutCode ComputeOutCode(int x, int y, Point bounds)
        {
            OutCode code = OutCode.Inside;

            if (x < 0)
                code |= OutCode.Left;
            else if (x > bounds.X)
                code |= OutCode.Right;
            if (y < 0)
                code |= OutCode.Bottom;
            else if (y > bounds.Y)
                code |= OutCode.Top;

            return code;
        }

        public static (bool shouldDraw, int x0, int y0, int x1, int y1) ClipToScreen(
            ScreenBuffer buffer,
            int x0,
            int y0,
            int x1,
            int y1)
        {
            OutCode outCode0 = ComputeOutCode(x0, y0, buffer.Dimensions);
            OutCode outCode1 = ComputeOutCode(x1, y1, buffer.Dimensions);
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
                    x = (int)(x0 + (x1 - x0) * (buffer.Dimensions.Y - y0) / ((double)y1 - y0));
                    y = buffer.Dimensions.Y;
                }
                else if ((outCodeOut & OutCode.Bottom) == OutCode.Bottom)
                {
                    x = (int)(x0 + (x1 - x0) * -y0 / ((double)y1 - y0));
                    y = 0;
                }
                else if ((outCodeOut & OutCode.Right) == OutCode.Right)
                {
                    y = (int)(y0 + (y1 - y0) * (buffer.Dimensions.X - x0) / ((double)x1 - x0));
                    x = buffer.Dimensions.X;
                }
                else if ((outCodeOut & OutCode.Left) == OutCode.Left)
                {
                    y = (int)(y0 + (y1 - y0) * -x0 / ((double)x1 - x0));
                    x = 0;
                }

                if (outCodeOut == outCode0)
                {
                    x0 = x;
                    y0 = y;
                    outCode0 = ComputeOutCode(x0, y0, buffer.Dimensions);
                }
                else
                {
                    x1 = x;
                    y1 = y;
                    outCode1 = ComputeOutCode(x1, y1, buffer.Dimensions);
                }
            }
            return (accept, x0, y0, x1, y1);
        }

        #endregion
    }
}