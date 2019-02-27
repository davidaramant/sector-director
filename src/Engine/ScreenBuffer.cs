// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using System.Linq;
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
        public void DrawPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                this[x, y] = c;
            }
        }

        public void AddPixel(Point p, Color c) => AddPixel(p.X, p.Y, c);
        public void AddPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                var current = this[x, y];

                this[x, y] = new Color(
                    Math.Min(current.R + c.R, 255),
                    Math.Min(current.G + c.G, 255),
                    Math.Min(current.B + c.B, 255));
            }
        }


        public ScreenBuffer(Point size)
        {
            Dimensions = size;
            _buffer = new Color[Width * Height];
        }

        private ScreenBuffer(Point size, Color[] buffer)
        {
            Dimensions = size;
            _buffer = buffer;
        }

        public void CopyToTexture(Texture2D texture) => texture.SetData(_buffer);
        public void Clear() => Array.Clear(_buffer, 0, _buffer.Length);
        public ScreenBuffer Clone() => new ScreenBuffer(Dimensions, _buffer.ToArray());
        public void CopyFrom(Color[] texture, Point textureSize, Point destination)
        {
            var xMargin = Width - destination.X;
            var xToCopy = MathHelper.Min(xMargin, textureSize.X);

            var yMargin = Height - destination.Y;
            var yToCopy = MathHelper.Min(yMargin, textureSize.Y);

            for (int y = 0; y < yToCopy; y++)
            {
                Array.Copy(
                    sourceArray: texture, sourceIndex: y * textureSize.X,
                    destinationArray: _buffer, destinationIndex: (destination.Y + y) * Width + destination.X,
                    length: xToCopy);
            }
        }
    }
}