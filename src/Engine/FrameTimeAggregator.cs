﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using System.Diagnostics;
using System.Linq;

namespace SectorDirector.Engine
{
    public sealed class FrameTimeAggregator
    {
        const int MaxSampleCount = 60;
        private readonly double[] _frameTimeSamples = new double[MaxSampleCount];
        private int _count = 0;
        private readonly Stopwatch _timer = new Stopwatch();
        private double _average;

        public void StartTiming() => _timer.Restart();
        public void StopTiming()
        {
            _timer.Stop();
            _frameTimeSamples[_count++] = _timer.Elapsed.TotalMilliseconds;
            if(_count == MaxSampleCount)
            {
                _average = _frameTimeSamples.Average();
                _count = 0;
            }
        }

        public void Reset() => _count = 0;

        public double GetAverageFrameTimeInMs() => _average;
    }
}