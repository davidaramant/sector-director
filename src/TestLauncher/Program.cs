// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SectorDirector.Core;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;
using SectorDirector.MapGenerator;

namespace SectorDirector.TestLauncher
{
    class Program
    {
        static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        static void Main(string[] args)
        {
            try
            {
                var circleMap = OverlappingMapGenerator.GenerateMap(100, PolygonTypes.OnlyCircles);
                var polygonMap = OverlappingMapGenerator.GenerateMap(100, PolygonTypes.OnlyPolygons);
                var mixedMap1 = OverlappingMapGenerator.GenerateMap(100, PolygonTypes.Everything);
                var mixedMap2 = OverlappingMapGenerator.GenerateMap(100, PolygonTypes.Everything);
                var bossMap = OverlappingMapGenerator.GenerateMap(100, PolygonTypes.Everything, includeBosses: true);

                //ImageExporter.CreateImage(generatedMap, "exported-map.svg", true);
                //Process.Start("exported-map.svg");

                LoadMaps(
                    SimpleExampleMap.Create(),         // M1
                    PyramidMap.Create(),               // M2
                    IslandTempleMapGenerator.Create(), // M3
                    MapConverter.Convert(circleMap),   // M4
                    MapConverter.Convert(polygonMap),  // M5
                    MapConverter.Convert(mixedMap1),   // M6
                    MapConverter.Convert(mixedMap2),   // M7
                    MapConverter.Convert(bossMap)      // M8
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadKey();
            }
        }

        private static void LoadMaps(params MapData[] maps)
        {
            string wadFilePath = "demo.wad";

            var enginePath = GetEngineExePath();

            var wad = new WadFile();

            foreach (var (map, index) in maps.Select((map, index) => (map, index)))
            {
                wad.Append(new Marker($"E1M{index + 1}"));
                wad.Append(new UdmfLump("TEXTMAP", map));
                wad.Append(new Marker("ENDMAP"));
            }

            wad.SaveTo(wadFilePath);

            Process.Start(
                enginePath,
                $"-file {wadFilePath} -skill 4 -iwad doom.wad -warp 1 1");
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
