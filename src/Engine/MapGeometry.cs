// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public struct Line
    {
        public readonly int V1Index;
        public readonly int V2Index;

        public Line(int v1Index, int v2Index)
        {
            V1Index = v1Index;
            V2Index = v2Index;
        }
    }

    public struct SectorInfo
    {
        public readonly int[] LineIds;

        public SectorInfo(int[] lineIds) => LineIds = lineIds;
    }

    public sealed class MapGeometry
    {
        private readonly Vector2[] _vertices;
        private readonly LineDef[] _lineDefs;
        private readonly SectorInfo[] _sectors;
        
        public MapData Map { get; }

        public int VertexCount => _vertices.Length;
        public Vector2[] Vertices => _vertices;
        public ref Vector2 GetVertex(int index) => ref _vertices[index];

        public LineDef[] LineDefs => _lineDefs;

        public ref SectorInfo GetSector(int index) => ref _sectors[index];
        public int SectorCount => _sectors.Length;

        public Vector2 BottomLeftCorner { get; }
        public Vector2 Area { get; }
        
        public MapGeometry(MapData map)
        {
            Map = map;

            _vertices = map.Vertices.Select(v => v.ToVector2()).ToArray();

            var minX = _vertices.Min(p => p.X);
            var maxX = _vertices.Max(p => p.X);
            var minY = _vertices.Min(p => p.Y);
            var maxY = _vertices.Max(p => p.Y);

            BottomLeftCorner = new Vector2(minX, minY);
            Area = new Vector2(maxX - minX, maxY - minY);

            _lineDefs = map.LineDefs.ToArray();
            // Go through and fix the ID of the linedefs since they might not be set
            for (int id = 0; id < _lineDefs.Length; id++)
            {
                _lineDefs[id].Id = id;
            }

            _sectors = new SectorInfo[map.Sectors.Count];

            foreach (var sectorIndex in Enumerable.Range(0, _sectors.Length))
            {
                // Check if the front side or the optional back side refers to this sector.
                var lineDefIds = _lineDefs
                    .Where(l => Map.SideDefs[l.SideFront].Sector == sectorIndex ||
                        (l.TwoSided && Map.SideDefs[l.SideBack].Sector == sectorIndex))
                    .Select(l => l.Id)
                    .ToArray();

                _sectors[sectorIndex] = new SectorInfo(lineDefIds);
            }
        }
    }
}
