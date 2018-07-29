// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class EntityDatabase<TId, T> where TId : struct where T : class
    {
        private readonly IdSequence<TId> _idSequence = new IdSequence<TId>();
        private readonly Dictionary<TId, T> _entityMap = new Dictionary<TId, T>();

        public TId Add(T vertex)
        {
            var id = _idSequence.GetNext();
            _entityMap.Add(id, vertex);
            return id;
        }

        public void Remove(TId id)
        {
            _entityMap.Remove(id);
        }

        public (List<T> entities, Dictionary<TId, int> idLookup) GetVertexListAndIdLookup()
        {
            var entities = new List<T>();
            var idLookup = new Dictionary<TId, int>();

            foreach (var pair in _entityMap)
            {
                idLookup[pair.Key] = entities.Count;
                entities.Add(pair.Value);
            }

            return (entities, idLookup);
        }
    }
}