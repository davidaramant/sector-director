// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
 
namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class CompiledEntities<TId, T> where TId : struct where T : class
    {
        public IEnumerable<T> Entities { get; }
        public ReadOnlyDictionary<TId,int> IdTranslation { get; }

        public CompiledEntities(List<T> entities, Dictionary<TId, int> idLookup)
        {
            Entities = entities;
            IdTranslation = new ReadOnlyDictionary<TId, int>(idLookup);
        }
    }
}