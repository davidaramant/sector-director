// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.Core.FormatModels.BinaryDoom;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.Wad
{
    public static class WadLoader
    {
        public static List<MapData> Load(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                foreach (var mapName in wad.GetMapNames())
                {
                    if (wad.IsMapUDMF(mapName))
                    {
                        maps.Add(MapData.LoadFrom(wad.GetTextmapStream(mapName)));
                    }
                    else
                    {
                        maps.Add(new MapData(nameSpace:"Doom",
                            things:LumpParsers.Thing(wad.GetNextLumpStreamOfName(mapName, "THINGS")),
                            lineDefs:LumpParsers.LineDef(wad.GetNextLumpStreamOfName(mapName, "LINEDEFS")),
                            sideDefs:LumpParsers.SideDef(wad.GetNextLumpStreamOfName(mapName, "SIDEDEFS")),
                            vertices:LumpParsers.Vertex(wad.GetNextLumpStreamOfName(mapName, "VERTEXES")),
                            sectors:LumpParsers.Sector(wad.GetNextLumpStreamOfName(mapName, "SECTORS"))));
                    }
                }
            }

            return maps;
        }
    }
}