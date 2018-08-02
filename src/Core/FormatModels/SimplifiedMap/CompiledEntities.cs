// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
 
namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class CompiledEntities<TId, T> where TId : struct
    {
        public IEnumerable<T> Entities { get; }
        public ImmutableDictionary<TId,int> IdTranslation { get; }

        public CompiledEntities(List<T> entities, Dictionary<TId, int> idLookup)
        {
            Entities = entities;
            IdTranslation = idLookup.ToImmutableDictionary();
        }
    }
}