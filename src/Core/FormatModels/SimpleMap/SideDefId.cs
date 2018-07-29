// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public struct SideDefId : IEquatable<SideDefId>
    {
        public static readonly SideDefId Invalid = new SideDefId(-1);
        public bool IsInvalid => _id < 0;

        private readonly int _id;

        public SideDefId(int id)
        {
            _id = id;
        }

        #region Equality
        public bool Equals(SideDefId other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SideDefId && Equals((SideDefId) obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(SideDefId left, SideDefId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SideDefId left, SideDefId right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}