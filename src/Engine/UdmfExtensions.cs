// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public static class UdfmExtensions
    {
        public static Point ToPoint(this Vertex v) => new Point((int)v.X, (int)v.Y);
        public static Vector2 ToVector2(this Vertex v) => new Vector2((float)v.X, (float)v.Y);

        public static Vector2 GetPosition(this Thing thing) => new Vector2((float)thing.X, (float)thing.Y);
    }
}