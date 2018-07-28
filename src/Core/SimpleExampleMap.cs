// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core
{
    public static class SimpleExampleMap
    {
        public static MapData Create()
        {
            return new MapData
            {
                NameSpace = "Doom",
                Vertices =
                {
                    new Vertex(-320,-128),
                    new Vertex(-320,128),
                    new Vertex(-320,384),
                    new Vertex(-64,384),
                    new Vertex(-64,128),
                    new Vertex(-64,-128),
                },
                LineDefs =
                {
                    new LineDef // 0
                    {
                    V1 = 0,
                    V2 = 1,
                    SideFront = 0,
                    Blocking = true,
                    },

                    new LineDef // 1
                    {
                        V1 = 2,
                        V2 = 3,
                        SideFront = 1,
                        Blocking = true,
                    },

                    new LineDef // 2
                    {
                        V1 = 4,
                        V2 = 5,
                        SideFront = 2,
                        Blocking = true,
                    },

                    new LineDef // 3
                    {
                        V1 = 5,
                        V2 = 0,
                        SideFront = 3,
                        Blocking = true,
                    },

                    new LineDef // 4
                    {
                        V1 = 1,
                        V2 = 2,
                        SideFront = 4,
                        Blocking = true,
                    },

                    new LineDef // 5
                    {
                        V1 = 1,
                        V2 = 4,
                        SideFront = 5,
                        SideBack = 6,
                        TwoSided = true,
                    },

                    new LineDef // 6
                    {
                        V1 = 3,
                        V2 = 4,
                        SideFront = 7,
                        Blocking = true,
                    },
                },
                SideDefs =
                {
                    new SideDef // 0
                    {
                        Sector = 0,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 1
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 2
                    {
                        Sector = 0,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 3
                    {
                        Sector = 0,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 4
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 5
                    {
                        Sector = 0,
                        TextureTop = "STEPTOP",
                        TextureBottom = "STEPTOP",
                    },

                    new SideDef // 6
                    {
                        Sector = 1,
                    },

                    new SideDef // 7
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },
                },
                Sectors =
                {
                    new Sector
                    {
                        HeightFloor = 0,
                        HeightCeiling = 128,
                        TextureFloor = "FLOOR0_1",
                        TextureCeiling = "CEIL1_1",
                        LightLevel = 192,
                    },
                    new Sector
                    {
                        HeightFloor = 16,
                        HeightCeiling = 112,
                        TextureFloor = "FLOOR0_1",
                        TextureCeiling = "CEIL1_1",
                        LightLevel = 160,
                    },
                },
                Things =
                {
                    new Thing
                    {
                        X = -192,
                        Y = 0,
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
                    }
                }
            };
        }
    }
}