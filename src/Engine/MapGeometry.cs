// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public struct Line
    {
        public readonly int LineDefId;
        public readonly int SectorId;
        public readonly int V1;
        public readonly int V2;
        public readonly int PortalToSectorId;

        public Line(int lineDefId, int sectorId, int v1, int v2, int portalToSectorId = -1)
        {
            LineDefId = lineDefId;
            SectorId = sectorId;
            V1 = v1;
            V2 = v2;
            PortalToSectorId = portalToSectorId;
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
        private readonly Line[] _lines;
        private readonly SectorInfo[] _sectors;

        public MapData Map { get; }

        public int VertexCount => _vertices.Length;
        public Vector2[] Vertices => _vertices;
        public ref Vector2 GetVertex(int vertexIndex) => ref _vertices[vertexIndex];

        public int LineCount => _lines.Length;
        public ref Line GetLine(int lineIndex) => ref _lines[lineIndex];

        public ref SectorInfo GetSector(int sectorIndex) => ref _sectors[sectorIndex];
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

            var lines = new List<Line>();
            for (int lineDefId = 0; lineDefId < Map.LineDefs.Count; lineDefId++)
            {
                var lineDef = Map.LineDefs[lineDefId];
                var frontSide = Map.SideDefs[lineDef.SideFront];

                if (lineDef.TwoSided)
                {
                    lines.Add(new Line(lineDefId, frontSide.Sector, lineDef.V1, lineDef.V2));
                }
                else
                {
                    var backSide = Map.SideDefs[lineDef.SideBack];
                    lines.Add(new Line(lineDefId, frontSide.Sector, lineDef.V1, lineDef.V2, portalToSectorId: backSide.Sector));
                    lines.Add(new Line(lineDefId, backSide.Sector, lineDef.V2, lineDef.V1, portalToSectorId: frontSide.Sector));
                }
            }
            _lines = lines.ToArray();

            _sectors = new SectorInfo[map.Sectors.Count];

            foreach (var sectorIndex in Enumerable.Range(0, _sectors.Length))
            {
                var linesForSector =
                    _lines.Select((line, index) => (line, index))
                    .Where(indexedLine => indexedLine.line.SectorId == sectorIndex)
                    .Select(indexedLine => indexedLine.index)
                    .ToArray();

                _sectors[sectorIndex] = new SectorInfo(linesForSector);
            }
        }
    }
}
