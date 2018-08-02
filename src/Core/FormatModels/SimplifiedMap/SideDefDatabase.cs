// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class SideDefDatabase : EntityDatabase<SideDefId,SideDefEntity>
    {
        private readonly IdSequence<SideDefId> _idSequence = new IdSequence<SideDefId>();
        private readonly Dictionary<SideDefId, SideDefEntity> _entityMap = new Dictionary<SideDefId, SideDefEntity>();

        public SideDefId Add(SideDefEntity vertex)
        {
            var id = _idSequence.GetNext();
            _entityMap.Add(id, vertex);
            return id;
        }

        public CompiledEntities<SideDefId, SideDef> Compile(ImmutableDictionary<SectorId, int> sectorLookup)
        {
            var entities = new List<SideDef>();
            var idLookup = new Dictionary<SideDefId, int>
            {
                { SideDefId.Invalid, -1 }
            };

            foreach (var pair in _entityMap)
            {
                idLookup[pair.Key] = entities.Count;

                var sideDef = pair.Value.Data.Clone();
                sideDef.Sector = sectorLookup[pair.Value.Sector];

                entities.Add(sideDef);
            }

            return new CompiledEntities<SideDefId, SideDef>(entities, idLookup);
        }
    }
}