﻿//// Copyright (c) 2018, Aaron Alexander and Matt Moseng
//// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using ClipperLib;
//using SectorDirector.MapGeneration.Data;

//using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;
//using Polygons = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

//namespace SectorDirector.MapGeneration
//{
//    public static class OverlappingMapGeneration
//    {
//        private const int DefaultCount = 3;
//        private const int RadiusMin = 80;
//        private const int RadiusMax = 250;
//        private const int CenterMin = 300;
//        private const int CenterMax = 3000;

//        private const int SidesInCircles = 96;
//        private const int MinimumPolygonSides = 3;
//        private const int MaximumPolygonSides = 12;
//        private const double CirclePercentage = 0.2;

//        public static Map GenerateMap(int shapeCount = DefaultCount, PolygonTypes types = PolygonTypes.Everything, int? seed = null, ThingsTypes thingsToInclude = ThingsTypes.Monsters | ThingsTypes.Items)
//        {
//            var map = new Map();

//            var random = new Random();
//            if (!seed.HasValue)
//            {
//                seed = random.Next(Int32.MaxValue);
//            }

//            Console.WriteLine("Using Seed: {0}", seed.Value);
//            random = new Random(seed.Value);

//            var allShapes = new Polygons();

//            for (int i = 0; i < shapeCount; i++)
//            {
//                var sides = GetSidesForShape(types, random);
//                var radius = random.Next(RadiusMin, RadiusMax);
//                var centerX = random.Next(CenterMin, CenterMax);
//                var centerY = random.Next(CenterMin, CenterMax);
//                var height = random.Next(1, 4);

//                var shape = ShapeGenerator.GenerateRegularShape(sides, radius, new IntPoint(centerX, centerY));
//                var layerShapes = new Polygons();

//                if (i == 0)
//                {
//                    layerShapes.Add(shape);
//                }
//                else
//                {
//                    layerShapes.AddRange(SubtractShape(shape, allShapes));

//                    foreach (var polygon in layerShapes)
//                        foreach (var point in polygon)
//                            ProcessCollinearity(point, map.Layers);
//                }

//                allShapes.AddRange(layerShapes);
//                map.Layers.Add(new Layer(
//                    height: height,
//                    layerNumber: shapeCount - i,
//                    shapes: layerShapes.Select(polygon => new Shape(polygon))
//                ));
//            }

//            map.BoundingShape = new Shape(BuildBoundingShape(map));

//            var outerPerimeter = OuterPerimeter(map).Select(polygon => new Shape(polygon));
//            //map.Layers.Add(new Layer(
//            //    depth: 0,
//            //    shapes: outerPerimeter));
//            map.OuterShapes.AddRange(outerPerimeter);

//            map.PlayerStart = RandomPosition(map, random);

//            if ((thingsToInclude & ThingsTypes.Monsters) == ThingsTypes.Monsters)
//            {
//                if ((thingsToInclude & ThingsTypes.Bosses) == ThingsTypes.Bosses)
//                {
//                    for (var i = 0; i < random.Next(5, 8); i++)
//                    {
//                        map.BossPositions.Add(RandomPosition(map, random));
//                    }
//                    for (var i = 0; i < random.Next(10, 40); i++)
//                    {
//                        map.MonsterPositions.Add(RandomPosition(map, random));
//                    }
//                }
//                else
//                {
//                    for (var i = 0; i < random.Next(10, 100); i++)
//                    {
//                        map.MonsterPositions.Add(RandomPosition(map, random));
//                    }
//                }
//            }

//            if ((thingsToInclude & ThingsTypes.Items) == ThingsTypes.Items)
//            {
//                for (var i = 0; i < random.Next(10, 40); i++)
//                {
//                    map.ItemPositions.Add(RandomPosition(map, random));
//                }
//            }

//            // PrintLayerPoints(map);

//            return map;
//        }

//        private static int GetSidesForShape(PolygonTypes types, Random random)
//        {
//            switch (types)
//            {
//                case PolygonTypes.Everything:
//                    return (random.NextDouble() <= CirclePercentage)
//                        ? SidesInCircles
//                        : random.Next(MinimumPolygonSides, MaximumPolygonSides);
//                case PolygonTypes.OnlyCircles:
//                    return SidesInCircles;
//                case PolygonTypes.OnlyPolygons:
//                    return random.Next(MinimumPolygonSides, MaximumPolygonSides);
//            }
//            throw new ArgumentException();
//        }

