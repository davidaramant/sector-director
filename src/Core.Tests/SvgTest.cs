using NUnit.Framework;
using SectorDirector.Core.FormatModels.Svg;
using System;
using System.IO;
using System.Linq;
using SectorDirector.Core.FormatModels.Wad;

namespace SectorDirector.Core.Tests
{
    [TestFixture]
    public class SvgTest
    {
        [Test]
        public void ExportDemoMap()
        {
            var maps = WadLoader.Load(@"C:\Games\Doom\iwads\doom.wad");

            var map = maps.First();

            SvgExporter.Export(map, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "demo.svg"));
        }
    }
}
