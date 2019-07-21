// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using GeoAPI.Geometries;
using NetTopologySuite.Index.Strtree;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;
using SectorDirector.Engine;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks
{
    public class PositionToSectorIdBenchmarks
    {
        [GlobalSetup]
        public void LoadMap()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                _map = new MapGeometry(MapData.LoadFrom(wad.GetTextmapStream("MAP28")));
            }

            _sectorBounds = new STRtree<int>();
            for(int sectorId = 0; sectorId < _map.Sectors.Length; sectorId++)
            {
                _sectorBounds.Insert(_map.GetSectorMinimumBoundingRectangle(sectorId), sectorId);
            }
            _sectorBounds.Build();
        }

        [Benchmark]
        public List<int> NaiveSearch()
        {
            List<int> mapping = Enumerable.Repeat(-1,_map.Map.Things.Count).ToList();
            int thingId = 0;
            foreach (var thing in _map.Map.Things)
            {
                var thingPos = thing.GetPosition();
                for(int sectorId = 0; sectorId < _map.Sectors.Length; sectorId++)
                {
                    if(_map.IsInsideSector(sectorId, ref thingPos))
                    {
                        mapping[thingId] = sectorId;
                        break;
                    }
                }
                thingId++;
            }
            return mapping;
        }

        [Benchmark]
        public List<int> SearchWithRTree()
        {
            List<int> mapping = Enumerable.Repeat(-1, _map.Map.Things.Count).ToList();
            int thingId = 0;
            foreach (var thing in _map.Map.Things)
            {
                var thingPos = thing.GetPosition();
                var thingEnv = new Envelope(new Coordinate(thing.X,thing.Y));
                
                foreach(var sectorId in _sectorBounds.Query(thingEnv))
                {
                    if(_map.IsInsideSector(sectorId, ref thingPos))
                    {
                        mapping[thingId] = sectorId;
                        break;
                    }
                }
                
                thingId++;
            }
            return mapping;
        }

        MapGeometry _map;
        STRtree<int> _sectorBounds;
    }
}