//        private static IntPoint RandomPosition(Map map, Random random)
//        {
//            var minimumX = (int) map.BoundingShape.Polygon.Select(point => point.X).Min() + 20;
//            var maximumX = (int)map.BoundingShape.Polygon.Select(point => point.X).Max() - 20;

//            var minimumY = (int)map.BoundingShape.Polygon.Select(point => point.Y).Min() + 20;
//            var maximumY = (int)map.BoundingShape.Polygon.Select(point => point.Y).Max() - 20;

//            var x = random.Next(minimumX, maximumX);
//            var y = random.Next(minimumY, maximumY);

//            return new IntPoint(x, y);
//        }

//        private static void ProcessCollinearity(IntPoint point, List<Layer> layers)
//        {
//            foreach (var layer in layers)
//            {
//                foreach (var shape in layer.Shapes)
//                {
//                    var previous = shape.Polygon[shape.Polygon.Count - 1];

//                    foreach (var current in shape.Polygon)
//                    {
//                        if (IsColinear(point, previous, current))
//                        {
//                            shape.Polygon.Insert(shape.Polygon.IndexOf(current), point);
//                            break;
//                        }

//                        previous = current;
//                    }
//                }
//            }
//        }

//        private static bool IsColinear(IntPoint toBeChecked, IntPoint p1, IntPoint p2)
//        {
//            if (toBeChecked.Equals(p1) || toBeChecked.Equals(p2))
//                return false;

//            var dist12 = Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
//            var distT1 = Math.Sqrt(Math.Pow((toBeChecked.X - p1.X), 2) + Math.Pow((toBeChecked.Y - p1.Y), 2));
//            var distT2 = Math.Sqrt(Math.Pow((p2.X - toBeChecked.X), 2) + Math.Pow((p2.Y - toBeChecked.Y), 2));

//            double epsilon = .1;

//            return Math.Abs(dist12 - (distT1 + distT2)) <= epsilon;
//        }

//        private static void PrintLayerPoints(Map map)
//        {
//            for (var layerIndex = 0; layerIndex < map.Layers.Count; layerIndex++)
//            {
//                var layer = map.Layers[layerIndex];
//                for (var shapeIndex = 0; shapeIndex < layer.Shapes.Count; shapeIndex++)
//                {
//                    var shape = layer.Shapes[shapeIndex];

//                    Console.WriteLine("Layer #{0}, Shape #{1}, Points: {2}", layerIndex, shapeIndex,
//                        String.Join(",", shape.Polygon.Select(point => $"({point.X},{point.Y})").ToArray()));
//                }
//            }
//        }

//        private static Polygons OuterPerimeter(Map map)
//        {
//            var boundingShape = BuildBoundingShape(map);

//            var allShapes = new Polygons();
//            foreach (var layer in map.Layers)
//            {
//                foreach (var shape in layer.Shapes)
//                {
//                    allShapes.Add(shape.Polygon);
//                }
//            }

//            return SubtractShape(boundingShape, allShapes);
//        }

//        private static Polygon BuildBoundingShape(Map map)
//        {
//            var minimumX = map.Vertices.Select(vertex => vertex.X).Min();
//            var maximumX = map.Vertices.Select(vertex => vertex.X).Max();
//            var minimumY = map.Vertices.Select(vertex => vertex.Y).Min();
//            var maximumY = map.Vertices.Select(vertex => vertex.Y).Max();

//            var boundingShape = new Polygon
//            {
//                new IntPoint(minimumX - 48, minimumY - 48),
//                new IntPoint(minimumX - 48, maximumY + 48),
//                new IntPoint(maximumX + 48, maximumY + 48),
//                new IntPoint(maximumX + 48, minimumY - 48),
//            };

//            return boundingShape;
//        }

//        private static Polygons SubtractShape(Polygon subject, Polygons clips)
//        {
//            return SubtractShape(new Polygons { subject }, clips);
//        }

//        private static Polygons SubtractShape(Polygons subjects, Polygons clips)
//        {
//            return ProcessShapes(subjects, clips, ClipType.ctDifference);
//        }

//        private static Polygons IntersectShape(Polygon subject, Polygons clips)
//        {
//            return ProcessShapes(new Polygons { subject }, clips, ClipType.ctIntersection);
//        }

//        private static Polygons ProcessShapes(Polygons subjects, Polygons clips, ClipType clipType)
//        {
//            var clipper = new Clipper();
//            clipper.AddPaths(subjects, PolyType.ptSubject, true);
//            clipper.AddPaths(clips, PolyType.ptClip, true);

//            var solution = new List<Polygon>();

//            clipper.Execute(clipType, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

//            return solution;
//        }
//    }
//}