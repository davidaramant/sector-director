// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class SideDefEntity
    {
        public SectorId Sector { get; set; } = SectorId.Invalid;

        public SideDef Data { get; }

        public SideDefEntity(SideDef data)
        {
            Data = data;
        }
    }
}