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

        public static Map GenerateMap(int shapeCount = DefaultCount)
        {
            var map = new Map();

            var random = new Random();
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

            return map;
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