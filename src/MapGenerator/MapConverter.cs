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

            foreach (var layer in map.Layers.OrderBy(layer => layer.LayerNumber))
            {
                AddLayerSectors(sectors, layer);

                var side = new SideDef(sector: layer.LayerNumber, textureBottom: "STEPTOP");
                sides.Add(side);
            }

            sides.Add(new SideDef(sector: 0, textureBottom: "STEPTOP"));

            var isFirstLayer = true;
            foreach (var layer in map.Layers.OrderByDescending(layer => layer.LayerNumber))
            {
                foreach (var shape in layer.Shapes)
                {
                    foreach (var line in BuildLinesForShape(lines, vertices, shape))
                    {
                        if (line.Id == -1)
                        {
                            var frontLayer = FindOtherLayer(line, vertices, map, shape);
                            if (isFirstLayer)
                            {
                                line.SideFront = frontLayer?.LayerNumber ?? sides.Count - 1;
                                line.SideBack = layer.LayerNumber;
                            }
                            else
                            {
                                line.SideFront = layer.LayerNumber; 
                                line.SideBack = frontLayer?.LayerNumber ?? sides.Count - 1;
                            }
                            line.TwoSided = true;
                            line.Id = lines.IndexOf(line);
                        }
                    }
                }

                isFirstLayer = false;
            }

            AddPlayerStart(things, map);
            AddMonsters(things, map, new Random());
            AddItems(things, map, new Random());

            return new MapData("Doom", lines, sides, vertices, sectors, things);
        }

        private static Layer FindOtherLayer(LineDef line, List<Vertex> vertices, Map map, Shape currentShape)
        {
            var point1 = vertices[line.V1];
            var point2 = vertices[line.V2];

            foreach (var otherLayer in map.Layers)
            {
                foreach (var otherShape in otherLayer.Shapes)
                {
                    if (otherShape == currentShape)
                        continue;

                    var otherPoints = GetLinesPointsForShape(otherShape);

                    foreach (var otherPointPair in otherPoints)
                    {
                        if (ArePointsSame(otherPointPair.Item1, point1) && ArePointsSame(otherPointPair.Item2, point2))
                        {
                            return otherLayer;
                        }
                        if (ArePointsSame(otherPointPair.Item1, point2) && ArePointsSame(otherPointPair.Item2, point1))
                        {
                            return otherLayer;
                        }
                    }
                }
            }

            return null;
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
                x: map.PlayerStart.X,
                y: map.PlayerStart.Y));
        }

        private static void AddMonsters(List<Thing> things, Map map, Random random)
        {
            var possibleMonsters = new List<int>
            {
                3001, /* Imp */
                9, /* Former Sergeant */
                3004, /* Former Human */
                3002, /* Demon */
                3005 /* Cacodemon */
            };
            AddThings(things, map.MonsterPositions, possibleMonsters, random);
        }

        private static void AddItems(List<Thing> things, Map map, Random random)
        {
            var possibleItems = new List<int>
            {
                2012, /* Medkit */
                2015, /* Armor Bonus */
                2018, /* Green Armor */
                2049, /* Box of Shells */
                2048, /* Box of Ammo */
                2007, /* Clip of Ammo */
            };
            AddThings(things, map.MonsterPositions, possibleItems, random);
        }

        private static void AddThings(List<Thing> things, List<IntPoint> positions, List<int> possibleItems, Random random)
        {
            foreach (var position in positions)
            {
                things.Add(new Thing(
                    type: possibleItems[random.Next(possibleItems.Count - 1)],
                    x: position.X,
                    y: position.Y,
                    skill1: true,
                    skill2: true,
                    skill3: true,
                    skill4: true,
                    skill5: true,
                    single: true));
            }
        }

        private static void AddBoundingSides(List<SideDef> sides)
        {
            sides.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));
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
                HeightFloor = layer.Height * 10,
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

        private static IEnumerable<LineDef> BuildLinesForShape(List<LineDef> lines, List<Vertex> vertices, Shape shape)
        {
            foreach (var pointPair in GetLinesPointsForShape(shape))
            {
                var line = GetLineForPoint(pointPair.Item1, pointPair.Item2, lines, vertices);
                if (line == null)
                {
                    var leftIndex = FindVertex(vertices, pointPair.Item1);
                    var rightIndex = FindVertex(vertices, pointPair.Item2);

                    line = new LineDef(leftIndex, rightIndex, 0);
                    lines.Add(line);
                }

                yield return line;
            }
        }

        private static IEnumerable<Tuple<IntPoint, IntPoint>> GetLinesPointsForShape(Shape shape)
        {
            var previous = shape.Polygon[shape.Polygon.Count - 1];

            foreach (var current in shape.Polygon)
            { 
                yield return Tuple.Create(current, previous);

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
            return GetLineForPoint(left, right, lines, vertices) != null;
        }

        private static LineDef GetLineForPoint(IntPoint left, IntPoint right, List<LineDef> lines, List<Vertex> vertices)
        {
            var leftIndex = FindVertex(vertices, left);
            var rightIndex = FindVertex(vertices, right);

            return lines.SingleOrDefault(line => (line.V1 == leftIndex && line.V2 == rightIndex) || (line.V1 == rightIndex && line.V2 == leftIndex));
        }

        private static bool ArePointsSame(Vertex x, Vertex y)
        {
            return Math.Abs(x.X - y.X) < 0.001 && Math.Abs(x.Y - y.Y) < 0.001;
        }

        private static bool ArePointsSame(Vertex x, IntPoint y)
        {
            return Math.Abs(x.X - y.X) < 0.001 && Math.Abs(x.Y - y.Y) < 0.001;
        }

        private static bool ArePointsSame(IntPoint x, Vertex y)
        {
            return Math.Abs(x.X - y.X) < 0.001 && Math.Abs(x.Y - y.Y) < 0.001;
        }
    }
}