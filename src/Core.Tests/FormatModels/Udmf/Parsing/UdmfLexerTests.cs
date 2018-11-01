﻿// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using Piglet.Lexer;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
using SectorDirector.Core.Tests.FormatModels.Common;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    [TestFixture]
    public sealed class UdmfLexerTests : BaseLexerTests
    {
        protected override ILexer<Token> GetDefinition()
        {
            return UdmfLexer.Definition;
        }
    }
}