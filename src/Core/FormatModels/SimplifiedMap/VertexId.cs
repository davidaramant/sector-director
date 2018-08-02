// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public struct VertexId : IEquatable<VertexId>
    {    
        public static readonly VertexId Invalid = new VertexId(-1);
        public bool IsInvalid => _id < 0;

        private readonly int _id;

        public VertexId(int id)
        {
            _id = id;
        }

        #region Equality
        public bool Equals(VertexId other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexId && Equals((VertexId) obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(VertexId left, VertexId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexId left, VertexId right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}