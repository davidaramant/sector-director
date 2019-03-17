// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Xna.Framework;
using SectorDirector.Engine;
using SectorDirector.Engine.Drawing;

namespace Benchmarks
{
    public class ScreenBufferElementByElementVsArrayCopy
    {
        readonly ScreenBuffer _buffer = new ScreenBuffer(new Point(1000, 1000));
        readonly Point _destination = new Point(10, 10);
        static readonly Point TextureSize = new Point(200, 200);
        readonly Color[] _textureData = new Color[TextureSize.Area()];

        [Benchmark(Baseline = true)]
        public void CopyElementByElement()
        {
            for (int y = 0; y < TextureSize.Y; y++)
            {
                for (int x = 0; x < TextureSize.X; x++)
                {
                    _buffer.DrawPixel(_destination.X + x, _destination.Y + y, _textureData[x + y * TextureSize.X]);
                }
            }
        }

        [Benchmark]
        public void CopyWithArrayCopy()
        {
            _buffer.CopyFrom(_textureData, TextureSize, _destination);
        }
    }

    public class ScreenBufferLineRendering
    {
        readonly ScreenBuffer _buffer = new ScreenBuffer(new Point(2000, 2000));
        private const int NumberOfPoints = 2000;
        private readonly Point[] _points = new Point[NumberOfPoints];

        [GlobalSetup]
        public void PickLineEndPoints()
        {
            var random = new Random();

            foreach (var pointIndex in Enumerable.Range(0, NumberOfPoints))
            {
                _points[pointIndex] = new Point(random.Next(_buffer.Width), random.Next(_buffer.Height));
            }
        }

        [IterationSetup]
        public void ClearBuffer() => _buffer.Clear();

        [Benchmark(Baseline = true)]
        public void BresenhamLineDrawer()
        {
            for (int i = 0; i < NumberOfPoints - 1; i++)
            {
                _buffer.PlotLine(_points[i], _points[i + 1], Color.Red);
            }
        }

        [Benchmark]
        public void WuLineDrawer()
        {
            for (int i = 0; i < NumberOfPoints - 1; i++)
            {
                _buffer.PlotLineSmooth(_points[i], _points[i + 1], Color.Red);
            }
        }
    }

    [InProcess]
    public class ScreenBufferClipping
    {
        private const int Margin = 500;
        readonly ScreenBuffer _buffer = new ScreenBuffer(new Point(2000, 2000));
        private const int NumberOfPoints = 2000;
        private readonly Point[] _points = new Point[NumberOfPoints];

        [GlobalSetup]
        public void PickLineEndPoints()
        {
            var random = new Random();

            foreach (var pointIndex in Enumerable.Range(0, NumberOfPoints))
            {
                _points[pointIndex] = new Point(
                    random.Next(-Margin, _buffer.Width + Margin),
                    random.Next(-Margin, _buffer.Height + Margin));
            }
        }

        [IterationSetup]
        public void ClearBuffer() => _buffer.Clear();

        [Benchmark(Baseline = true)]
        public void SimpleClipping()
        {
            for (int i = 0; i < NumberOfPoints - 1; i++)
            {
                var sc1 = _points[i];
                var sc2 = _points[i + 1];

                if (LineClipping.CouldAppearOnScreen(_buffer, sc1, sc2))
                {
                    _buffer.PlotLineSmooth(sc1, sc2, Color.Red);
                }
            }
        }

        [Benchmark]
        public void CohenSutherlandClipping()
        {
            for (int i = 0; i < NumberOfPoints - 1; i++)
            {
                var sc1 = _points[i];
                var sc2 = _points[i + 1];

                var result = LineClipping.ClipToScreen(_buffer, sc1, sc2);
                if (result.shouldDraw)
                {
                    _buffer.PlotLineSmooth(result.p0, result.p1, Color.Red);
                }
            }
        }

    }
}