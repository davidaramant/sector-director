// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;

namespace Benchmarks
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 3)]
    public class MapLoadingBenchmarks
    {
        [Benchmark]
        public MapData LoadLargeMap()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFrom(wad.GetMapStream("MAP28"));
            }
        }
    }
}
