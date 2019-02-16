// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public sealed class MapGeometry
    {
        public MapData Map { get; }

        public Vector2[] Vertices { get; }

        public Vector2 BottomLeftCorner { get; }
        public Vector2 Area { get; }

        public MapGeometry(MapData map)
        {
            Map = map;

            Vertices = map.Vertices.Select(v => v.ToVector2()).ToArray();

            var minX = Vertices.Min(p => p.X);
            var maxX = Vertices.Max(p => p.X);
            var minY = Vertices.Min(p => p.Y);
            var maxY = Vertices.Max(p => p.Y);

            BottomLeftCorner = new Vector2(minX, minY);
            Area = new Vector2(maxX - minX, maxY - minY);
        }
    }
}
