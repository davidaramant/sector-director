// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using SectorDirector.Core.CollectionExtensions;
using SectorDirector.Core.FormatModels.Svg;
using SectorDirector.Core.FormatModels.Wad;
using System;
using System.IO;

namespace SectorDirector.Core.Tests.FormatModels.Svg
{
    [TestFixture, Parallelizable]
    public class SvgExporterTest
    {
        [Test]
        [Explicit]
        public void ExportDemoMap()
        {
            //var inputWad = @"C:\Games\Doom\IWADS\doom.wad";
            var inputWad = @"C:\Games\Doom\levels\10sector.wad";
            //var inputWad = @"C:\Users\aramant\Desktop\Doom\freedoom1-udmf.wad";
            //var inputWad = @"C:\Users\aramant\Desktop\Doom\10sector-udmf.wad";

            var baseOutputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Doom", "SVGs");
            if (!Directory.Exists(baseOutputPath))
            {
                Directory.CreateDirectory(baseOutputPath);
            }

            var wadName = Path.GetFileNameWithoutExtension(inputWad);

            foreach (var (name,map) in WadLoader.Load(inputWad))
            {
                try
                {
                    SvgExporter.Export(map,
                        Path.Combine(baseOutputPath, $"{wadName}.{name}.svg"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(name + ": " + e);
                }
            }
        }
    }
}
