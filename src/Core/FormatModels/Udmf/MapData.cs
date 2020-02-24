// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using SectorDirector.Core.FormatModels.Udmf.Parsing;

namespace SectorDirector.Core.FormatModels.Udmf
{
    public sealed partial class MapData
    {
        public double MinX => Vertices.Min(p => p.X);
        public double MaxX => Vertices.Max(p => p.X);
        public double MinY => Vertices.Min(p => p.Y);
        public double MaxY => Vertices.Max(p => p.Y);
        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public static MapData LoadFrom(TextReader reader)
        {
            var lexer = new UdmfLexer(reader);
            var result = UdmfParser.Parse(lexer.Scan());
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