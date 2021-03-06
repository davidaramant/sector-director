// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace SectorDirector.Engine.Tests
{
    /// <summary>
    /// Tests to verify various math things
    /// </summary>
    [TestFixture, Parallelizable]
    public sealed class MathPlayground
    {
        [TestCase(-10, 10, 10, 10, 0, 0, false)]
        [TestCase(-10, 10, 10, 10, 0, 15, true)]

        [TestCase(10, -10, -10, -10, 0, 0, false)]
        [TestCase(10, -10, -10, -10, 0, -15, true)]

        [TestCase(10, 10, 10, -10, 0, 0, false)]
        [TestCase(10, 10, 10, -10, 15, 0, true)]

        [TestCase(-10, -10, -10, 10, 0, 0, false)]
        [TestCase(-10, -10, -10, 10, -15, 0, true)]
        public void ShouldDetermineThatPointHasNotCrossedLine(
            float v1x, float v1y,
            float v2x, float v2y,
            float px, float py,
            bool hasCrossed)
        {
            var v1 = new Vector2(v1x, v1y);
            var v2 = new Vector2(v2x, v2y);

            var pos = new Vector2(px, py);

            Assert.That(Line.HasCrossed(ref v1, ref v2, ref pos), Is.EqualTo(hasCrossed));
        }

        [Test]
        public void ShouldComputeIntersection()
        {
            var v1 = new Vector2(-10, 0);
            var v2 = new Vector2(10, 0);

            var p1 = new Vector2(5, -10);
            var p2 = new Vector2(5, 10);

            var intersection = Line.Intersection(ref v1, ref v2, ref p1, ref p2);

            Assert.That(intersection, Is.EqualTo(new Vector2(5, 0)));
        }

        [Test]
        public void WhatIsTheIntersectionIfItDoesNotIntersect()
        {
            var v1 = new Vector2(-10, 0);
            var v2 = new Vector2(10, 0);

            var p1 = new Vector2(15, -10);
            var p2 = new Vector2(15, 10);

            var intersection = Line.Intersection(ref v1, ref v2, ref p1, ref p2);

            Assert.That(intersection, Is.EqualTo(new Vector2(15, 0)));
        }
    }
}
