using NUnit.Framework;
using SectorDirector.Core.FormatModels.Svg;
using SectorDirector.Core.FormatModels.Wad;
using System;
using System.IO;
using System.Linq;
using SectorDirector.Core.CollectionExtensions;

namespace SectorDirector.Core.Tests.FormatModels.Svg
{
    [TestFixture]
    public class SvgExporterTest
    {
        [Test]
        [Explicit]
        public void ExportDemoMap()
        {
            //var maps = WadLoader.Load(@"C:\Games\Doom\iwads\doom.wad");

//            var inputWad = @"C:\Users\aramant\Desktop\Doom\freedoom1-udmf.wad";
            var inputWad = @"C:\Users\aramant\Desktop\Doom\10sector-udmf.wad";

            var maps = WadLoader.Load(inputWad);

            var baseOutputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Doom", "SVGs");
            if (!Directory.Exists(baseOutputPath))
            {
                Directory.CreateDirectory(baseOutputPath);
            }

            var wadName = Path.GetFileNameWithoutExtension(inputWad);

            foreach (var map in maps.Indexed())
            {
                SvgExporter.Export(map.Value,
                    Path.Combine(baseOutputPath, $"{wadName}.{map.Index}.svg"));
            }
        }
    }
}
