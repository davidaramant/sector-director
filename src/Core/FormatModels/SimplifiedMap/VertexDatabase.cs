// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Functional.Maybe;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public sealed class VertexDatabase
    {
        private readonly IdSequence<VertexId> _idSequence = new IdSequence<VertexId>();
        private readonly Dictionary<ScaledVertex, VertexId> _vertexToId = new Dictionary<ScaledVertex, VertexId>();
        private readonly Dictionary<VertexId, ScaledVertex> _idToVertex = new Dictionary<VertexId, ScaledVertex>();

        public VertexId Add(ScaledVertex vertex)
        {
            return _vertexToId.Lookup(vertex).OrElse(() =>
            {
                var id = _idSequence.GetNext();
                _vertexToId.Add(vertex, id);
                _idToVertex.Add(id, vertex);
                return id;
            });
        }

        public void Remove(VertexId id)
        {
            if (_idToVertex.ContainsKey(id))
            {
                var vertex = _idToVertex[id];
                _idToVertex.Remove(id);
                _vertexToId.Remove(vertex);
            }
        }

        public void Remove(ScaledVertex vertex)
        {
            if (_vertexToId.ContainsKey(vertex))
            {
                var id = _vertexToId[vertex];
                _vertexToId.Remove(vertex);
                _idToVertex.Remove(id);
            }
        }

        public CompiledEntities<VertexId,Vertex> CompileEntities()
        {
            var entities = new List<Vertex>();
            var idLookup = new Dictionary<VertexId, int>();

            foreach (var pair in _idToVertex)
            {
                idLookup[pair.Key] = entities.Count;
                entities.Add(pair.Value.ToVertex());
            }

            return new CompiledEntities<VertexId, Vertex>(entities, idLookup);
        }
    }
}