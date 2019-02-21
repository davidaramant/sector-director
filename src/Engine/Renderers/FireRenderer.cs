// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class FireRenderer : IRenderer
    {
        // http://fabiensanglard.net/doom_fire_psx/

        readonly Color[] _palette = new[]
        {
            new Color(0x07,0x07,0x07),
            new Color(0x1F,0x07,0x07),
            new Color(0x2F,0x0F,0x07),
            new Color(0x47,0x0F,0x07),
            new Color(0x57,0x17,0x07),
            new Color(0x67,0x1F,0x07),
            new Color(0x77,0x1F,0x07),
            new Color(0x8F,0x27,0x07),
            new Color(0x9F,0x2F,0x07),
            new Color(0xAF,0x3F,0x07),
            new Color(0xBF,0x47,0x07),
            new Color(0xC7,0x47,0x07),
            new Color(0xDF,0x4F,0x07),
            new Color(0xDF,0x57,0x07),
            new Color(0xDF,0x57,0x07),
            new Color(0xD7,0x5F,0x07),
            new Color(0xD7,0x5F,0x07),
            new Color(0xD7,0x67,0x0F),
            new Color(0xCF,0x6F,0x0F),
            new Color(0xCF,0x77,0x0F),
            new Color(0xCF,0x7F,0x0F),
            new Color(0xCF,0x87,0x17),
            new Color(0xC7,0x87,0x17),
            new Color(0xC7,0x8F,0x17),
            new Color(0xC7,0x97,0x1F),
            new Color(0xBF,0x9F,0x1F),
            new Color(0xBF,0x9F,0x1F),
            new Color(0xBF,0xA7,0x27),
            new Color(0xBF,0xA7,0x27),
            new Color(0xBF,0xAF,0x2F),
            new Color(0xB7,0xAF,0x2F),
            new Color(0xB7,0xB7,0x2F),
            new Color(0xB7,0xB7,0x37),
            new Color(0xCF,0xCF,0x6F),
            new Color(0xDF,0xDF,0x9F),
            new Color(0xEF,0xEF,0xC7),
            new Color(0xFF,0xFF,0xFF)
        };

        static readonly Point FireSize = new Point(300, 200);
        byte[] _fireBuffer = new byte[FireSize.Area()];
        Random _rand = new Random();
        readonly TimeSpan _updateFrequency = TimeSpan.FromSeconds(1 / 10);
        TimeSpan _lastUpdate;

        bool _pressingLeft = false;
        bool _pressingRight = false;
        bool _killFire = false;

        public FireRenderer()
        {
            InitializeFire();
        }

        void InitializeFire()
        {
            Array.Clear(_fireBuffer, 0, _fireBuffer.Length);
            // set last row to white
            for (int x = 0; x < FireSize.X; x++)
            {
                _fireBuffer[(FireSize.Y - 1) * FireSize.X + x] = (byte)(_palette.Length - 1);
            }
            _killFire = false;
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {
            if (inputs.TurnLeft)
            {
                if (!_pressingLeft)
                {
                    _pressingLeft = true;
                    _killFire = true;
                }
            }
            else
            {
                _pressingLeft = false;
            }

            if (inputs.TurnRight)
            {
                if (!_pressingRight)
                {
                    _pressingRight = true;
                    InitializeFire();
                }
            }
            else
            {
                _pressingRight = false;
            }


            if (gameTime.TotalGameTime - _lastUpdate > _updateFrequency)
            {
                _lastUpdate = gameTime.TotalGameTime;
                if (_killFire)
                {
                    StopFire();
                }
                UpdateFire();
            }
        }

        void SpreadFire(int pixelIndex)
        {
            var colorIndex = _fireBuffer[pixelIndex];
            if (colorIndex == 0)
            {
                _fireBuffer[pixelIndex - FireSize.X] = 0;
            }
            else
            {
                var randomOffset = _rand.Next(3);
                var randomColorOffset = _rand.Next(2);
                var destinationIndex = pixelIndex - randomOffset + 1;
                _fireBuffer[destinationIndex - FireSize.X] = (byte)(colorIndex - randomColorOffset);
            }
        }

        void UpdateFire()
        {
            for (int x = 0; x < FireSize.X; x++)
            {
                for (int y = 1; y < FireSize.Y; y++)
                {
                    SpreadFire(y * FireSize.X + x);
                }
            }
        }

        void StopFire()
        {
            // Decrement the intensity of the bottom rows
            const int rowsToDecrement = 8;
            for (var y = 0; y < rowsToDecrement; y++)
            {
                for (int x = 0; x < FireSize.X; x++)
                {
                    var index = (FireSize.Y - 1 - y) * FireSize.X + x;
                    var current = _fireBuffer[index];
                    _fireBuffer[index] = (byte)Math.Max(0, current - _rand.Next(1, 3));
                }
            }
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            var screenCenter = screen.Dimensions.DivideBy(2);
            var fireCenter = FireSize.DivideBy(2);
            var offset = screenCenter - fireCenter;

            for (int y = 0; y < FireSize.Y; y++)
            {
                for (int x = 0; x < FireSize.X; x++)
                {
                    var colorIndex = _fireBuffer[y * FireSize.X + x];
                    var color = _palette[colorIndex];
                    screen.DrawPixel(offset.X + x, offset.Y + y, color);
                }
            }
        }
    }
}