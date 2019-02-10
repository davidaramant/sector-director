// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

using static System.Math;

namespace SectorDirector.Core
{
    public sealed class IslandTempleMapGenerator
    {
        const int PlayableRadius = 2048;
        const int SpiralLongestRadius = 1024;
        const int SpiralShortestRadius = 512;
        const int PlateauRadius = 384;

        const int Height = 2048;

        const int Bounds = 2048;

        public static MapData Create()
        {
            MapData map = new MapData { NameSpace = "Doom" };
  
            AddPerimeter(map);
            AddExitSwitch(map);
            CreateSpiralCenter(map);

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

        static Vertex VertexOnCircle(int radius, double angle) =>
            new Vertex(
                x: radius * Cos(angle),
                y: radius * Sin(angle));

        static double GetCircleAngle(int step, int stepCount) => (2 * PI / stepCount) * step;

        static void AddPerimeter(MapData map)
        {
            const int skyHackWidth = 1;
            const int waterWidth = 2048;
            const int borderWallWidth = 32;
            const int waterNumLineSegments = 32;
            const int borderWallHeight = 24;
            const int waterHeight = -8;

            var random = new Random();

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
                    textureFloor: "LAVA1",
                    textureCeiling: "F_SKY1",
                    heightFloor:waterHeight,
                    heightCeiling:Height,
                    lightLevel: 255),
                // border wall - 2
                new Sector(
                    textureFloor: "FLOOR6_2",
                    textureCeiling: "F_SKY1",
                    heightFloor:borderWallHeight,
                    heightCeiling:Height,
                    lightLevel: 144),
                // main area - 3
                new Sector(
                    textureFloor: "FLAT5_7",
                    textureCeiling: "F_SKY1",
                    heightFloor:0,
                    heightCeiling:Height,
                    lightLevel: 144),
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
                map.Vertices.Add(VertexOnCircle(outerRadius, -GetCircleAngle(segmentIndex, waterNumLineSegments)));

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
                    var angle = startAngle - GetCircleAngle(segmentIndex, numInnerLineSegments);

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

        static void AddExitSwitch(MapData mapData)
        {
            var lastSectorId = mapData.Sectors.Count;
            mapData.Sectors.Add(new Sector(
                textureCeiling: "F_SKY1",
                textureFloor: "FLAT23",
                lightLevel: 192,
                heightCeiling: Height,
                heightFloor: 72));

            var yOffset = -PlayableRadius + 128;

            var switchVertexStart = mapData.Vertices.Count;
            mapData.Vertices.AddRange(new[]
            {
                new Vertex(-32,yOffset-8),
                new Vertex(+32,yOffset-8),
                new Vertex(+32,yOffset+8),
                new Vertex(-32,yOffset+8),
            });

            var switchFrontSD = mapData.SideDefs.Count;
            var switchSideSD = switchFrontSD + 1;
            var switchInsideSD = switchSideSD + 1;
            mapData.SideDefs.AddRange(new[]
            {
                new SideDef(sector:lastSectorId-1,textureBottom:"SW1COMM"),
                new SideDef(sector:lastSectorId-1, textureBottom:"SHAWN2"),
                new SideDef(sector:lastSectorId),

            });

            mapData.LineDefs.AddRange(new[]
            {
                new LineDef(v1:switchVertexStart,  v2:switchVertexStart+1, sideFront:switchFrontSD,sideBack:switchInsideSD,twoSided:true,special:11),
                new LineDef(v1:switchVertexStart+1,v2:switchVertexStart+2, sideFront:switchSideSD, sideBack:switchInsideSD,twoSided:true),
                new LineDef(v1:switchVertexStart+2,v2:switchVertexStart+3, sideFront:switchSideSD, sideBack:switchInsideSD,twoSided:true),
                new LineDef(v1:switchVertexStart+3,v2:switchVertexStart,   sideFront:switchSideSD, sideBack:switchInsideSD,twoSided:true),
            });

        }
        
        static double GetDistance(Vertex v1, Vertex v2) => Sqrt(Pow(v1.X - v2.X, 2) + Pow(v1.Y - v2.Y, 2));

        static void CreateSpiralCenter(MapData map)
        {
            const int numberOfSteps = 128;
            const int plateauHeight = 1024;
            const int textureWidth = 128;

            int insideAreaSectorId = map.Sectors.Count - 2; // HACK: Take into account exit switch
            int plateauSectorId = map.Sectors.Count;
            map.Sectors.Add(new Sector(
                textureFloor: "FLOOR6_1",
                textureCeiling: "F_SKY1",
                heightFloor: plateauHeight,
                heightCeiling: Height,
                lightLevel: 176));

            var vertexOffset = map.Vertices.Count;
            int plateauTextureOffset = 0;
            foreach (var segmentIndex in Enumerable.Range(0, numberOfSteps))
            {
                var currentVertex = VertexOnCircle(PlateauRadius, GetCircleAngle(segmentIndex, numberOfSteps));
                map.Vertices.Add(currentVertex);

                if (segmentIndex > 0)
                {
                    var plateauSideLength = (int)GetDistance(
                        currentVertex,
                        VertexOnCircle(PlateauRadius, GetCircleAngle(segmentIndex + 1, numberOfSteps)));
                    plateauTextureOffset += plateauSideLength;
                    plateauTextureOffset %= textureWidth;
                }

                int sideDefId = map.SideDefs.Count;
                map.SideDefs.AddRange(new[]
                {
                    new SideDef(sector: insideAreaSectorId, textureBottom: "ROCKRED1", offsetX:plateauTextureOffset),
                    new SideDef(sector:plateauSectorId),
                });

                map.LineDefs.Add(new LineDef(
                    v1: vertexOffset + segmentIndex,
                    v2: vertexOffset + (segmentIndex + 1) % numberOfSteps,
                    sideFront: sideDefId,
                    sideBack: sideDefId + 1,
                    twoSided: true));
            }
        }
    }
}