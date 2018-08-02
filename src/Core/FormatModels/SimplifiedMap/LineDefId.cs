// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Core.FormatModels.SimplifiedMap
{
    public struct LineDefId : IEquatable<LineDefId>
    {
        public static readonly LineDefId Invalid = new LineDefId(-1);
        public bool IsInvalid => _id < 0;

        private readonly int _id;

        public LineDefId(int id)
        {
            _id = id;
        }

        #region Equality

        public bool Equals(LineDefId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LineDefId && Equals((LineDefId) obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(LineDefId left, LineDefId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LineDefId left, LineDefId right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}