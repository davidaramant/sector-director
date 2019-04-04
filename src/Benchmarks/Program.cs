// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MapLoadingBenchmarks>();

            GC.KeepAlive(MapLoadingBenchmarks.LoadAllFreedoomMaps());
            var map = MapLoadingBenchmarks.LoadZDCMP2();
            Console.WriteLine($"Vertices: {map.Vertices.Count:N0}");
            Console.WriteLine($"LineDefs: {map.LineDefs.Count:N0}");
            Console.WriteLine($"SideDefs: {map.SideDefs.Count:N0}");
            Console.WriteLine($"Sectors: {map.Sectors.Count:N0}");
            Console.WriteLine($"Things: {map.Things.Count:N0}");
            Console.WriteLine($"Unknown Blocks: {map.UnknownBlocks.Count:N0}");
            Console.WriteLine($"Unknown Properties: {map.UnknownProperties.Count:N0}");
        }
    }
}
