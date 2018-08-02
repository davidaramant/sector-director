// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class LineDefDatabase : EntityDatabase<LineDefId,LineDefEntity>
    {
        public CompiledEntities<LineDefId, LineDef> Compile(
            ImmutableDictionary<VertexId, int> vertexLookup,
            ImmutableDictionary<SideDefId, int> sideDefLookup)
        {
            return CompileEntities(lineDefEntity =>
            {
                var lineDef = lineDefEntity.Data.Clone();
                lineDef.V1 = vertexLookup[lineDefEntity.Vertex1];
                lineDef.V2 = vertexLookup[lineDefEntity.Vertex1];
                lineDef.SideFront = sideDefLookup[lineDefEntity.FrontSide];
                lineDef.SideBack = sideDefLookup[lineDefEntity.BackSide];

                return lineDef;
            });
        }
    }
}