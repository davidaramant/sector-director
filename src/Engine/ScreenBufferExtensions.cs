// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;
using static System.Math;

namespace SectorDirector.Engine
{
    public static class ScreenBufferExtensions
    {
        #region Bresenham's Line Algorithm (unsafe, will crash if drawing off the buffer)

        public static void PlotLineUnsafe(this ScreenBuffer buffer, Point p0, Point p1, Color color) =>
            PlotLineUnsafe(buffer, p0.X, p0.Y, p1.X, p1.Y, color);

        public static void PlotLineUnsafe(this ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
        {
            if (Abs(y1 - y0) < Abs(x1 - x0))
            {
                if (x0 > x1)
                    PlotLineLowUnsafe(buffer, x1, y1, x0, y0, color);
                else
                    PlotLineLowUnsafe(buffer, x0, y0, x1, y1, color);
            }
            else
            {
                if (y0 > y1)
                    PlotLineHighUnsafe(buffer, x1, y1, x0, y0, color);
                else
                    PlotLineHighUnsafe(buffer, x0, y0, x1, y1, color);
            }
        }

        private static void PlotLineLowUnsafe(ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
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
                buffer.DrawPixelUnsafe(x, y, color);

                if (D > 0)
                {
                    y = y + yi;
                    D = D - 2 * dx;
                }
                D = D + 2 * dy;
            }
        }

        private static void PlotLineHighUnsafe(ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
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
                buffer.DrawPixelUnsafe(x, y, color);

                if (D > 0)
                {
                    x = x + xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
            }
        }

        #endregion

        #region Bresenham's Line Algorithm (safe, will silently ignore drawing off the buffer)

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
                buffer.DrawPixel(x, y, color);

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
                buffer.DrawPixel(x, y, color);

                if (D > 0)
                {
                    x = x + xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
            }
        }
        #endregion

        #region Bresenham's Circle Algorithm (unsafe, will crash if drawing off the buffer)
        public static void PlotCircle(this ScreenBuffer buffer, Point center, int radius, Color color) =>
            PlotCircle(buffer, center.X, center.Y, radius, color);

        public static void PlotCircle(this ScreenBuffer buffer, int xCenter, int yCenter, int radius, Color color)
        {
            int x = 0, y = radius;
            int d = 3 - 2 * radius;
            PlotCircleSegments(buffer, xCenter, yCenter, x, y, color);
            while (y >= x)
            {
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }
                PlotCircleSegments(buffer, xCenter, yCenter, x, y, color);
            }
        }

        private static void PlotCircleSegments(ScreenBuffer buffer, int xc, int yc, int x, int y, Color color)
        {
            buffer.DrawPixelUnsafe(xc + x, yc + y, color);
            buffer.DrawPixelUnsafe(xc - x, yc + y, color);
            buffer.DrawPixelUnsafe(xc + x, yc - y, color);
            buffer.DrawPixelUnsafe(xc - x, yc - y, color);
            buffer.DrawPixelUnsafe(xc + y, yc + x, color);
            buffer.DrawPixelUnsafe(xc - y, yc + x, color);
            buffer.DrawPixelUnsafe(xc + y, yc - x, color);
            buffer.DrawPixelUnsafe(xc - y, yc - x, color);
        }
        #endregion

        #region Bresenham's Circle Algorithm (safe, will silently ignore drawing off the buffer)
        public static void PlotCircleSafe(this ScreenBuffer buffer, Point center, int radius, Color color) =>
            PlotCircleSafe(buffer, center.X, center.Y, radius, color);

        public static void PlotCircleSafe(this ScreenBuffer buffer, int xCenter, int yCenter, int radius, Color color)
        {
            int x = 0, y = radius;
            int d = 3 - 2 * radius;
            PlotCircleSegmentsSafe(buffer, xCenter, yCenter, x, y, color);
            while (y >= x)
            {
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }
                PlotCircleSegmentsSafe(buffer, xCenter, yCenter, x, y, color);
            }
        }

        private static void PlotCircleSegmentsSafe(ScreenBuffer buffer, int xc, int yc, int x, int y, Color color)
        {
            buffer.DrawPixel(xc + x, yc + y, color);
            buffer.DrawPixel(xc - x, yc + y, color);
            buffer.DrawPixel(xc + x, yc - y, color);
            buffer.DrawPixel(xc - x, yc - y, color);
            buffer.DrawPixel(xc + y, yc + x, color);
            buffer.DrawPixel(xc - y, yc + x, color);
            buffer.DrawPixel(xc + y, yc - x, color);
            buffer.DrawPixel(xc - y, yc - x, color);
        }
        #endregion
    }
}