// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SectorDirector.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionBasePath = Path.Combine(Enumerable.Repeat("..", 5).ToArray());
            var corePath = Path.Combine(solutionBasePath, "Core");
            var udmfPath = Path.Combine(corePath, "FormatModels", "Udmf");
            var udmfParsingPath = Path.Combine(udmfPath, "Parsing");

            // Generate HIME lexer / parser
            using (var himeProcess = Process.Start(
                Path.Combine(solutionBasePath, "..", "hime", "himecc.bat"),
                "Udmf.gram"))
            {
                // Create data model
                UdmfModelGenerator.WriteToPath(udmfPath);

                // Generate mapping of HIME output to data model
                //using (var parserStream = File.CreateText(Path.Combine(udmfParsingPath, "UdmfParser.Generated.cs")))
                //{
                //    UdmfParserGenerator.WriteTo(parserStream);
                //}


                himeProcess.WaitForExit();
                if (himeProcess.ExitCode != 0)
                {
                    Console.ReadLine();
                }
                else
                {
                    void CopyFileToParsingPath(string fileName)
                    {
                        File.Copy(fileName, Path.Combine(udmfParsingPath, fileName), overwrite:true);
                    }

                    CopyFileToParsingPath("UdmfLexer.cs");
                    CopyFileToParsingPath("UdmfParser.cs");
                    CopyFileToParsingPath("UdmfLexer.bin");
                    CopyFileToParsingPath("UdmfParser.bin");
                }
            }


        }
    }
}
