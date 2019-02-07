// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;
using static System.Math;

namespace SectorDirector.Engine
{
    public static class ScreenBufferExtensions
    {
        public static void PlotLine(this ScreenBuffer buffer, Point p0, Point p1, Color color) =>
            PlotLine(buffer, p0.X, p0.Y, p1.X, p1.Y, color);

        public static void PlotLine(this ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
        {
            if (Abs(y1 - y0) < Abs(x1 - x0))
            {
                if (x0 > x1)
                    PlotLineLow(buffer, x1, y1, x0, y0, color);
                else
                    PlotLineLow(buffer, x0, y0, x1, y1, color);
            }
            else
            {
                if (y0 > y1)
                    PlotLineHigh(buffer, x1, y1, x0, y0, color);
                else
                    PlotLineHigh(buffer, x0, y0, x1, y1, color);
            }
        }

        private static void PlotLineLow(ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                buffer[x, y] = color;

                if (D > 0)
                {
                    y = y + yi;
                    D = D - 2 * dx;
                }
                D = D + 2 * dy;
            }
        }

        private static void PlotLineHigh(ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int D = 2 * dx - dy;
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                buffer[x, y] = color;

                if (D > 0)
                {
                    x = x + xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
            }
        }
    }
}