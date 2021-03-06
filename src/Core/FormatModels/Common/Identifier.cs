﻿// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;

namespace SectorDirector.Core.FormatModels.Common
{
    [DebuggerDisplay("{" + nameof(_name) + "}")]
    public sealed class Identifier
    {
        private readonly string _name;

        public Identifier(string name) => _name = name;
        public string ToLower() => _name.ToLowerInvariant();
        public override string ToString() => _name;
        public static explicit operator string(Identifier id) => id._name;

        #region Equality members

        private bool Equals(Identifier other)
        {
            return string.Equals(_name, other._name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Identifier && Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(_name);
        }

        public static bool operator ==(Identifier left, Identifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}