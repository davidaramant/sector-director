// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.Engine
{
    public struct Size
    {
        public readonly int Width;
        public readonly int Height;

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Size Quarter() => new Size(Width / 2, Height / 2);

        #region Equality

        public override bool Equals(object obj)
        {
            if (!(obj is Size))
            {
                return false;
            }

            var size = (Size)obj;
            return Width == size.Width &&
                   Height == size.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Equals(size2);
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        #endregion
    }
}