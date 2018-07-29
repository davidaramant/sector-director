// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class SectorId : IEquatable<SectorId>
    {
        private readonly int _id;

        public SectorId(int id)
        {
            _id = id;
        }
        #region Equality

        public bool Equals(SectorId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is SectorId && Equals((SectorId) obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(SectorId left, SectorId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SectorId left, SectorId right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}