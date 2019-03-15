// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Text;
using Hime.Redist;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
using Is = NUnit.DeepObjectCompare.Is;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldParseExpression()
        {
            var lexer = new UdmfLexer("someProperty = 10;");
            var parser = new UdmfParser(lexer);
            // Executes the parsing
            ParseResult result = parser.Parse();
            
            
        }

        [Test]
        public void ShouldRoundTripDemoMap()
        {
            var map = DemoMap.Create();

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var sa = new UdmfSyntaxAnalyzer();
                    //var roundTripped = UdmfParser.Parse(sa.Analyze(new UdmfLexer(textReader)));

                    //Assert.That(roundTripped, Is.DeepEqualTo(map));                   
                    throw new NotImplementedException("Switch over to new parser");
                }
            }
        }

        [Test]
        public void ShouldParseOldDemoMap()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Udmf", "Parsing", "TEXTMAP.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UdmfSyntaxAnalyzer();
                //var map = UdmfParser.Parse(sa.Analyze(new UdmfLexer(textReader)));
                throw new NotImplementedException("Switch over to new parser");
            }
        }
    }
}