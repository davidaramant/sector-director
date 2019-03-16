// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using System.IO;
using SectorDirector.Core.FormatModels.Wad;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public static class WadLoader
    {
        public static List<MapData> Load(string path)
        {
            var maps = new List<MapData>();

            var wad = WadFile.Read(path);


            foreach (var lump in wad.Where(l => l.Name.ToString() == "TEXTMAP"))
            {
                using (var ms = new MemoryStream(lump.GetData()))
                {
                    maps.Add(MapData.LoadFrom(ms));
                }
            }

            return maps;
        }
    }
}