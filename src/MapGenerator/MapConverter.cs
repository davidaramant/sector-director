// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.MapGenerator.Data;

namespace SectorDirector.MapGenerator
{
    public static class MapConverter
    {
        public static MapData Convert(Map map)
        {
            MapData data = new MapData();

            var allVertices = map.Vertices.ToList(); // TODO: make these unique

            var allLines = new List<LineDef>();

            foreach (var outerShape in map.OuterShapes)
            {
                var previous = outerShape.Polygon[outerShape.Polygon.Count - 2];

                foreach (var current in outerShape.Polygon)
                {
                    var leftIndex = FindVertex(allVertices, previous);
                    var rightIndex = FindVertex(allVertices, current);

                    if (AlreadyHasLine(previous, current, allLines, allVertices))
                        continue;

                    allLines.Add(new LineDef(leftIndex, rightIndex, sideFront: 0 /* TODO: set this */, blocking: true));

                    previous = current;
                }
            }

            foreach (var layer in map.Layers)
            {
                foreach (var shape in layer.Shapes)
                {
                    var previous = shape.Polygon[shape.Polygon.Count - 2];

                    foreach (var current in shape.Polygon)
                    {
                        var leftIndex = FindVertex(allVertices, previous);
                        var rightIndex = FindVertex(allVertices, current);

                        if (AlreadyHasLine(previous, current, allLines, allVertices))
                            continue;

                        allLines.Add(new LineDef(leftIndex, rightIndex, 0));

                        previous = current;
                    }
                }
            }

            var allSectors = new List<Sector>();


            return data;
        }

        private static int FindVertex(List<Vertex> vertices, IntPoint point)
        {
            return vertices.IndexOf(vertices.First(vertex => vertex.X == point.X && vertex.Y == point.Y));
        }

        private static bool AlreadyHasLine(IntPoint left, IntPoint right, List<LineDef> lines, List<Vertex> vertices)
        {
            var leftIndex = FindVertex(vertices, left);
            var rightIndex = FindVertex(vertices, right);

            return lines.Any(line => (line.V1 == leftIndex && line.V2 == rightIndex) || (line.V1 == rightIndex && line.V2 == leftIndex));
        }
    }
}