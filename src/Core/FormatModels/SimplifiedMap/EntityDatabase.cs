// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public abstract class EntityDatabase<TId, TEntities> 
        where TId : struct where TEntities : class
    {
        protected readonly IdSequence<TId> IdSequence = new IdSequence<TId>();
        protected readonly Dictionary<TId, TEntities> EntityMap = new Dictionary<TId, TEntities>();

        public TId Add(TEntities vertex)
        {
            var id = IdSequence.GetNext();
            EntityMap.Add(id, vertex);
            return id;
        }

        public void Remove(TId id)
        {
            EntityMap.Remove(id);
        }

        protected CompiledEntities<TId,TResult> CompileEntities<TResult>(
            Func<TEntities,TResult> transform)
        {
            var entities = new List<TResult>();
            var idLookup = new Dictionary<TId, int>();

            foreach (var pair in EntityMap)
            {
                idLookup[pair.Key] = entities.Count;
                entities.Add(transform(pair.Value));
            }

            return new CompiledEntities<TId, TResult>(entities, idLookup);
        }
    }
}