// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf;
using Is = NUnit.DeepObjectCompare.Is;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    [TestFixture]
    public sealed class UdmfSemanticAnalyzerTests
    {
        [Test]
        public void ShouldParseGlobalFields()
        {
            var map = new MapData()
            {
                NameSpace = "NameSpace",
                Comment = "Comment",
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownGlobalFields()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                UnknownProperties = { new UnknownProperty(new Identifier("user_global"), "\"someValue\"") }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseBlocks()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                Vertices = { new Vertex(1, 2) }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownBlocks()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                UnknownBlocks =
                {
                    new UnknownBlock(new Identifier("SomeWeirdBlock"))
                    {
                        Properties = 
                        {
                            new UnknownProperty(new Identifier("id1"), "1" ),
                            new UnknownProperty(new Identifier("id2"), "true" ),
                        }
                    }
                }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownFieldsInBlock()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                Vertices = { new Vertex(1, 2)
                {
                    UnknownProperties =
                    {
                        new UnknownProperty(new Identifier("id1"), "1" ),
                        new UnknownProperty(new Identifier("id2"), "true" ),
                    }
                } },
            };

            AssertRoundTrip(map);
        }


        [Test]
        public void ShouldRoundTripDemoMap()
        {
            AssertRoundTrip(DemoMap.Create());
        }

        static void AssertRoundTrip(MapData map)
        {
            var roundTripped = RoundTrip(map);

            Assert.That(roundTripped, Is.DeepEqualTo(map));
        }

        static MapData RoundTrip(MapData map)
        {
            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                return MapData.LoadFrom(stream);
            }
        }

    }
}
