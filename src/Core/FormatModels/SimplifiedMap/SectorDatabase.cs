// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class SectorDatabase : EntityDatabase<SectorId,Sector>
    {
        public CompiledEntities<SectorId, Sector> Compile() => CompileEntities(sector => sector);
    }
}