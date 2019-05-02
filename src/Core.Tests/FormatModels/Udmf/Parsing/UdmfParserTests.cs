// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
using SectorDirector.Core.FormatModels.Udmf.Parsing.AbstractSyntaxTree;
using System.IO;
using System.Linq;
using System.Text;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldParseAssignment()
        {
            var tokenStream = new Token[]
            {
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("id")),
                new EqualsToken(FilePosition.StartOfFile()),
                new IntegerToken(FilePosition.StartOfFile(), 5),
                new SemicolonToken(FilePosition.StartOfFile()),
            };

            var results = UdmfParser.Parse(tokenStream).ToArray();
            Assert.That(results, Has.Length.EqualTo(1));
            Assert.That(results[0], Is.TypeOf<Assignment>());
        }

        [Test]
        public void ShouldParseBlock()
        {
            var tokenStream = new Token[]
            {
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("blockName")),
                new OpenBraceToken(FilePosition.StartOfFile()),
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("id")),
                new EqualsToken(FilePosition.StartOfFile()),
                new IntegerToken(FilePosition.StartOfFile(), 5),
                new SemicolonToken(FilePosition.StartOfFile()),
                new CloseBraceToken(FilePosition.StartOfFile()),
            };

            var results = UdmfParser.Parse(tokenStream).ToArray();
            Assert.That(results, Has.Length.EqualTo(1));
            Assert.That(results[0], Is.TypeOf<Block>());
            Assert.That(((Block)results[0]).Fields, Has.Length.EqualTo(1));
        }

        [Test]
        public void ShouldHandleParsingDemoMap()
        {
            var map = DemoMap.Create();

            using (var fs = File.OpenWrite(Path.Combine(TestContext.CurrentContext.TestDirectory, "text.udmf")))
            {
                map.WriteTo(fs);
            }

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var lexer = new UdmfLexer(textReader);
                    var result = UdmfParser.Parse(lexer.Scan()).ToArray();
                }
            }
        }
    }
}