// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
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

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var lexer = new UdmfLexer(textReader);
                    var parser = new UdmfParser(lexer);
                    return UdmfSemanticAnalyzer.Process(parser.Parse());
                }
            }
        }

    }
}
