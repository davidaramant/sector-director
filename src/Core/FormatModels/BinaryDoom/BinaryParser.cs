using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SectorDirector.Core.FormatModels.BinaryDoom
{
    public static class BinaryParser
    {

        private static readonly string[] lumpTypes = {
            "THINGS",
            "VERTEXES",
            "LINEDEFS",
            "SIDEDEFS",
            "SECTORS",
        };

        public static List<MapData> LoadBinary(string filePath)
        {
            var maps = new List<MapData>();
            MapData currentMap = null;
            using (var reader = WadReader.Read(filePath))
            {
                foreach(var lump in reader.Directory)
                {
                    if (lump.Size > 0 && lumpTypes.Contains(lump.Name.ToString()))
                    {
                        var lumpStream = reader.GetLumpStream(lump);
                        UpdateMap(currentMap, lump, lumpStream);
                    } else if(Regex.IsMatch(lump.Name.ToString(), @"^E\d+M\d+$"))
                    {
                        if (null != currentMap)
                        {
                            maps.Append(currentMap);
                        }
                        currentMap = new MapData
                        {
                            Comment = lump.Name.ToString()
                        };
                    }
                }
            }
            maps.Append(currentMap);
            return maps;
        }

        private static void UpdateMap(MapData map, LumpInfo lump, Stream lumpStream)
        {
            switch(lump.Name.ToString())
            {
                case "THINGS":
                    UpdateMapContents<Thing>(map.Things, lumpStream, lump.Size);
                    break;
                case "LINEDEFS":
                    UpdateMapContents<LineDef>(map.LineDefs, lumpStream, lump.Size);
                    break;
                case "SIDEDEFS":
                    UpdateMapContents<SideDef>(map.SideDefs, lumpStream, lump.Size);
                    break;
                case "VERTEXES":
                    UpdateMapContents<Vertex>(map.Vertices, lumpStream, lump.Size);
                    break;
                case "SECTORS":
                    UpdateMapContents<Sector>(map.Sectors, lumpStream, lump.Size);
                    break;
                default:
                    break;
            }
        }

        private static void UpdateMapContents<T>(List<T> mapList, Stream lumpStream, int lumpSize)
        {
            using (var reader = new StreamReader(lumpStream))
            {
                var buffer = new char[lumpSize];
                reader.ReadBlock(buffer, 0, lumpSize);
                Console.WriteLine(buffer);
            }
        }
    }
}
