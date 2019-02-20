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

        #region Mike Abrash's version of Wu line drawing

        /* Wu antialiased line drawer.
         * (X0,Y0),(X1,Y1) = line to draw
         * BaseColor = color # of first color in block used for antialiasing, the
         *          100% intensity version of the drawing color
         * NumLevels = size of color block, with BaseColor+NumLevels-1 being the
         *          0% intensity version of the drawing color
         * IntensityBits = log base 2 of NumLevels; the # of bits used to describe
         *          the intensity of the drawing color. 2**IntensityBits==NumLevels
         */
        public static void PlotLineSafeAbrash(this ScreenBuffer buffer, Point p1, Point p2, Color baseColor) =>
           DrawWuLine(buffer, p1.X, p1.Y, p2.X, p2.Y, baseColor);
        public static void DrawWuLine(this ScreenBuffer buffer, int x0, int y0, int x1, int y1, Color baseColor)
        {
            const uint numLevels = 256;
            const int intensityBits = 8;

            void DrawPixel(int x, int y, uint weightingParam = 0)
            {
                var colorCopy = baseColor;
                colorCopy.A = (byte)(byte.MaxValue - (byte)weightingParam);
                buffer.DrawPixel(x, y, colorCopy);
            }

            uint errorAdj;
            uint errorAccTemp, weighting, weightingComplementMask;
            int deltaX, deltaY, xDir;

            /* Make sure the line runs top to bottom */
            if (y0 > y1)
            {
                var temp = y0; y0 = y1; y1 = temp;
                temp = x0; x0 = x1; x1 = temp;
            }
            /* Draw the initial pixel, which is always exactly intersected by
               the line and so needs no weighting */
            DrawPixel(x0, y0);

            if ((deltaX = x1 - x0) >= 0)
            {
                xDir = 1;
            }
            else
            {
                xDir = -1;
                deltaX = -deltaX; /* make DeltaX positive */
            }
            /* Special-case horizontal, vertical, and diagonal lines, which
               require no weighting because they go right through the center of
               every pixel */
            if ((deltaY = y1 - y0) == 0)
            {
                /* Horizontal line */
                while (deltaX-- != 0)
                {
                    x0 += xDir;
                    DrawPixel(x0, y0);
                }
                return;
            }
            if (deltaX == 0)
            {
                /* Vertical line */
                do
                {
                    y0++;
                    DrawPixel(x0, y0);
                } while (--deltaY != 0);
                return;
            }
            if (deltaX == deltaY)
            {
                /* Diagonal line */
                do
                {
                    x0 += xDir;
                    y0++;
                    DrawPixel(x0, y0);
                } while (--deltaY != 0);
                return;
            }

            /* line is not horizontal, diagonal, or vertical */
            uint errorAcc = 0;  /* initialize the line error accumulator to 0 */
                                /* # of bits by which to shift ErrorAcc to get intensity level */
            int intensityShift = 16 - intensityBits;
            /* Mask used to flip all bits in an intensity weighting, producing the
               result (1 - intensity weighting) */
            weightingComplementMask = numLevels - 1;

            /* Is this an X-major or Y-major line? */
            if (deltaY > deltaX)
            {
                /* Y-major line; calculate 16-bit fixed-point fractional part of a
                   pixel that X advances each time Y advances 1 pixel, truncating the
                   result so that we won't overrun the endpoint along the X axis */
                errorAdj = ((uint)deltaX << 16) / (uint)deltaY;
                /* Draw all pixels other than the first and last */
                while (--deltaY != 0)
                {
                    errorAccTemp = errorAcc;   /* remember currrent accumulated error */
                    errorAcc += errorAdj;      /* calculate error for next pixel */
                    if (errorAcc <= errorAccTemp)
                    {
                        /* The error accumulator turned over, so advance the X coord */
                        x0 += xDir;
                    }
                    y0++; /* Y-major, so always advance Y */
                          /* The IntensityBits most significant bits of ErrorAcc give us the
                             intensity weighting for this pixel, and the complement of the
                             weighting for the paired pixel */
                    weighting = errorAcc >> intensityShift;
                    DrawPixel(x0, y0, weighting);
                    DrawPixel(x0 + xDir, y0, (weighting ^ weightingComplementMask));
                }
                /* Draw the final pixel, which is always exactly intersected by the line
                   and so needs no weighting */
                DrawPixel(x1, y1);
                return;
            }

            /* It's an X-major line; calculate 16-bit fixed-point fractional part of a
               pixel that Y advances each time X advances 1 pixel, truncating the
               result to avoid overrunning the endpoint along the X axis */
            errorAdj = ((uint)deltaY << 16) / (uint)deltaX;
            /* Draw all pixels other than the first and last */
            while (--deltaX != 0)
            {
                errorAccTemp = errorAcc;   /* remember currrent accumulated error */
                errorAcc += errorAdj;      /* calculate error for next pixel */
                if (errorAcc <= errorAccTemp)
                {
                    /* The error accumulator turned over, so advance the Y coord */
                    y0++;
                }
                x0 += xDir; /* X-major, so always advance X */
                            /* The IntensityBits most significant bits of ErrorAcc give us the
                               intensity weighting for this pixel, and the complement of the
                               weighting for the paired pixel */
                weighting = errorAcc >> intensityShift;
                DrawPixel(x0, y0, weighting);
                DrawPixel(x0, y0 + 1, (weighting ^ weightingComplementMask));
            }
            /* Draw the final pixel, which is always exactly intersected by the line
               and so needs no weighting */
            DrawPixel(x1, y1);
        }

        #endregion

        #region https://github.com/nejcgalof/Rasterization/blob/master/rasterizacija/Form1.cs

        // HACK!!!! Keep this as global mutable state for convenience
        public static float GammaExponent = 2.5f;
        public static void PlotLineSmooth(this ScreenBuffer buffer, Point point1, Point point2, Color baseColor) =>
            RasterDude(buffer, point1.X, point1.Y, point2.X, point2.Y, baseColor);
        public static void RasterDude(this ScreenBuffer buffer, int x1, int y1, int x2, int y2, Color baseColor)
        {
            void swap(ref int a, ref int b)
            {
                var temp = a;
                a = b;
                b = temp;
            }
            int iPart(float f) => (int)f;
            int round(float f) => (int)(f + 0.5f);
            float fPart(float f) => f - (int)f;
            float rfPart(float f) => 1 - fPart(f);

            float Gamma(float x, float exp) => (float)Pow(x, 1.0f / exp);
            void draw(int x, int y, float intensity) => buffer.AddPixel(x, y, baseColor * Gamma(intensity, GammaExponent));

            bool direction = Abs(y2 - y1) > Abs(x2 - x1);
            if (direction)//replace x and y
            {
                swap(ref x1, ref y1);
                swap(ref x2, ref y2);
            }
            if (x1 > x2)//replace points
            {
                swap(ref x1, ref x2);
                swap(ref y1, ref y2);
            }

            float dx = x2 - x1;
            float dy = y2 - y1;
            float gradient = dy / dx;
            //first point
            int endX = round(x1);
            float endY = y1 + gradient * (endX - x1);
            float gapX = rfPart(x1 + 0.5f);
            int pxlX_1 = endX;
            int pxlY_1 = iPart(endY);
            if (direction)
            {
                draw(pxlY_1, pxlX_1, rfPart(endY) * gapX);
                draw(pxlY_1 + 1, pxlX_1, fPart(endY) * gapX);
            }
            else
            {
                draw(pxlX_1, pxlY_1, rfPart(endY) * gapX);
                draw(pxlX_1, pxlY_1 + 1, fPart(endY) * gapX);
            }
            float intery = endY + gradient;

            //second point
            endX = round(x2);
            endY = y2 + gradient * (endX - x2);
            gapX = fPart(x2 + 0.5f);
            int pxlX_2 = endX;
            int pxlY_2 = iPart(endY);
            if (direction)
            {
                draw(pxlY_2, pxlX_2, rfPart(endY) * gapX);
                draw(pxlY_2 + 1, pxlX_2, fPart(endY) * gapX);
            }
            else
            {
                draw(pxlX_2, pxlY_2, rfPart(endY) * gapX);
                draw(pxlX_2, pxlY_2 + 1, fPart(endY) * gapX);
            }

            //loop from all points
            for (float x = (pxlX_1 + 1); x <= (pxlX_2 - 1); x++)
            {
                if (direction)
                {
                    draw(iPart(intery), (int)x, rfPart(intery));
                    draw((iPart(intery) + 1), (int)x, fPart(intery));
                }
                else
                {
                    draw((int)x, iPart(intery), rfPart(intery));
                    draw((int)x, iPart(intery) + 1, fPart(intery));
                }
                intery += gradient;
            }
        }

        #endregion
    }
}