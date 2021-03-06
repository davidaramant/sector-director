﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using static System.Math;

namespace SectorDirector.Engine.Drawing
{
    public static class ScreenBufferExtensions
    {
        public static void DrawPixel(this IScreenBuffer buffer, Point p, Color c) => buffer.DrawPixel(p.X, p.Y, c);
        public static void AddPixel(this IScreenBuffer buffer, Point p, Color c) => buffer.AddPixel(p.X, p.Y, c);

        #region Bresenham's Line Algorithm

        public static void PlotLine(this IScreenBuffer buffer, Point p0, Point p1, Color color) =>
            PlotLine(buffer, p0.X, p0.Y, p1.X, p1.Y, color);

        public static void PlotLine(this IScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
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

        private static void PlotLineLow(IScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
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

        private static void PlotLineHigh(IScreenBuffer buffer, int x0, int y0, int x1, int y1, Color color)
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

        #region Bresenham's Circle Algorithm
        public static void PlotCircle(this IScreenBuffer buffer, Point center, int radius, Color color) =>
            PlotCircle(buffer, center.X, center.Y, radius, color);

        public static void PlotCircle(this IScreenBuffer buffer, int xCenter, int yCenter, int radius, Color color)
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

        private static void PlotCircleSegments(IScreenBuffer buffer, int xc, int yc, int x, int y, Color color)
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

        #region Adapted version of Mike Abrash's Wu line drawer

        // HACK!!!! Keep this as global mutable state for convenience
        public static float GammaExponent = 2.5f;

        // Based on the version from his Graphics Programming Black Book http://www.jagregory.com/abrash-black-book/
        // The integer error stuff was removed in favor of floats.
        // It might have worked if I used 'unchecked' but I got frustrated debugging it.  FPUs are considerably faster these days anyway.
        public static void PlotLineSmooth(this IScreenBuffer buffer, Point p1, Point p2, Color baseColor) =>
            PlotLineSmooth(buffer, p1.X, p1.Y, p2.X, p2.Y, baseColor);
        public static void PlotLineSmooth(this IScreenBuffer buffer, int x0, int y0, int x1, int y1, Color baseColor)
        {
            void Swap(ref int a, ref int b)
            {
                var temp = a;
                a = b;
                b = temp;
            }
            float FractionalPart(float f) => f - (int)f;
            float ReciprocalOfFractionalPart(float f) => 1 - FractionalPart(f);

            float Gamma(float x, float exp) => (float)Pow(x, 1.0f / exp);
            void DrawPixel(int x, int y) => buffer.DrawPixel(x, y, baseColor);
            void DrawPixelScale(int x, int y, float intensity) => buffer.AddPixel(x, y, baseColor * Gamma(intensity, GammaExponent));

            // Make sure the line runs top to bottom
            if (y0 > y1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            // Draw the initial pixel, which is always exactly intersected by the line and so needs no weighting 
            DrawPixel(x0, y0);

            var deltaX = x1 - x0;
            var xDir = 1;

            if (deltaX < 0)
            {
                xDir = -1;
                deltaX = -deltaX; // make DeltaX positive
            }

            var deltaY = y1 - y0; // Guaranteed to be positive since we made sure it goes from top to bottom

            // Special cases for horizontal, vertical, and diagonal lines
            if (deltaY == 0)
            {
                while (deltaX-- != 0)
                {
                    x0 += xDir;
                    DrawPixel(x0, y0);
                }
                return;
            }
            if (deltaX == 0)
            {
                do
                {
                    y0++;
                    DrawPixel(x0, y0);
                } while (--deltaY != 0);
                return;
            }
            if (deltaX == deltaY)
            {
                do
                {
                    x0 += xDir;
                    y0++;
                    DrawPixel(x0, y0);
                } while (--deltaY != 0);
                return;
            }

            // line is not horizontal, diagonal, or vertical
            float gradient = 0;
            float accumulatedError = 0;

            bool isYMajorLine = deltaY > deltaX;
            if (isYMajorLine)
            {
                gradient = (float)deltaX / deltaY;
                while (--deltaY != 0)
                {
                    accumulatedError += gradient;
                    y0++;

                    DrawPixelScale(x0 + xDir * (int)accumulatedError, y0, ReciprocalOfFractionalPart(accumulatedError));
                    DrawPixelScale(x0 + xDir * (int)accumulatedError + xDir, y0, FractionalPart(accumulatedError));
                }
            }
            else
            {
                gradient = (float)deltaY / deltaX;
                while (--deltaX != 0)
                {
                    accumulatedError += gradient;
                    x0 += xDir;

                    DrawPixelScale(x0, y0 + (int)accumulatedError, ReciprocalOfFractionalPart(accumulatedError));
                    DrawPixelScale(x0, y0 + (int)accumulatedError + 1, FractionalPart(accumulatedError));
                }
            }
            // Draw the final pixel, which is always exactly intersected by the line
            // and so needs no weighting
            DrawPixel(x1, y1);
        }

        #endregion
    }
}