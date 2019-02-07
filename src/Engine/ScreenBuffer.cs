// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SectorDirector.Engine
{
    public sealed class ScreenBuffer
    {
        readonly Color[] _buffer;

        public int Width { get; }
        public int Height { get; }

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

        public ScreenBuffer(Size size) : this(size.Width, size.Height)
        {
        }

        public ScreenBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            _buffer = new Color[width * height];
        }

        public void CopyToTexture(Texture2D texture) => texture.SetData(_buffer);
        public void Clear() => System.Array.Clear(_buffer, 0, _buffer.Length);
    }
}