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
        public void ShouldHandleParsingDemoMap()
        {
            var map = DemoMap.Create();

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var lexer = new UdmfLexer(textReader);
                    var parser = new UdmfParser(lexer);

                    ParseResult result = parser.Parse();
                    Assert.That(result.IsSuccess, Is.True);
                }
            }
        }
    }
}