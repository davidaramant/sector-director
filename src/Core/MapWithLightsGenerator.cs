// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

using static System.Math;

namespace SectorDirector.Core
{
    public sealed class MapWithLightsGenerator
    {
        const int PlayableRadius = 2048;

        const int Height = 255;

        const int Bounds = 2048;

        public static MapData Create()
        {
            MapData map = new MapData { NameSpace = "Doom" };

            // build perimeter
            AddPerimeterCircle(map);

            // Put the player in a safe spot
            map.Things.Add(new Thing
            {
                X = 0,
                Y = -PlayableRadius + 64,
                Angle = 90,
                Type = 1,
                Skill1 = true,
                Skill2 = true,
                Skill3 = true,
                Skill4 = true,
                Skill5 = true,
                Single = true,
                Dm = true,
                Coop = true,
            });

            return map;
        }

        static void AddPerimeterCircle(MapData map)
        {
            Vertex VertexOnCircle(int radius, double angle) =>
                new Vertex(
                    x: radius * Cos(angle),
                    y: radius * Sin(angle));

            const int skyHackWidth = 1;
            const int waterWidth = 2048;
            const int borderWallWidth = 32;
            const int waterNumLineSegments = 32;
            const int borderWallHeight = 24;
            const int waterHeight = -8;

            map.Sectors.AddRange(new[]{
                // Sky hack sector - 0
                new Sector(
                    textureFloor: "FLAT10",
                    textureCeiling: "F_SKY1",
                    heightFloor:waterHeight,
                    heightCeiling:waterHeight,
                    lightLevel: 0),
                // water border - 1
                new Sector(
                    textureFloor: "FWATER1",
                    textureCeiling: "F_SKY1",
                    heightFloor:waterHeight,
                    heightCeiling:Height,
                    lightLevel: 192),
                // border wall - 2
                new Sector(
                    textureFloor: "FLOOR6_2",
                    textureCeiling: "F_SKY1",
                    heightFloor:borderWallHeight,
                    heightCeiling:Height,
                    lightLevel: 192),
                // main area - 3
                new Sector(
                    textureFloor: "FLAT5_7",
                    textureCeiling: "F_SKY1",
                    heightFloor:0,
                    heightCeiling:Height,
                    lightLevel: 192),
            });

            map.SideDefs.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));

            map.SideDefs.Add(new SideDef(sector: 0));
            map.SideDefs.Add(new SideDef(sector: 1));

            map.SideDefs.Add(new SideDef(sector: 1, textureBottom: "ASHWALL"));
            map.SideDefs.Add(new SideDef(sector: 2));

            map.SideDefs.Add(new SideDef(sector: 2));
            map.SideDefs.Add(new SideDef(sector: 3, textureBottom: "ASHWALL"));

            var outerRadius = PlayableRadius + borderWallWidth + waterWidth + skyHackWidth;

            // Make the outer ring
            foreach (var segmentIndex in Enumerable.Range(0, waterNumLineSegments))
            {
                // reverse the angle to make sure we're generating linedefs in the correct order
                var angle = -((2 * PI / waterNumLineSegments) * segmentIndex);
                map.Vertices.Add(VertexOnCircle(outerRadius, angle));

                map.LineDefs.Add(new LineDef(
                    v1: segmentIndex,
                    v2: (segmentIndex + 1) % waterNumLineSegments,
                    sideFront: 0,
                    blocking: true,
                    dontDraw: true));
            }

            // Make the inner rings
            var ringOffsets = new[]
            {
                PlayableRadius + borderWallWidth + waterWidth,
                PlayableRadius + borderWallWidth,
                PlayableRadius,
            };

            var random = new Random();
            foreach (var (radius, borderIndex) in ringOffsets.Select((radius, index) => (radius, index)))
            {
                var numInnerLineSegments = waterNumLineSegments;
                var startAngle = 0d;

                if (borderIndex > 0)
                {
                    var lineSegmentVariance = 64;
                    var baseLineSegments = 256;
                    numInnerLineSegments = random.Next(
                        baseLineSegments - lineSegmentVariance / 2,
                        baseLineSegments + lineSegmentVariance / 2);

                    startAngle = random.NextDouble() * (2 * PI);
                }

                int vertexIndexOffset = map.Vertices.Count;
                foreach (var segmentIndex in Enumerable.Range(0, numInnerLineSegments))
                {
                    var angle = startAngle - ((2 * PI / numInnerLineSegments) * segmentIndex);

                    var radiusOffset = 0;
                    if (borderIndex > 0)
                    {
                        var radiusVariance = 24;
                        radiusOffset = random.Next(-radiusVariance / 2, radiusVariance / 2);
                    }

                    map.Vertices.Add(VertexOnCircle(radius + radiusOffset, angle));

                    map.LineDefs.Add(new LineDef(
                        v1: vertexIndexOffset + segmentIndex,
                        v2: vertexIndexOffset + ((segmentIndex + 1) % numInnerLineSegments),
                        sideFront: 1 + (borderIndex * 2) + 1,
                        sideBack: 1 + (borderIndex * 2),
                        twoSided: true,
                        blocking: true,
                        dontDraw: borderIndex == 0));
                }
            }
        }

    }
}