// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class LineDefEntity
    {
        public VertexId Vertex1 { get; set; } = VertexId.Invalid;
        public VertexId Vertex2 { get; set; } = VertexId.Invalid;

        public SideDefId FrontSide { get; set; } = SideDefId.Invalid;
        public SideDefId BackSide { get; set; } = SideDefId.Invalid;

        public LineDef Data { get; }

        public LineDefEntity(LineDef data)
        {
            Data = data;
        }
    }
}