// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public static class MapGeometryExtensions
    {
        public static bool IsInsideSector(this MapGeometry map, int sectorIndex, Vector2 point)
        {
            // https://stackoverflow.com/questions/11716268/point-in-polygon-algorithm
            bool insideSector = false;
            foreach(var lineId in map.Sectors[sectorIndex].LineIds)
            {
                var line = map.Lines[lineId];
                var vertex1 = map.Map.Vertices[line.V1];
                var vertex2 = map.Map.Vertices[line.V2];

                if (((vertex1.Y >= point.Y) != (vertex2.Y >= point.Y)) &&
                    (point.X <= (vertex2.X - vertex1.X) * (point.Y - vertex1.Y) / (vertex2.Y - vertex1.Y) + vertex1.X))
                {
                    insideSector = !insideSector;
                }
            }

            return insideSector;
        }
    }
}