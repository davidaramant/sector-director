// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public static class MonoGameExtensions
    {
        public static Point Scale(this Point p, float factor) => new Point((int)(p.X * factor), (int)(p.Y * factor));
        public static Point InvertY(this Point p, int height) => new Point(p.X, height - 1 - p.Y);
    }
}