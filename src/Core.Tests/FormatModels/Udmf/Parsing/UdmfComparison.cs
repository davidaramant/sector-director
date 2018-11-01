// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    public static partial class UdmfComparison
    {
        public static void AssertEqual(UnknownProperty actual, UnknownProperty expected)
        {
            Func<UnknownProperty, string> toString =
                up => $"{(string)up.Name} = {up.Value}";
            Assert.That(
                toString(actual),
                Is.EqualTo(toString(expected)),
                "Different unknown properties");
        }

        public static void AssertEqual(IEnumerable<UnknownProperty> actual, IEnumerable<UnknownProperty> expected)
        {
            Func<IEnumerable<UnknownProperty>, string> toString =
                list => string.Join("\n", list.Select(up => $"{(string)up.Name} = {up.Value}"));
            Assert.That(
                toString(actual),
                Is.EqualTo(toString(expected)),
                "Different unknown properties");
        }

        public static void AssertEqual(UnknownBlock actual, UnknownBlock expected)
        {
            Assert.That(
                actual.Name,
                Is.EqualTo(expected.Name),
                "Different name on unknown block.");

            AssertEqual(actual.Properties, expected.Properties);
        }
    }
}