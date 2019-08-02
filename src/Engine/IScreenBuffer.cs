// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public interface IScreenBuffer
    {
        Color this[Point p] { get; }
        Color this[int x, int y] { get; }

        Point Dimensions { get; }
        int Height { get; }
        int Width { get; }

        void AddPixel(int x, int y, Color c);
        void Clear();
        void Clear(Rectangle area);
        void DrawPixel(int x, int y, Color c);
    }
}