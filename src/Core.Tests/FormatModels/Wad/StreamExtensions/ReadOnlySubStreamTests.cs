// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Wad.StreamExtensions;

namespace SectorDirector.Core.Tests.FormatModels.Wad.StreamExtensions
{
    [TestFixture]
    public sealed class ReadOnlySubStreamTests
    {
        [Test]
        public void ShouldReadSubSetOfTextStream()
        {
            //          01234567890123456
            var text = "Here is some text";

            var textBytes = Encoding.ASCII.GetBytes(text);
            using (var baseStream = new MemoryStream(textBytes))
            using (var subStream = new ReadOnlySubStream(baseStream, 5, 7))
            using (var textReader = new StreamReader(subStream, Encoding.ASCII))
            {
                var actual = textReader.ReadLine();
                Assert.That(actual, Is.EqualTo("is some"));
            }
        }
    }
}
