﻿// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.CollectionExtensions;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.LogicalMap
{
    public sealed class SectorGraph
    {
        public MapData Map { get; }
        public List<SubSector> SubSectors { get; } = new List<SubSector>();

        public SectorGraph(MapData map, IEnumerable<SubSector> subSectors)
        {
            Map = map;
            SubSectors.AddRange(subSectors);
        }

        private struct LineAndVertices
        {
            public LineAndVertices(Line line, int startVertexId, int endVertexId)
            {
                Line = line;
                StartVertexId = startVertexId;
                EndVertexId = endVertexId;
            }

            public Line Line { get; }
            public int StartVertexId { get; }
            public int EndVertexId { get; }

            public void Deconstruct(out int startIndex, out int endIndex, out Line line)
            {
                startIndex = StartVertexId;
                endIndex = EndVertexId;
                line = Line;
            }
        }

        private static List<LineAndVertices> BuildLinesWithStartingVertex(MapData map)
        {
            var lines = new List<LineAndVertices>();

            foreach (var lineDef in map.LineDefs)
            {
                lines.Add(new LineAndVertices(
                    new Line(
                        start: map.Vertices[lineDef.V1],
                        end: map.Vertices[lineDef.V2],
                        side: map.SideDefs[lineDef.SideFront],
                        frontSide: true,
                        definition: lineDef),
                    lineDef.V1,
                    lineDef.V2));

                if (lineDef.TwoSided)
                {
                    // Reverse the vertices
                    lines.Add(new LineAndVertices(
                        new Line(
                            start: map.Vertices[lineDef.V2],
                            end: map.Vertices[lineDef.V1],
                            side: map.SideDefs[lineDef.SideBack],
                            frontSide: false,
                            definition: lineDef),
                        lineDef.V2,
                        lineDef.V1));
                }
            }

            return lines;
        }

        public static SectorGraph BuildFrom(MapData map)
        {
            var vertexAndlines = BuildLinesWithStartingVertex(map);

            List<SubSector> subSectors = new List<SubSector>();

            foreach (var lineGroup in vertexAndlines.GroupBy(pair => pair.Line.Side.Sector))
            {
                var sector = map.Sectors[lineGroup.Key];

                var sectorLines = new LinkedList<LineAndVertices>(lineGroup);

                while (sectorLines.Any())
                {
                    var (startVertexId, lastVertexId, line) = sectorLines.TakeFirst();

                    var subSectorLines = new List<Line> { line };

                    while (sectorLines.Any() && lastVertexId != startVertexId)
                    {
                        (_, lastVertexId, line) = sectorLines.TakeFirst(pair => pair.StartVertexId == lastVertexId);

                        subSectorLines.Add(line);
                    }

                    subSectors.Add(new SubSector(lineGroup.Key, sector, subSectorLines));
                }
            }

            return new SectorGraph(map, subSectors);
        }
    }
}