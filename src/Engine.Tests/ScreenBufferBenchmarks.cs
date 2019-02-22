// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Tests
{
    [TestFixture]
    public sealed class ScreenBufferBenchmarks
    {
        public class ElementByElementVsArrayCopy
        {
            readonly ScreenBuffer _buffer = new ScreenBuffer(new Point(1000, 1000));
            readonly Point _destination = new Point(10, 10);
            static readonly Point _textureSize = new Point(200, 200);
            readonly Color[] _textureData = new Color[_textureSize.Area()];

            [Benchmark(Baseline = true)]
            public void CopyElementByElement()
            {
                for (int y = 0; y < _textureSize.Y; y++)
                {
                    for (int x = 0; x < _textureSize.X; x++)
                    {
                        _buffer.DrawPixel(_destination.X + x, _destination.Y + y, _textureData[x + y * _textureSize.X]);
                    }
                }
            }

            [Benchmark]
            public void CopyWithArrayCopy()
            {
                _buffer.CopyFrom(_textureData, _textureSize, _destination);
            }
        }

        [Test, Explicit]
        public void BenchmarkCopyingTexture()
        {
            var summary = BenchmarkRunner.Run<ElementByElementVsArrayCopy>();
        }
    }
}