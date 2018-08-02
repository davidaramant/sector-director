// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class SimpleMap
    {
        public EntityDatabase<VertexId, ScaledVertex> Vertices { get; } = new EntityDatabase<VertexId, ScaledVertex>();
        public EntityDatabase<SideDefId, SideDefEntity> SideDefs { get; } = new EntityDatabase<SideDefId, SideDefEntity>();
        public EntityDatabase<LineDefId, LineDefEntity> LineDefs { get; } = new EntityDatabase<LineDefId, LineDefEntity>();
        public EntityDatabase<SectorId, Sector> Sectors { get; } = new EntityDatabase<SectorId, Sector>();

    }
}