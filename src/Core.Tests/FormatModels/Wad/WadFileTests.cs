// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf.Parsing;
using SectorDirector.Core.FormatModels.Wad;
using SectorDirector.Core.Tests.FormatModels.Udmf.Parsing;
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
                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UdmfLump("TEXTMAP", DemoMap.Create()));
                wad.Append(new Marker("ENDMAP"));
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

                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UdmfLump("TEXTMAP", map));
                wad.Append(new Marker("ENDMAP"));
                wad.SaveTo(fileInfo.FullName);

                wad = WadFile.Read(fileInfo.FullName);
                Assert.That(wad.Count, Is.EqualTo(3), "Did not return correct count.");
                Assert.That(
                    wad.Select(l => l.Name).ToArray(),
                    Is.EquivalentTo(new[] { new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP"), }),
                    "Did not return correct lump names.");

                var mapBytes = wad[1].GetData();
                using (var ms = new MemoryStream(mapBytes))
                using (var textReader = new StreamReader(ms, Encoding.ASCII))
                {
                    var sa = new UdmfSyntaxAnalyzer();
                    throw new NotImplementedException("Switch over to new parser");
                    //var roundTripped = UdmfParser.Parse(sa.Analyze(new UdmfLexer(textReader)));

                    //Assert.That(roundTripped, Is.DeepEqualTo(map));
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