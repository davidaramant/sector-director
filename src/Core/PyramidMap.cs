// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core
{
    public static class PyramidMap
    {
        public static MapData Create()
        {
            var mapData = new MapData { NameSpace = "Doom" };

            const int pyramidLevels = 20;
            const int stepHeight = 16;
            const int stepWidth = 32;

            const int startingWidth = 2048;
            const int startingHeight = 512;

            foreach (var level in Enumerable.Range(0, pyramidLevels))
            {
                mapData.Sectors.Add(new Sector
                {
                    HeightFloor = level * stepHeight,
                    HeightCeiling = startingHeight,
                    TextureCeiling = "F_SKY1",
                    TextureFloor = "FLAT10",
                    LightLevel = 192,
                });

                int posOffset = stepWidth * level;

                mapData.Vertices.AddRange(new[]
                {
                    new Vertex(posOffset, posOffset),
                    new Vertex(startingWidth-posOffset, posOffset),
                    new Vertex(startingWidth-posOffset, startingWidth-posOffset),
                    new Vertex(posOffset, startingWidth-posOffset),
                });

                if (level == 0)
                {
                    mapData.SideDefs.Add(new SideDef(sector: 0, textureMiddle: "ASHWALL"));
                }
                else
                {
                    mapData.SideDefs.AddRange(new[]
                    {
                        new SideDef(sector:level-1,textureBottom:"STEPTOP"),
                        new SideDef(sector:level,textureBottom:"STEPTOP"),
                    });
                }

                int vertexOffset = level * 4;

                if (level == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mapData.LineDefs.Add(
                            new LineDef
                            {
                                V1 = (i + 1) % 4,
                                V2 = i,
                                SideFront = 0,
                                Blocking = true,
                            });
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mapData.LineDefs.Add(
                            new LineDef
                            {
                                V1 = vertexOffset + i,
                                V2 = vertexOffset + (i + 1) % 4,
                                SideFront = level * 2 - 1,
                                SideBack = level * 2,
                                TwoSided = true,
                            });
                    }
                }
            }

            var lastSectorId = mapData.Sectors.Count;
            mapData.Sectors.Add(new Sector(
                textureCeiling: "F_SKY1",
                textureFloor: "FLAT23",
                lightLevel: 192,
                heightCeiling: startingHeight,
                heightFloor: ((pyramidLevels - 1) * stepHeight) + 72));

            var halfWidth = startingWidth / 2d;

            var switchVertexStart = mapData.Vertices.Count;
            mapData.Vertices.AddRange(new[]
            {
                new Vertex(halfWidth-32,halfWidth-8),
                new Vertex(halfWidth+32,halfWidth-8),
                new Vertex(halfWidth+32,halfWidth+8),
                new Vertex(halfWidth-32,halfWidth+8),
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

            mapData.Things.Add(new Thing(
                type: 1,
                x: startingWidth / 2d,
                y: startingWidth / 2d));

            return mapData;
        }
    }
}