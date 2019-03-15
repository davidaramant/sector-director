// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
using Token = Hime.Redist.Token;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    [TestFixture]
    public sealed class UdmfLexerTests
    {
        [TestCase("true", "KEYWORD")]
        [TestCase("false", "KEYWORD")]
        [TestCase("1", "INTEGER")]
        [TestCase("-1", "INTEGER")]
        [TestCase("0x1234abcd", "INTEGER")]
        [TestCase("1.24", "FLOAT")]
        [TestCase("\"some string\"", "QUOTED_STRING")]
        [TestCase("A_identifier", "IDENTIFIER")]
        public void ShouldLexValues(string input, string expectedSymbolType)
        {
            var lexer = new UdmfLexer(input);
            var results = lexer.Output.ToList();
            Assert.That(results.Select(t => t.Symbol.Name), 
                Is.EqualTo(new[] { expectedSymbolType }));
        }


        [TestCase("someProperty = 10;")]
        [TestCase("      someProperty=10;")]
        [TestCase("      someProperty  = 10 ;")]
        public void ShouldIgnoreWhitespace(string input)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    Token.Integer(10),
            //    Token.Semicolon);
        }

        [TestCase("someProperty = 0;", 0)]
        [TestCase("someProperty = 10;", 10)]
        [TestCase("someProperty = 0010;", 10)]
        [TestCase("someProperty = 0xa;", 10)]
        [TestCase("someProperty = +10;", 10)]
        [TestCase("someProperty = -10;", -10)]
        public void ShouldLexIntegers(string input, int value)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    Token.Integer(value),
            //    Token.Semicolon);
        }

        [TestCase("someProperty = 10.5;", 10.5)]
        [TestCase("someProperty = +10.5;", 10.5)]
        [TestCase("someProperty = 1.05e1;", 10.5)]
        [TestCase("someProperty = 1e10;", 1e10)]
        public void ShouldLexDoubles(string input, double value)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    Token.Double(value),
            //    Token.Semicolon);
        }

        [TestCase("someProperty = true;", true)]
        [TestCase("someProperty = false;", false)]
        public void ShouldLexBooleans(string input, bool boolValue)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    boolValue ? Token.BooleanTrue : Token.BooleanFalse,
            //    Token.Semicolon);
        }

        [TestCase("someProperty = \"true\";", "true")]
        [TestCase("someProperty = \"0xFB010304\";", "0xFB010304")]
        [TestCase("someProperty = \"\";", "")]
        public void ShouldLexStrings(string input, string stringValue)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    Token.String(stringValue),
            //    Token.Semicolon);
        }

        [Test]
        public void ShouldLexEmptyBlock()
        {
            //VerifyLexing("block { }",
            //    Token.Identifier("block"),
            //    Token.OpenParen,
            //    Token.CloseParen);
        }

        [Test]
        public void ShouldLexBlockWithAssignments()
        {
            //VerifyLexing("block { id1 = 1; id2 = false; }",
            //    Token.Identifier("block"),
            //    Token.OpenParen,
            //    Token.Identifier("id1"),
            //    Token.Equal,
            //    Token.Integer(1),
            //    Token.Semicolon,
            //    Token.Identifier("id2"),
            //    Token.Equal,
            //    Token.BooleanFalse,
            //    Token.Semicolon,
            //    Token.CloseParen);
        }

        [Test]
        public void ShouldLexBlockWithArrays()
        {
            //VerifyLexing("block { {1,2},{3,4} }",
            //    Token.Identifier("block"),
            //    Token.OpenParen,

            //    Token.OpenParen,
            //    Token.Integer(1),
            //    Token.Comma,
            //    Token.Integer(2),
            //    Token.CloseParen,

            //    Token.Comma,

            //    Token.OpenParen,
            //    Token.Integer(3),
            //    Token.Comma,
            //    Token.Integer(4),
            //    Token.CloseParen,

            //    Token.CloseParen);
        }

        [TestCase("// Comment\r\nsomeProperty = 10;")]
        [TestCase("// Comment\nsomeProperty = 10;")]
        [TestCase("someProperty = 10; // Comment")]
        public void ShouldIgnoreComments(string input)
        {
            //VerifyLexing(input,
            //    Token.Identifier("someProperty"),
            //    Token.Equal,
            //    Token.Integer(10),
            //    Token.Semicolon);
        }

        private void VerifyLexing(string input, string expectedType)
        {

        }
    }
}