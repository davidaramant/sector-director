// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class SplitScreenRenderer : IRenderer
    {
        private readonly IRenderer _leftRenderer;
        private readonly IRenderer _rightRenderer;

        public SplitScreenRenderer(IRenderer leftRenderer, IRenderer rightRenderer)
        {
            _leftRenderer = leftRenderer;
            _rightRenderer = rightRenderer;
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {
            _leftRenderer.Update(inputs, gameTime);
            _rightRenderer.Update(inputs, gameTime);
        }

        sealed class LeftScreen : IScreenBuffer
        {
            private readonly IScreenBuffer _buffer;

            public LeftScreen(IScreenBuffer buffer) => _buffer = buffer;

            public Color this[Point p] => _buffer[p];
            public Color this[int x, int y] => _buffer[x, y];

            public int Height => _buffer.Height;
            public int Width => _buffer.Width / 2;
            public Point Dimensions => new Point(Width, Height);

            public void AddPixel(int x, int y, Color c) => _buffer.AddPixel(x, y, c);

            public void Clear()
            {
                _buffer.Clear(new Rectangle(0, 0, Width, Height));
            }

            public void Clear(Rectangle area)
            {
                throw new NotImplementedException();
            }

            public void DrawPixel(int x, int y, Color c) => _buffer.DrawPixel(x, y, c);
        }

        sealed class RightScreen : IScreenBuffer
        {
            private readonly IScreenBuffer _buffer;

            public RightScreen(IScreenBuffer buffer) => _buffer = buffer;

            public Color this[Point p] => _buffer[p + new Point(Width, 0)];
            public Color this[int x, int y] => _buffer[x + Width, y];

            public Point Dimensions => new Point(Width, Height);
            public int Height => _buffer.Height;
            public int Width => _buffer.Width / 2;
            public void AddPixel(int x, int y, Color c) => _buffer.AddPixel(x + Width, y, c);

            public void Clear()
            {
                _buffer.Clear(new Rectangle(Width, 0, Width, Height));
            }

            public void Clear(Rectangle area)
            {
                throw new NotImplementedException();
            }

            public void DrawPixel(int x, int y, Color c) => _buffer.DrawPixel(x + Width, y, c);
        }

        public void Render(IScreenBuffer screen, PlayerInfo player)
        {
            _leftRenderer.Render(new LeftScreen(screen), player);
            _rightRenderer.Render(new RightScreen(screen), player);
        }
    }
}