﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Pidgin;
using System.IO;
using System.Text;
using SectorDirector.Core.FormatModels.Udmf.Parsing;

namespace SectorDirector.Core.FormatModels.Udmf
{
    public sealed partial class MapData
    {
        public static MapData LoadFrom(TextReader reader)
        {
            var lexer = new UdmfLexer(reader);
            var result = UdmfParser.TranslationUnit.ParseOrThrow(lexer.Scan());
            return UdmfSemanticAnalyzer.Process(result);
        }

        public static MapData LoadFrom(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFrom(textReader);
            }
        }
    }
}