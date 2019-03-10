﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;

namespace SectorDirector.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionBasePath = Path.Combine(Enumerable.Repeat("..", 5).ToArray());
            var udmfPath = Path.Combine(solutionBasePath, "Core", "FormatModels", "Udmf");

            UdmfModelGenerator.WriteToPath(udmfPath);

            using (var parserStream = File.CreateText(Path.Combine(udmfPath, "Parsing", "UdmfParser.Generated.cs")))
            {
                UdmfParserGenerator.WriteTo(parserStream);
            }
        }
    }
}
