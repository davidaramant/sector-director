// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;
using Is = NUnit.DeepObjectCompare.Is;

namespace SectorDirector.Core.Tests.FormatModels.Wad
{
    [TestFixture]
    public sealed class WadFileTests
    {
        [Test]
        public void ShouldCreateWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var wad = new WadWriter();
                wad.Append("MAP01", DemoMap.Create());
                wad.SaveTo(fileInfo.FullName);
            }
            finally
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }

        [Test]
        public void ShouldReadCreatedWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var map = DemoMap.Create();

                var wadWriter = new WadWriter();
                wadWriter.Append("MAP01", map);
                wadWriter.SaveTo(fileInfo.FullName);

                using (var wadReader = WadReader.Read(fileInfo.FullName))
                {
                    Assert.That(wadReader.Directory.Length, Is.EqualTo(3), "Did not return correct count.");
                    Assert.That(
                        wadReader.Directory.Select(l => l.Name).ToArray(),
                        Is.EquivalentTo(new[]
                            {new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP"),}),
                        "Did not return correct lump names.");
                    
                    var roundTripped = MapData.LoadFrom(wadReader.GetMapStream("MAP01"));

                    Assert.That(roundTripped, Is.DeepEqualTo(map));
                }
            }
            finally
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }
    }
}