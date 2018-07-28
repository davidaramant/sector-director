// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;

namespace SectorDirector.TestLauncher
{
    class Program
    {
        static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        static void Main(string[] args)
        {
            try
            {
                //LoadMap(new MapData());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadKey();
            }
        }
        private static void LoadMap(MapData mapData)
        {
            string wadFilePath = "demo.wad";

            var enginePath = GetEngineExePath();

            var wad = new WadFile();
            wad.Append(new Marker("E1M1"));
            wad.Append(new UdmfLump("TEXTMAP", mapData));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadFilePath);

            Process.Start(
                enginePath,
                $"-skill 4 -iwad doom.wad -warp 1 1");
        }

        private static string GetEngineExePath()
        {
            const string inputFile = "DoomEnginePath.txt";

            var pathsToCheck = new[]
            {
                ".",
                "..",
                DesktopPath,
            };

            foreach (var path in pathsToCheck)
            {
                var fullPath = Path.Combine(path, inputFile);

                if (File.Exists(fullPath))
                {
                    var enginePath = File.ReadAllLines(fullPath).Single().Trim();

                    if (Path.GetExtension(enginePath) != ".exe")
                    {
                        throw new ArgumentException("No EXE path found in the file.");
                    }

                    return enginePath;
                }
            }

            throw new ArgumentException(
                $"Could not find {inputFile}.  " +
                "Create this file in the output directory containing a single line with the full path to the Doom engine EXE (GZDoom, ec).  " +
                "Do not quote the path.");

        }
    }
}
