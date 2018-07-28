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
        private const int BorderRadius = 4096;
        private const int Bounds = 2048;

        public static MapData Create()
        {
            MapData map = new MapData { NameSpace = "Doom" };

            // build perimeter
            AddPerimeterCircle(map);

            // Put the player in a safe spot
            map.Things.Add(new Thing
            {
                X = 0,
                Y = -BorderRadius + 256,
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

            const int numLineSegments = 64;
            const int skyHackWidth = 2;
            const int borderWidth = 8;
            const int borderHeight = 48;

            map.Sectors.AddRange(new[]{
                new Sector(
                    textureFloor: "FLAT10",
                    textureCeiling: "F_SKY1",
                    heightFloor:borderHeight,
                    heightCeiling:borderHeight,
                    lightLevel: 0),
                new Sector(
                    textureFloor: "FLAT10",
                    textureCeiling: "F_SKY1",
                    heightFloor:borderHeight,
                    heightCeiling:255,
                    lightLevel: 192),
                new Sector(
                    textureFloor: "FLAT10",
                    textureCeiling: "F_SKY1",
                    heightFloor:0,
                    heightCeiling:255,
                    lightLevel: 192),
            });

            map.SideDefs.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));

            map.SideDefs.Add(new SideDef(sector: 1));
            map.SideDefs.Add(new SideDef(sector: 0));

            map.SideDefs.Add(new SideDef(sector: 2, textureBottom: "ASHWALL"));
            map.SideDefs.Add(new SideDef(sector: 1));


            // Make the outer ring
            foreach (var segmentIndex in Enumerable.Range(0, numLineSegments))
            {
                // reverse the angle to make sure we're generating linedefs in the correct order
                var angle = -((2 * PI / numLineSegments) * segmentIndex);
                map.Vertices.Add(VertexOnCircle(BorderRadius, angle));

                map.LineDefs.Add(new LineDef(
                    v1: segmentIndex,
                    v2: (segmentIndex + 1) % numLineSegments,
                    sideFront: 0,
                    blocking: true));
            }

            // Make the inner rings

            var ringOffsets = new[] { skyHackWidth, skyHackWidth + borderWidth };

            foreach (var (borderOffset, index) in ringOffsets.Select((offset, index) => (offset, index)))
            {
                int vertexIndexOffset = map.Vertices.Count;
                foreach (var segmentIndex in Enumerable.Range(0, numLineSegments))
                {
                    // reverse the angle to make sure we're generating linedefs in the correct order
                    var angle = -((2 * PI / numLineSegments) * segmentIndex);
                    map.Vertices.Add(VertexOnCircle(BorderRadius - borderOffset, angle));

                    map.LineDefs.Add(new LineDef(
                        v1: vertexIndexOffset + segmentIndex,
                        v2: vertexIndexOffset + ((segmentIndex + 1) % numLineSegments),
                        sideFront: 1 + (index*ringOffsets.Length),
                        sideBack: 1 + (index*ringOffsets.Length) + 1,
                        twoSided: true));
                }
            }
        }

    }
}