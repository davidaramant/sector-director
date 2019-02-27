// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using Microsoft.Xna.Framework;
using SectorDirector.Engine;

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
}