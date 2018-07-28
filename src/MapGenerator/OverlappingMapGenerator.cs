﻿// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using SectorDirector.MapGenerator.Data;

using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Polygons = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace SectorDirector.MapGenerator
{
    public static class OverlappingMapGenerator
    {
        private const int DefaultCount = 3;
        private const int RadiusMin = 80;
        private const int RadiusMax = 140;
        private const int CenterMin = 200;
        private const int CenterMax = 620;

        public static Map GenerateMap(int shapeCount = DefaultCount, int? seed = null)
        {
            var map = new Map();

            var random = new Random();
            if (!seed.HasValue)
            {
                seed = random.Next(Int32.MaxValue);
            }

            Console.WriteLine("Using Seed: {0}", seed.Value);
            random = new Random(seed.Value);

            var allShapes = new Polygons();

            for (int i = 0; i < shapeCount; i++)
            {
                var sides = (random.NextDouble() > 0.5) ? 200 : random.Next(3, 8);
                var radius = random.Next(RadiusMin, RadiusMax);
                var centerX = random.Next(CenterMin, CenterMax);
                var centerY = random.Next(CenterMin, CenterMax);

                var shape = ShapeGenerator.GenerateRegularShape(sides, radius, new IntPoint(centerX, centerY));
                var layerShapes = new Polygons();

                if (i == 0)
                {
                    layerShapes.Add(shape);
                }
                else
                {
                    layerShapes.AddRange(SubtractShape(shape, allShapes));
                }

                allShapes.AddRange(layerShapes);
                map.Layers.Add(new Layer(
                    depth: shapeCount - i,
                    shapes: layerShapes.Select(polygon => new Shape(polygon))
                ));
            }

            map.BoundingShape = new Shape(BuildBoundingShape(map));

            var outerPerimeter = OuterPerimeter(map).Select(polygon => new Shape(polygon));
            //map.Layers.Add(new Layer(
            //    depth: 0,
            //    shapes: outerPerimeter));
            map.OuterShapes.AddRange(outerPerimeter);

            PrintLayerPoints(map);

            return map;
        }

        private static void PrintLayerPoints(Map map)
        {
            for (var layerIndex = 0; layerIndex < map.Layers.Count; layerIndex++)
            {
                var layer = map.Layers[layerIndex];
                for (var shapeIndex = 0; shapeIndex < layer.Shapes.Count; shapeIndex++)
                {
                    var shape = layer.Shapes[shapeIndex];

                    Console.WriteLine("Layer #{0}, Shape #{1}, Points: {2}", layerIndex, shapeIndex,
                        String.Join(",", shape.Polygon.Select(point => $"({point.X},{point.Y})").ToArray()));
                }
            }
        }

        private static Polygons OuterPerimeter(Map map)
        {
            var boundingShape = BuildBoundingShape(map);

            var allShapes = new Polygons();
            foreach (var layer in map.Layers)
            {
                foreach (var shape in layer.Shapes)
                {
                    allShapes.Add(shape.Polygon);
                }
            }

            return SubtractShape(boundingShape, allShapes);
        }

        private static Polygon BuildBoundingShape(Map map)
        {
            var minimumX = map.Vertices.Select(vertex => vertex.X).Min();
            var maximumX = map.Vertices.Select(vertex => vertex.X).Max();
            var minimumY = map.Vertices.Select(vertex => vertex.Y).Min();
            var maximumY = map.Vertices.Select(vertex => vertex.Y).Max();

            var boundingShape = new Polygon
            {
                new IntPoint(minimumX - 35, minimumY - 35),
                new IntPoint(minimumX - 35, maximumY + 35),
                new IntPoint(maximumX + 35, maximumY + 35),
                new IntPoint(maximumX + 35, minimumY - 35),
            };

            return boundingShape;
        }

        private static Polygons SubtractShape(Polygon subject, Polygons clips)
        {
            return SubtractShape(new Polygons { subject }, clips);
        }

        private static Polygons SubtractShape(Polygons subjects, Polygons clips)
        {
            return ProcessShapes(subjects, clips, ClipType.ctDifference);
        }

        private static Polygons IntersectShape(Polygon subject, Polygons clips)
        {
            return ProcessShapes(new Polygons { subject }, clips, ClipType.ctIntersection);
        }

        private static Polygons ProcessShapes(Polygons subjects, Polygons clips, ClipType clipType)
        {
            var clipper = new Clipper();
            clipper.AddPaths(subjects, PolyType.ptSubject, true);
            clipper.AddPaths(clips, PolyType.ptClip, true);

            var solution = new List<Polygon>();

            clipper.Execute(clipType, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return solution;
        }
    }
}