// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class OverheadRenderer : IRenderer
    {
        delegate void DrawLine(ScreenBuffer buffer, Point p0, Point p1, Color c);

        readonly GameSettings _settings;
        readonly MapGeometry _map;

        const float VertexSize = 3f;
        const float FrontSideMarkerLength = 5f;
        private const float MsToZoomSpeed = 0.001f;
        private const float MinMapToScreenRatio = 0.2f;
        private const float MaxMapToScreenRatio = 5f;
        private const float DefaultMapToScreenRatio = 0.9f;
        private float _mapToScreenRatio = DefaultMapToScreenRatio;
        DrawLine _drawLine = ScreenBufferExtensions.PlotLine;
        private const float MsToMoveSpeed = 200f / 1000f;
        Vector2 _viewOffset = Vector2.Zero;

        public OverheadRenderer(GameSettings settings, MapGeometry map)
        {
            _settings = settings;
            _map = map;

            _settings.FollowModeChanged += (s, e) => _viewOffset = Vector2.Zero;
            _settings.DrawAntiAliasedModeChanged += (s, e) => PickLineDrawer();
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
            if (!_settings.FollowMode)
            {
                var distance = gameTime.ElapsedGameTime.Milliseconds * MsToMoveSpeed;

                if (inputs.Forward)
                {
                    _viewOffset -= Vector2.UnitY * distance;
                }
                else if (inputs.Backward)
                {
                    _viewOffset += Vector2.UnitY * distance;
                }

                if (inputs.TurnRight || inputs.StrafeRight)
                {
                    _viewOffset -= Vector2.UnitX * distance;
                }
                else if (inputs.TurnLeft || inputs.StrafeLeft)
                {
                    _viewOffset += Vector2.UnitX * distance;
                }
            }

            if (inputs.ZoomIn)
            {
                var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
                _mapToScreenRatio = Math.Min(MaxMapToScreenRatio, _mapToScreenRatio * (1f + zoomAmount));
            }
            else if (inputs.ZoomOut)
            {
                var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
                _mapToScreenRatio = Math.Max(MinMapToScreenRatio, _mapToScreenRatio / (1f + zoomAmount));
            }
            else if (inputs.ResetZoom)
            {
                _mapToScreenRatio = DefaultMapToScreenRatio;
            }
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN

            // Maps have an origin in the bottom left.  Positive Y is UP

            // TODO: The scale shouldn't depend on the size of the map...
            var screenDimensionsV = screen.Dimensions.ToVector2();
            var desiredMapScreenLength = screen.Dimensions.SmallestSide() * _mapToScreenRatio;
            var largestMapSide = _map.Area.LargestSide();

            var gameToScreenFactor = desiredMapScreenLength / largestMapSide;

            var screenCenterInMapCoords = screenDimensionsV / gameToScreenFactor / 2;
            var playerCenteringOffset = screenCenterInMapCoords - player.Position;

            Point ToScreenCoords(Vector2 worldCoordinate)
            {
                var shiftedWorldCoordinate = worldCoordinate;

                if (_settings.RotateMode)
                {
                    // translate coordinate to be relative to player
                    shiftedWorldCoordinate -= player.Position;

                    var rotation = Matrix.CreateRotationZ(-(float)Math.Atan2(player.Direction.Y, player.Direction.X) + MathHelper.PiOver2);

                    var rotatedCoord = Vector2.Transform(shiftedWorldCoordinate, rotation);
                    shiftedWorldCoordinate = rotatedCoord + player.Position;
                }

                shiftedWorldCoordinate += playerCenteringOffset + _viewOffset;

                return (shiftedWorldCoordinate * gameToScreenFactor).ToPoint().InvertY(screen.Height);
            }

            void DrawLine(Vector2 wc1, Vector2 wc2, Color c)
            {
                var sc1 = ToScreenCoords(wc1);
                var sc2 = ToScreenCoords(wc2);

                //Do some rudimentary clipping to eliminate lines that can't possibly show up on screen
                if ((sc1.X < 0 && sc2.X < 0) ||
                    (sc1.X >= screen.Width && sc2.X >= screen.Width) ||
                    (sc1.Y < 0 && sc2.Y < 0) ||
                    (sc1.Y >= screen.Height && sc2.Y >= screen.Height))
                {
                    return;
                }

                _drawLine(screen, sc1, sc2, c);
            }

            void DrawBox(Vector2 center, float halfWidth, Color c)
            {
                var topLeft = center + new Vector2(-halfWidth, halfWidth);
                var topRight = center + new Vector2(halfWidth, halfWidth);
                var bottomLeft = center + new Vector2(-halfWidth, -halfWidth);
                var bottomRight = center + new Vector2(halfWidth, -halfWidth);

                DrawLine(topLeft, topRight, c);
                DrawLine(topRight, bottomRight, c);
                DrawLine(bottomRight, bottomLeft, c);
                DrawLine(bottomLeft, topLeft, c);
            }

            void DrawVertex(Vector2 wc, Color c) => DrawBox(wc, 2f, c);

            foreach (var lineDef in _map.Map.LineDefs)
            {
                ref Vector2 vertex1 = ref _map.Vertices[lineDef.V1];
                ref Vector2 vertex2 = ref _map.Vertices[lineDef.V2];

                var lineSurroundsPlayerSector =
                    _map.Map.SideDefs[lineDef.SideFront].Sector == player.CurrentSectorId ||
                    (lineDef.TwoSided && _map.Map.SideDefs[lineDef.SideBack].Sector == player.CurrentSectorId);

                var lineColor =
                    lineSurroundsPlayerSector ?
                        (lineDef.TwoSided ? Color.DarkRed : Color.Red) :
                        (lineDef.TwoSided ? Color.DimGray : Color.White);

                DrawLine(vertex1, vertex2, lineColor);

                // Draw front side indication
                var lineDirection = vertex2 - vertex1;
                var lineMidPoint = vertex1 + lineDirection / 2;

                var perpendicularDirection = lineDirection.PerpendicularClockwise();
                perpendicularDirection.Normalize();

                var frontMarkerLineEnd = lineMidPoint + perpendicularDirection * FrontSideMarkerLength;

                DrawLine(lineMidPoint, frontMarkerLineEnd, lineColor);
            }

            // Circle every vertex
            foreach (var vertex in _map.Vertices)
            {
                DrawVertex(vertex, Color.DeepSkyBlue);
            }

            // Draw player position
            var playerHalfWidth = player.Width / 2;
            DrawBox(player.Position, halfWidth: playerHalfWidth, Color.Green);

            // Draw player direction arrow
            var playerLineStart = player.Position - (playerHalfWidth / 2 * player.Direction);
            var playerLineEnd = player.Position + (playerHalfWidth / 2 * player.Direction);
            DrawLine(playerLineStart, playerLineEnd, Color.LightGreen);

            var perpendicularPlayerDirection = player.Direction.PerpendicularClockwise();
            var baseOfArrow = player.Position + (playerHalfWidth / 6 * player.Direction);
            var arrowBaseHalfWidth = playerHalfWidth / 3.5f;

            var rightBaseOfArrow = baseOfArrow + (arrowBaseHalfWidth * perpendicularPlayerDirection);
            DrawLine(rightBaseOfArrow, playerLineEnd, Color.LightGreen);

            var leftBaseOfArrow = baseOfArrow - (arrowBaseHalfWidth * perpendicularPlayerDirection);
            DrawLine(leftBaseOfArrow, playerLineEnd, Color.LightGreen);
        }
    }
}