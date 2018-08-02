// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class EntityDatabase<TId, TEntities> where TId : struct where TEntities : class
    {
        private readonly IdSequence<TId> _idSequence = new IdSequence<TId>();
        private readonly Dictionary<TId, TEntities> _entityMap = new Dictionary<TId, TEntities>();

        public TId Add(TEntities vertex)
        {
            var id = _idSequence.GetNext();
            _entityMap.Add(id, vertex);
            return id;
        }

        public void Remove(TId id)
        {
            _entityMap.Remove(id);
        }

        public CompiledEntities<TId,TEntities> CompileEntities()
        {
            var entities = new List<TEntities>();
            var idLookup = new Dictionary<TId, int>();

            foreach (var pair in _entityMap)
            {
                idLookup[pair.Key] = entities.Count;
                entities.Add(pair.Value);
            }

            return new CompiledEntities<TId, TEntities>(entities, idLookup);
        }
    }
}