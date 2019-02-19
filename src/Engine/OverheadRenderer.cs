// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class OverheadRenderer
    {
        readonly MapGeometry _map;
        const float VertexSize = 3f;
        const float FrontSideMarkerLength = 5f;
        private const float MsToZoomSpeed = 0.001f;
        private const float MinMapToScreenRatio = 0.2f;
        private const float MaxMapToScreenRatio = 5f;
        private const float DefaultMapToScreenRatio = 0.9f;
        private float _mapToScreenRatio = DefaultMapToScreenRatio;

        private const float MsToMoveSpeed = 200f / 1000f;
        Vector2 _viewOffset = Vector2.Zero;

        public bool FollowMode { get; private set; } = true;
        public bool RotateMode { get; private set; } = false;

        public OverheadRenderer(MapGeometry map)
        {
            _map = map;
        }

        public void ToggleFollowMode()
        {
            FollowMode = !FollowMode;
            _viewOffset = Vector2.Zero;
        }

        public void ToggleRotateMode()
        {
            RotateMode = !RotateMode;
        }

        public void ResetZoom()
        {
            _mapToScreenRatio = DefaultMapToScreenRatio;
        }

        public void ZoomIn(GameTime gameTime)
        {
            var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
            _mapToScreenRatio = Math.Min(MaxMapToScreenRatio, _mapToScreenRatio * (1f + zoomAmount));
        }

        public void ZoomOut(GameTime gameTime)
        {
            var zoomAmount = gameTime.ElapsedGameTime.Milliseconds * MsToZoomSpeed;
            _mapToScreenRatio = Math.Max(MinMapToScreenRatio, _mapToScreenRatio / (1f + zoomAmount));
        }

        public void UpdateView(MovementInputs inputs, GameTime gameTime)
        {
            var distance = gameTime.ElapsedGameTime.Milliseconds * MsToMoveSpeed;

            if (inputs.HasFlag(MovementInputs.Forward))
            {
                _viewOffset -= Vector2.UnitY * distance;
            }
            else if (inputs.HasFlag(MovementInputs.Backward))
            {
                _viewOffset += Vector2.UnitY * distance;
            }

            if (inputs.HasFlag(MovementInputs.TurnRight) || inputs.HasFlag(MovementInputs.StrafeRight))
            {
                _viewOffset -= Vector2.UnitX * distance;
            }
            else if (inputs.HasFlag(MovementInputs.TurnLeft) || inputs.HasFlag(MovementInputs.StrafeLeft))
            {
                _viewOffset += Vector2.UnitX * distance;
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

            Point ToScreenCoords(Vector2 gameCoordinate)
            {
                var shiftedGameCoordinate = gameCoordinate;

                if (RotateMode)
                {
                    // translate coordinate to be relative to player
                    shiftedGameCoordinate -= player.Position;

                    var rotation = Matrix.CreateRotationZ(-(float)Math.Atan2(player.Direction.Y, player.Direction.X) + MathHelper.PiOver2);

                    var rotatedCoord = Vector2.Transform(shiftedGameCoordinate, rotation);
                    shiftedGameCoordinate = rotatedCoord + player.Position;
                }

                shiftedGameCoordinate += playerCenteringOffset + _viewOffset;

                return (shiftedGameCoordinate * gameToScreenFactor).ToPoint().InvertY(screen.Height);
            }

            foreach (var lineDef in _map.Map.LineDefs)
            {
                ref Vector2 vertex1 = ref _map.Vertices[lineDef.V1];
                ref Vector2 vertex2 = ref _map.Vertices[lineDef.V2];

                var isPlayerInThisSector =
                    _map.Map.SideDefs[lineDef.SideFront].Sector == player.CurrentSectorId ||
                    (lineDef.TwoSided && _map.Map.SideDefs[lineDef.SideBack].Sector == player.CurrentSectorId);

                var lineColor =
                    isPlayerInThisSector ?
                        (lineDef.TwoSided ? Color.DarkRed : Color.Red) :
                        (lineDef.TwoSided ? Color.DarkGray : Color.White);

                var p1 = ToScreenCoords(vertex1);
                var p2 = ToScreenCoords(vertex2);
                screen.PlotLine(p1, p2, lineColor);

                // Draw front side indication
                var lineDirection = vertex2 - vertex1;
                var lineMidPoint = vertex1 + lineDirection / 2;

                var perpendicularDirection = lineDirection.PerpendicularClockwise();
                perpendicularDirection.Normalize();

                var frontMarkerLineEnd = lineMidPoint + perpendicularDirection * FrontSideMarkerLength;

                screen.PlotLine(ToScreenCoords(lineMidPoint), ToScreenCoords(frontMarkerLineEnd), lineColor);
            }

            // Circle every vertex
            foreach (var vertexInScreenCoords in _map.Vertices.Select(ToScreenCoords))
            {
                screen.PlotCircleSafe(vertexInScreenCoords, (int)(gameToScreenFactor * VertexSize), Color.Aqua);
            }

            // Draw player position
            var playerPositionInScreenCoords = ToScreenCoords(player.Position);
            var halfWidth = player.Width / 2;
            var playerTopLeft = ToScreenCoords(player.Position + new Vector2(-halfWidth, halfWidth));
            var playerTopRight = ToScreenCoords(player.Position + new Vector2(halfWidth, halfWidth));
            var playerBottomLeft = ToScreenCoords(player.Position + new Vector2(-halfWidth, -halfWidth));
            var playerBottomRight = ToScreenCoords(player.Position + new Vector2(halfWidth, -halfWidth));

            screen.PlotLine(playerTopLeft, playerTopRight, Color.Green);
            screen.PlotLine(playerTopRight, playerBottomRight, Color.Green);
            screen.PlotLine(playerBottomRight, playerBottomLeft, Color.Green);
            screen.PlotLine(playerBottomLeft, playerTopLeft, Color.Green);

            // Draw player direction arrow
            var playerLineStart = player.Position - (halfWidth / 2 * player.Direction);
            var playerLineEnd = player.Position + (halfWidth / 2 * player.Direction);
            screen.PlotLine(ToScreenCoords(playerLineStart), ToScreenCoords(playerLineEnd), Color.LightGreen);

            var perpendicularPlayerDirection = player.Direction.PerpendicularClockwise();
            var baseOfArrow = player.Position + (halfWidth / 5 * player.Direction);
            var arrowBaseHalfWidth = halfWidth/3;

            var rightBaseOfArrow = baseOfArrow + (arrowBaseHalfWidth * perpendicularPlayerDirection);
            screen.PlotLine(ToScreenCoords(rightBaseOfArrow), ToScreenCoords(playerLineEnd), Color.LightGreen);

            var leftBaseOfArrow = baseOfArrow - (arrowBaseHalfWidth * perpendicularPlayerDirection);
            screen.PlotLine(ToScreenCoords(leftBaseOfArrow), ToScreenCoords(playerLineEnd), Color.LightGreen);
        }
    }
}