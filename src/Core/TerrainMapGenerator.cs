// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

using static System.Math;

namespace SectorDirector.Core
{
    public sealed class TerrainMapGenerator
    {
        const int PlayableRadius = 4096;

        const int Height = 2048;

        public static MapData Create()
        {
            MapData map = new MapData { NameSpace = "Doom" };

            AddPerimeter(map);

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

            map.Sectors.AddRange(new[]{
                // Sky hack sector - 0
                new Sector(
                    textureFloor: "FWATER1",
                    textureCeiling: "F_SKY1",
                    heightFloor:0,
                    heightCeiling:0,
                    lightLevel: 0),
                // water border - 1
                new Sector(
                    textureFloor: "FWATER1",
                    textureCeiling: "F_SKY1",
                    heightFloor:0,
                    heightCeiling:Height,
                    lightLevel: 160),
            });

            map.SideDefs.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));

            map.SideDefs.Add(new SideDef(sector: 0));
            map.SideDefs.Add(new SideDef(sector: 1));


            var outerRadius = PlayableRadius + skyHackWidth;

            // Make the outer ring
            const int waterNumLineSegments = 64;
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

            // Make the inner ring
            int vertexIndexOffset = map.Vertices.Count;
            foreach (var segmentIndex in Enumerable.Range(0, waterNumLineSegments))
            {
                map.Vertices.Add(VertexOnCircle(PlayableRadius, -GetCircleAngle(segmentIndex, waterNumLineSegments)));

                map.LineDefs.Add(new LineDef(
                    v1: vertexIndexOffset + segmentIndex,
                    v2: vertexIndexOffset + ((segmentIndex + 1) % waterNumLineSegments),
                    sideFront: 2,
                    sideBack: 1,
                    twoSided: true,
                    blocking: true,
                    dontDraw: true));
            }
        }

        static double GetDistance(Vertex v1, Vertex v2) => Sqrt(Pow(v1.X - v2.X, 2) + Pow(v1.Y - v2.Y, 2));
    }
}