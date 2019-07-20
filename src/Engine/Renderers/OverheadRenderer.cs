// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Drawing;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class OverheadRenderer : IRenderer
    {
        delegate void DrawLine(ScreenBuffer buffer, Point p0, Point p1, Color c);

        readonly GameSettings _settings;
        readonly MapGeometry _map;
        private readonly Point[] _verticesInScreenCoords;

        const float VertexHalfWidth = 2f;
        const float FrontSideMarkerLength = 5f;
        const float MsToZoomSpeed = 0.001f;
        const float MinMapToScreenRatio = 0.2f;
        const float MaxMapToScreenRatio = 5f;
        private readonly Camera2D _camera = new Camera2D();
        DrawLine _drawLine = ScreenBufferExtensions.PlotLine;
        private const float MsToMoveSpeed = 200f / 1000f;

        public OverheadRenderer(GameSettings settings, MapGeometry map)
        {
            _settings = settings;
            _map = map;
            _verticesInScreenCoords = new Point[map.Vertices.Length];

            _settings.FollowModeChanged += (s, e) => _camera.ViewOffset = Vector2.Zero;
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
                    _camera.ViewOffset += Vector2.UnitY * distance;
                }
                else if (inputs.Backward)
                {
                    _camera.ViewOffset -= Vector2.UnitY * distance;
                }

                if (inputs.TurnRight || inputs.StrafeRight)
                {
                    _camera.ViewOffset += Vector2.UnitX * distance;
                }
                else if (inputs.TurnLeft || inputs.StrafeLeft)
                {
                    _camera.ViewOffset -= Vector2.UnitX * distance;
                }
            }

            if (inputs.ZoomIn)
            {
                var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
                _camera.Zoom = Math.Min(MaxMapToScreenRatio, _camera.Zoom * (1f + zoomAmount));
            }
            else if (inputs.ZoomOut)
            {
                var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
                _camera.Zoom = Math.Max(MinMapToScreenRatio, _camera.Zoom / (1f + zoomAmount));
            }
            else if (inputs.ResetZoom)
            {
                _camera.Zoom = 1;
            }
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN
            // Maps have an origin in the bottom left.  Positive Y is UP

            _camera.ScreenBounds = screen.Dimensions;
            _camera.Center = player.Position;

            if (_settings.RotateMode)
            {
                _camera.RotationInRadians = player.Angle;
            }
            else
            {
                _camera.RotationInRadians = 0;
            }

            // Transform all vertices
            for (int v = 0; v < _map.Vertices.Length; v++)
            {
                _verticesInScreenCoords[v] = ToScreenCoords(_map.Vertices[v]);
            }


            Point ToScreenCoords(Vector2 worldCoordinate)
            {
                return _camera.WorldToScreen(worldCoordinate).ToPoint();
            }

            void DrawLineFromVertices(int v1, int v2, Color c) =>
                DrawLineFromScreenCoordinates(_verticesInScreenCoords[v1], _verticesInScreenCoords[v2], c);
            void DrawLineFromWorldCoordinates(Vector2 wc1, Vector2 wc2, Color c)
            {
                var sc1 = ToScreenCoords(wc1);
                var sc2 = ToScreenCoords(wc2);
                DrawLineFromScreenCoordinates(sc1, sc2, c);
            }

            void DrawLineFromScreenCoordinates(Point sc1, Point sc2, Color c)
            {
                var result = LineClipping.ClipToScreen(screen, sc1, sc2);
                if (result.shouldDraw)
                {
                    _drawLine(screen, result.p0, result.p1, c);
                }
            }

            void DrawBox(Vector2 center, float halfWidth, Color c)
            {
                var topLeft = center + new Vector2(-halfWidth, halfWidth);
                var topRight = center + new Vector2(halfWidth, halfWidth);
                var bottomLeft = center + new Vector2(-halfWidth, -halfWidth);
                var bottomRight = center + new Vector2(halfWidth, -halfWidth);

                DrawLineFromWorldCoordinates(topLeft, topRight, c);
                DrawLineFromWorldCoordinates(topRight, bottomRight, c);
                DrawLineFromWorldCoordinates(bottomRight, bottomLeft, c);
                DrawLineFromWorldCoordinates(bottomLeft, topLeft, c);
            }

            void DrawVertex(Vector2 wc, Color c) => DrawBox(wc, VertexHalfWidth, c);

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

                DrawLineFromVertices(lineDef.V1, lineDef.V2, lineColor);

                // Draw front side indication
                var lineDirection = vertex2 - vertex1;
                var lineMidPoint = vertex1 + lineDirection / 2;

                var perpendicularDirection = lineDirection.PerpendicularClockwise();
                perpendicularDirection.Normalize();

                var frontMarkerLineEnd = lineMidPoint + perpendicularDirection * FrontSideMarkerLength;

                DrawLineFromWorldCoordinates(lineMidPoint, frontMarkerLineEnd, lineColor);
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
            DrawLineFromWorldCoordinates(playerLineStart, playerLineEnd, Color.LightGreen);

            var perpendicularPlayerDirection = player.Direction.PerpendicularClockwise();
            var baseOfArrow = player.Position + (playerHalfWidth / 6 * player.Direction);
            var arrowBaseHalfWidth = playerHalfWidth / 3.5f;

            var rightBaseOfArrow = baseOfArrow + (arrowBaseHalfWidth * perpendicularPlayerDirection);
            DrawLineFromWorldCoordinates(rightBaseOfArrow, playerLineEnd, Color.LightGreen);

            var leftBaseOfArrow = baseOfArrow - (arrowBaseHalfWidth * perpendicularPlayerDirection);
            DrawLineFromWorldCoordinates(leftBaseOfArrow, playerLineEnd, Color.LightGreen);
        }
    }
}