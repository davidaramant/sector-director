// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using SectorDirector.Core.FormatModels.Wad;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Tests
{
    [TestFixture, Parallelizable]
    public sealed class MapGeometryTests
    {
        ImmutableList<MapGeometry> ConcaveMaps;

        [OneTimeSetUp]
        public void LoadTestMaps()
        {
            // Test Maps:
            // MAP01:
            // - Sector 0 - a 256x256 box with the bottom left at (0,0)
            // - Sector 1 - a jagged concave sector in the middle
            ConcaveMaps =
                WadLoader.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "ConcaveSectors.wad")).
                Select(m => new MapGeometry(m)).
                ToImmutableList();
        }

        [TestCase(32, 32, 0)]
        [TestCase(140, 155, 1)]
        [TestCase(512, 155, 2, Description = "Outside of either sector")]
        public void ShouldDetermineIfInsideSector(float x, float y, int indexOfContainingSector)
        {
            var point = new Vector2(x, y);
            for (int sectorId = 0; sectorId < 2; sectorId++)
            {
                Assert.That(ConcaveMaps[0].IsInsideSector(sectorId, ref point),
                    Is.EqualTo(indexOfContainingSector == sectorId),
                    $"Did not properly determine containment for sector index {sectorId}");
            }
        }
    }
}