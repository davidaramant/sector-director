// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using SectorDirector.Core.FormatModels.LogicalMap;
using SectorDirector.Core.FormatModels.Udmf;
using System.Linq;

namespace SectorDirector.Core.Tests.FormatModels.LogicalMap
{
    [TestFixture, Parallelizable]
    public class SectorGraphTests
    {
        [Test]
        public void ShouldGenerateMultipleSubSectorsWhenSectorHasUnconnectedAreas()
        {
            // This map has one sector composed of two squares that are not connected at all
            var map = new MapData
            {
                NameSpace = "Doom",
                Vertices =
                {
                    new Vertex // 0
                    {
                        X = 0,
                        Y = 0,
                    },
                    new Vertex // 1
                    {
                        X = 100,
                        Y = 0,
                    },
                    new Vertex // 2
                    {
                        X = 100,
                        Y = 100,
                    },
                    new Vertex // 3
                    {
                        X = 0,
                        Y = 100,
                    },
                    new Vertex // 4
                    {
                        X = 1000,
                        Y = 0,
                    },
                    new Vertex // 5
                    {
                        X = 1100,
                        Y = 0,
                    },
                    new Vertex // 6
                    {
                        X = 1100,
                        Y = 100,
                    },
                    new Vertex // 7
                    {
                        X = 1000,
                        Y = 100,
                    },
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
                        V1 = 1,
                        V2 = 2,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 2
                    {
                        V1 = 2,
                        V2 = 3,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 3
                    {
                        V1 = 3,
                        V2 = 0,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 4
                    {
                        V1 = 4,
                        V2 = 5,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 5
                    {
                        V1 = 5,
                        V2 = 6,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 6
                    {
                        V1 = 6,
                        V2 = 7,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 7
                    {
                        V1 = 7,
                        V2 = 4,
                        SideFront = 0,
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
                },
                Things =
                {
                    new Thing
                    {
                        X = 50,
                        Y = 50,
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

            var graph = SectorGraph.BuildFrom(map);

            Assert.That(graph.LogicalSectors, Has.Count.EqualTo(1));
            Assert.That(graph.LogicalSectors.First(), Has.Count.EqualTo(2));
        }

        [Test]
        public void ShouldGenerateMultipleSubSectorsWhenSectorHasTwoConnectedAreas()
        {
            // This map has one sector composed of two squares that are connected
            var map = new MapData
            {
                NameSpace = "Doom",
                Vertices =
                {
                    new Vertex // 0
                    {
                        X = 0,
                        Y = 0,
                    },
                    new Vertex // 1
                    {
                        X = 100,
                        Y = 0,
                    },
                    new Vertex // 2
                    {
                        X = 200,
                        Y = 0,
                    },
                    new Vertex // 3
                    {
                        X = 200,
                        Y = 100,
                    },
                    new Vertex // 4
                    {
                        X = 100,
                        Y = 100,
                    },
                    new Vertex // 5
                    {
                        X = 0,
                        Y = 100,
                    },
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
                        V1 = 1,
                        V2 = 2,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 2
                    {
                        V1 = 2,
                        V2 = 3,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 3
                    {
                        V1 = 3,
                        V2 = 4,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 4
                    {
                        V1 = 4,
                        V2 = 5,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 5
                    {
                        V1 = 5,
                        V2 = 0,
                        SideFront = 0,
                        Blocking = true,
                    },
                    new LineDef // 6
                    {
                        V1 = 1,
                        V2 = 4,
                        SideFront = 1,
                        SideBack = 1,
                        TwoSided = true,
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
                        Sector = 0,
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
                },
                Things =
                {
                    new Thing
                    {
                        X = 50,
                        Y = 50,
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

            var graph = SectorGraph.BuildFrom(map);

            Assert.That(graph.LogicalSectors, Has.Count.EqualTo(1));
            Assert.That(graph.LogicalSectors.First(), Has.Count.EqualTo(2));
        }
    }
}
