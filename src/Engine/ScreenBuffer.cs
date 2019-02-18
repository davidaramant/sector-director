// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SectorDirector.Engine
{
    public sealed class ScreenBuffer
    {
        readonly Color[] _buffer;

        public Point Dimensions { get; }
        public int Width => Dimensions.X;
        public int Height => Dimensions.Y;

        public Color this[Point p]
        {
            get => this[p.X, p.Y];
            private set => this[p.X, p.Y] = value;
        }

        public Color this[int x, int y]
        {
            get => _buffer[y * Width + x];
            private set => _buffer[y * Width + x] = value;
        }

        public void DrawPixel(Point p, Color c) => DrawPixel(p.X, p.Y, c);
        public void DrawPixel(int x, int y, Color c)=>this[x, y] = c;

        public void DrawPixelSafe(Point p, Color c) => DrawPixelSafe(p.X, p.Y, c);
        public void DrawPixelSafe(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                this[x, y] = c;
            }
        }

        public void AddPixelSafe(Point p, Color c) => AddPixelSafe(p.X,p.Y,c);
        public void AddPixelSafe(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                var current = this[x,y];

                this[x, y] = new Color(
                                    MathHelper.Clamp(current.R + c.R,0,255),
                                    MathHelper.Clamp(current.G + c.G,0,255),
                                    MathHelper.Clamp(current.B + c.B,0,255));
            }
        }

        public ScreenBuffer(Point size)
        {
            Dimensions = size;
            _buffer = new Color[Width * Height];
        }

        public void CopyToTexture(Texture2D texture) => texture.SetData(_buffer);
        public void Clear() => System.Array.Clear(_buffer, 0, _buffer.Length);
    }
}