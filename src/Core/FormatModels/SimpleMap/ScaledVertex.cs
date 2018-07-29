// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class ScaledVertex : IEquatable<ScaledVertex>
    {
        public const int Scale = 100;

        public int ScaledX { get; set; }
        public int ScaledY { get; set; } 

        public double X
        {
            get => (double)ScaledX / Scale;
            set => ScaledX = (int)(value * Scale);
        }

        public double Y
        {
            get => (double)ScaledY / Scale;
            set => ScaledY = (int)(value * Scale);
        }

        public ScaledVertex(int scaledX, int scaledY)
        {
            ScaledX = scaledX;
            ScaledY = scaledY;
        }

        public ScaledVertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vertex ToVertex() => new Vertex(X, Y);

        #region Equality

        public bool Equals(ScaledVertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ScaledX == other.ScaledX && ScaledY == other.ScaledY;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ScaledVertex && Equals((ScaledVertex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ScaledX * 397) ^ ScaledY;
            }
        }

        public static bool operator ==(ScaledVertex left, ScaledVertex right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScaledVertex left, ScaledVertex right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}