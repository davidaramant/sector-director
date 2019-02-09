// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SectorDirector.Engine
{
    public sealed class ScreenBuffer
    {
        readonly Color[] _buffer;

        public Size Dimensions { get; }
        public int Width => Dimensions.Width;
        public int Height => Dimensions.Height;

        public Color this[Point p]
        {
            get => this[p.X, p.Y];
            set => this[p.X, p.Y] = value;
        }

        public Color this[int x, int y]
        {
            get => _buffer[y * Width + x];
            set => _buffer[y * Width + x] = value;
        }

        public void SafeSet(Point p, Color c) => SafeSet(p.X, p.Y, c);
        public void SafeSet(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                this[x, y] = c;
            }
        }

        public ScreenBuffer(Size size)
        {
            Dimensions = size;
            _buffer = new Color[Width * Height];
        }

        public void CopyToTexture(Texture2D texture) => texture.SetData(_buffer);
        public void Clear() => System.Array.Clear(_buffer, 0, _buffer.Length);
    }
}