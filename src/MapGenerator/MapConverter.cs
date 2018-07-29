// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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
            var vertices = BuildVerticesList(map);

            var lines = new List<LineDef>();
            var sides = new List<SideDef>();
            var sectors = new List<Sector>();
            var things = new List<Thing>();

            AddBoundingSector(sectors);
            AddBoundingSides(sides);
            AddBoundingLines(map, lines, vertices);

            // TODO: Generate a side between each possible sector pairing and hold in a map

            foreach (var layer in map.Layers)
            {
                AddLayerSectors(sectors, layer);
                AddLayerSides(sides, layer); // TODO: remove this
                AddLayerLines(layer, lines, vertices);
            }

            AddPlayerStart(things, map);

            return new MapData("Doom", lines, sides, vertices, sectors, things);
        }

        private static void AddLayerLines(Layer layer, List<LineDef> lines, List<Vertex> vertices)
        {
            foreach (var shape in layer.Shapes)
            {
                AddLinesForShape(lines, vertices, shape,
                    (rightIndex, leftIndex) => new LineDef(leftIndex, rightIndex, sideFront: layer.Depth * 2 - 1,
                        // TODO: determine the front and back side based on the two sectors/shapes that share the indices of this line
                        sideBack: layer.Depth * 2, twoSided: true));
            }
        }

        private static void AddBoundingLines(Map map, List<LineDef> lines, List<Vertex> vertices)
        {
            AddLinesForShape(lines, vertices, map.BoundingShape,
                (leftIndex, rightIndex) => new LineDef(leftIndex, rightIndex, sideFront: 0, blocking: true));
        }

        private static void AddPlayerStart(List<Thing> things, Map map)
        {
            things.Add(new Thing(
                type: 1,
                x: map.BoundingShape.Polygon[0].X + 20,
                y: map.BoundingShape.Polygon[0].Y + 20));
        }

        private static void AddBoundingSides(List<SideDef> sides)
        {
            sides.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));
        }

        private static void AddLayerSides(List<SideDef> sides, Layer layer)
        {
            sides.Add(new SideDef(sector: layer.Depth - 1, textureBottom: "STEPTOP"));
            sides.Add(new SideDef(sector: layer.Depth));
        }

        private static void AddBoundingSector(List<Sector> sectors)
        {
            sectors.Add(new Sector
            {
                HeightFloor = 0,
                HeightCeiling = 200,
                TextureCeiling = "F_SKY1",
                TextureFloor = "FLAT10",
                LightLevel = 192,
            });
        }

        private static void AddLayerSectors(List<Sector> sectors, Layer layer)
        {
            sectors.Add(new Sector
            {
                HeightFloor = layer.Depth * 10,
                HeightCeiling = 200,
                TextureCeiling = "F_SKY1",
                TextureFloor = "FLOOR0_1",
                LightLevel = 192,
            });
        }

        private static void AddLinesForShape(List<LineDef> lines, List<Vertex> vertices, Shape shape, Func<int, int, LineDef> lineGenerator)
        {
            var previous = shape.Polygon[shape.Polygon.Count - 1];

            foreach (var current in shape.Polygon)
            {
                var leftIndex = FindVertex(vertices, previous);
                var rightIndex = FindVertex(vertices, current);

                if (!AlreadyHasLine(previous, current, lines, vertices))
                {
                    lines.Add(lineGenerator(leftIndex, rightIndex));
                }

                previous = current;
            }
        }

        private static List<Vertex> BuildVerticesList(Map map)
        {
            var vertices = new List<Vertex>();
            vertices.AddRange(map.BoundingShape.Vertices);

            foreach (var vertex in map.Vertices)
            {
                if (!vertices.Any(listVertex => ArePointsSame(listVertex, vertex)))
                {
                    vertices.Add(vertex);
                }
            }

            return vertices;
        }

        private static int FindVertex(List<Vertex> vertices, IntPoint point)
        {
            return vertices.IndexOf(vertices.First(vertex => ArePointsSame(vertex, point)));
        }

        private static bool AlreadyHasLine(IntPoint left, IntPoint right, List<LineDef> lines, List<Vertex> vertices)
        {
            var leftIndex = FindVertex(vertices, left);
            var rightIndex = FindVertex(vertices, right);

            return lines.Any(line => (line.V1 == leftIndex && line.V2 == rightIndex) || (line.V1 == rightIndex && line.V2 == leftIndex));
        }

        private static bool ArePointsSame(Vertex x, Vertex y)
        {
            return Math.Abs(x.X - y.X) < 0.001 && Math.Abs(x.Y - y.Y) < 0.001;
        }

        private static bool ArePointsSame(Vertex x, IntPoint y)
        {
            return Math.Abs(x.X - y.X) < 0.001 && Math.Abs(x.Y - y.Y) < 0.001;
        }
    }
}