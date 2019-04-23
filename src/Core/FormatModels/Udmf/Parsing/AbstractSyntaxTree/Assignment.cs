﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.WritingExtensions;
using System;
using System.Diagnostics;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.AbstractSyntaxTree
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Assignment : IGlobalExpression, IEquatable<Assignment>
    {
        public readonly Identifier Name;
        public readonly Token Value;

        public Assignment(Identifier name, Token value)
        {
            Name = name;
            Value = value;
        }

        public string ValueAsString()
        {
            switch (Value)
            {
                case IntegerToken i:
                    return i.Value.ToString();
                
                case FloatToken f:
                    return f.Value.ToStringWithDecimal();
                
                case BooleanToken b:
                    return b.Value.ToString().ToLowerInvariant();

                case StringToken s:
                    return '"' + s.Value + '"';

                default:
                    return Value.ToString();
            }
        }

        public override string ToString() => $"{Name}: {ValueAsString()} ({Value.GetType()})";

        #region Equality
        public bool Equals(Assignment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Assignment other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(Assignment left, Assignment right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Assignment left, Assignment right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}