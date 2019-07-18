// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using NUnit.Framework;
using SectorDirector.Engine.Renderers;

namespace SectorDirector.Engine.Tests.Renderers
{
    [TestFixture]
    public sealed class Camera2DTests
    {
        private const int ScreenWidth = 1920;
        private const int ScreenHeight = 1200;

        [TestCase(0, 0, ScreenWidth / 2, ScreenHeight / 2)]
        [TestCase(10, 0, ScreenWidth / 2 + 10, ScreenHeight / 2)]
        [TestCase(-10, 0, ScreenWidth / 2 - 10, ScreenHeight / 2)]
        [TestCase(0, 10, ScreenWidth / 2, ScreenHeight / 2 - 10)]
        [TestCase(0, -10, ScreenWidth / 2, ScreenHeight / 2 + 10)]
        public void ShouldAccountForDifferentOrigin(int playerX, int playerY, int expectedScreenX, int expectedScreenY)
        {
            var camera = new Camera2D
            {
                ScreenBounds = new Point(ScreenWidth, ScreenHeight),
            };

            var screenPosition = camera.WorldToScreen(new Vector2(playerX, playerY));

            Assert.That(screenPosition.X, Is.EqualTo(expectedScreenX + 0.5f), "Incorrect X value");
            Assert.That(screenPosition.Y, Is.EqualTo(expectedScreenY - 0.5f), "Incorrect Y value");
        }

        [TestCase(0, 0, ScreenWidth / 2 - 10, ScreenHeight / 2 + 10)]
        [TestCase(10, 10, ScreenWidth / 2, ScreenHeight / 2)]
        public void ShouldHandleDifferentCenter(int playerX, int playerY, int expectedScreenX, int expectedScreenY)
        {
            var camera = new Camera2D
            {
                Center = new Vector2(10,10),
                ScreenBounds = new Point(ScreenWidth, ScreenHeight),
            };

            var screenPosition = camera.WorldToScreen(new Vector2(playerX, playerY));

            Assert.That(screenPosition.X, Is.EqualTo(expectedScreenX + 0.5f), "Incorrect X value");
            Assert.That(screenPosition.Y, Is.EqualTo(expectedScreenY - 0.5f), "Incorrect Y value");
        }

        [TestCase(0, 0, ScreenWidth / 2 - 20, ScreenHeight / 2 + 20)]
        [TestCase(20, 20, ScreenWidth / 2, ScreenHeight / 2)]
        public void ShouldHandleOffsetFromCenter(int playerX, int playerY, int expectedScreenX, int expectedScreenY)
        {
            var camera = new Camera2D
            {
                Center = new Vector2(10,10),
                ViewOffset = new Vector2(10,10),
                ScreenBounds = new Point(ScreenWidth, ScreenHeight),
            };

            var screenPosition = camera.WorldToScreen(new Vector2(playerX, playerY));

            Assert.That(screenPosition.X, Is.EqualTo(expectedScreenX + 0.5f), "Incorrect X value");
            Assert.That(screenPosition.Y, Is.EqualTo(expectedScreenY - 0.5f), "Incorrect Y value");
        }

        public void ShouldScaleCorrectly(int playerX, int playerY, int scale, int expectedScreenX, int expectedScreenY)
        {

        }
    }
}