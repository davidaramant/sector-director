// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

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
            var parser = new UdmfParser(lexer);
            return UdmfSemanticAnalyzer.Process(parser.Parse());
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