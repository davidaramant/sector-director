﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Drawing;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class LineTestRenderer : IRenderer
    {
        delegate void DrawLine(IScreenBuffer buffer, Point p0, Point p1, Color c);
        DrawLine _drawLine = ScreenBufferExtensions.PlotLine;
        float _angle = 0;
        private readonly ScreenMessage _message;
        private const float MsToGammaSpeed = 0.001f;
        private const float MsToRadiansDeltaSpeed = 0.000001f;
        private float _msToRadians = 0.1f / 1000f;
        private GameSettings _settings;

        public LineTestRenderer(GameSettings settings, ScreenMessage message)
        {
            _settings = settings;
            settings.DrawAntiAliasedModeChanged += (s, e) => PickLineDrawer();
            _message = message;
            PickLineDrawer();
        }

        private void PickLineDrawer()
        {
            if (_settings.DrawAntiAliased)
            {
                _drawLine = ScreenBufferExtensions.PlotLineSmooth;
            }
            else
            {
                _drawLine = ScreenBufferExtensions.PlotLine;
            }
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {
            var rotationDelta = gameTime.ElapsedGameTime.Milliseconds * MsToRadiansDeltaSpeed;

            if (inputs.TurnLeft)
            {
                _msToRadians -= rotationDelta;
            }
            else if (inputs.TurnRight)
            {
                _msToRadians += rotationDelta;
            }
            else if(inputs.Forward)
            {
                _msToRadians = 0;
            }

            if (inputs.ZoomIn)
            {
                var changeAmount = gameTime.ElapsedGameTime.Milliseconds * MsToGammaSpeed;
                ScreenBufferExtensions.GammaExponent += changeAmount;
                _message.ShowMessage($"Current gamma: {ScreenBufferExtensions.GammaExponent}");
            }
            else if (inputs.ZoomOut)
            {
                var changeAmount = gameTime.ElapsedGameTime.Milliseconds * MsToGammaSpeed;
                ScreenBufferExtensions.GammaExponent -= changeAmount;
                _message.ShowMessage($"Current gamma: {ScreenBufferExtensions.GammaExponent}");
            }

            var rotationRadians = gameTime.ElapsedGameTime.Milliseconds * _msToRadians;
            _angle += rotationRadians;
        }

        public void Render(IScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            var center = screen.Dimensions.DivideBy(2).ToVector2();
            var shortestSide = center.SmallestSide();

            var radius = 0.9f * shortestSide;

            const int numSegments = 5;
            var radianOffset = MathHelper.TwoPi / numSegments / 2;
            
            // This fixes jittering
            var pixelOffset = new Vector2(0.5f, 0.5f);
            
            foreach (var segment in Enumerable.Range(0, numSegments))
            {
                var rotation = Matrix.CreateRotationZ(segment * radianOffset + _angle);
                var direction = Vector2.Transform(Vector2.UnitX, rotation);

                var start = (center - direction * radius + pixelOffset).ToPoint();
                var end = (center + direction * radius + pixelOffset).ToPoint();

                _drawLine(screen, start, end, Color.Red);
            }
        }
    }
}