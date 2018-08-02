// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class SimpleMap
    {
        public VertexDatabase Vertices { get; } = new VertexDatabase();
        public SectorDatabase Sectors { get; } = new SectorDatabase();
        public SideDefDatabase SideDefs { get; } = new SideDefDatabase();
        public LineDefDatabase LineDefs { get; } = new LineDefDatabase();
        public List<Thing> Things { get; } = new List<Thing>();

        public MapData Compile()
        {
            var map = new MapData {NameSpace = "Doom"};

            var compiledVertices = Vertices.CompileEntities();
            map.Vertices.AddRange(compiledVertices.Entities);
            
            var compiledSectors = Sectors.Compile();
            map.Sectors.AddRange(compiledSectors.Entities);

            var compiledSideDefs = SideDefs.Compile(compiledSectors.IdTranslation);
            map.SideDefs.AddRange(compiledSideDefs.Entities);

            var compiledLineDefs = LineDefs.Compile(compiledVertices.IdTranslation, compiledSideDefs.IdTranslation);
            map.LineDefs.AddRange(compiledLineDefs.Entities);

            map.Things.AddRange(Things);

            return map;
        }
    }
}