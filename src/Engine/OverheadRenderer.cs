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


        public OverheadRenderer(MapGeometry map)
        {
            _map = map;
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

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN

            // Maps have an origin in the bottom left.  Positive Y is UP

            var screenDimensionsV = screen.Dimensions.ToVector2();
            var desiredMapScreenLength = screen.Dimensions.SmallestSide() * _mapToScreenRatio;
            var largestMapSide = _map.Area.LargestSide();

            var gameToScreenFactor = desiredMapScreenLength / largestMapSide;

            var mapSizeInScreenCoords = _map.Area * gameToScreenFactor;
            var centeringOffset = (screenDimensionsV - mapSizeInScreenCoords) / 2;

            Point ConvertToScreenCoords(Vector2 gameCoordinate) =>
                (centeringOffset + (gameCoordinate - _map.BottomLeftCorner) * gameToScreenFactor).ToPoint().InvertY(screen.Height);

            foreach (var lineDef in _map.Map.LineDefs)
            {
                ref Vector2 vertex1 = ref _map.GetVertex(lineDef.V1);
                ref Vector2 vertex2 = ref _map.GetVertex(lineDef.V2);

                var isPlayerInThisSector =
                    _map.Map.SideDefs[lineDef.SideFront].Sector == player.CurrentSectorId ||
                    (lineDef.TwoSided && _map.Map.SideDefs[lineDef.SideBack].Sector == player.CurrentSectorId);

                var lineColor =
                    isPlayerInThisSector ?
                        (lineDef.TwoSided ? Color.DarkRed : Color.Red) :
                        (lineDef.TwoSided ? Color.DarkGray : Color.White);

                var p1 = ConvertToScreenCoords(vertex1);
                var p2 = ConvertToScreenCoords(vertex2);
                screen.PlotLineSafe(p1, p2, lineColor);

                // Draw front side indication
                var lineDirection = vertex2 - vertex1;
                var lineMidPoint = vertex1 + lineDirection / 2;

                var perpendicularDirection = lineDirection.PerpendicularClockwise();
                perpendicularDirection.Normalize();

                var frontMarkerlineEnd = lineMidPoint + perpendicularDirection * FrontSideMarkerLength;

                screen.PlotLineSafe(ConvertToScreenCoords(lineMidPoint), ConvertToScreenCoords(frontMarkerlineEnd), lineColor);
            }

            // Circle every vertex
            foreach (var vertexInScreenCoords in _map.Vertices.Select(ConvertToScreenCoords))
            {
                screen.PlotCircleSafe(vertexInScreenCoords, (int)(gameToScreenFactor * VertexSize), Color.Aqua);
            }

            // Draw player position
            var playerPositionInScreenCoords = ConvertToScreenCoords(player.Position);
            var playerRadiusInScreenSize = (int)(player.Radius * gameToScreenFactor);
            screen.PlotCircleSafe(playerPositionInScreenCoords, playerRadiusInScreenSize, Color.Green);

            // Draw player direction
            var playerLineEnd = ConvertToScreenCoords(player.Position + player.Radius * player.Direction);
            screen.PlotLineSafe(playerPositionInScreenCoords, playerLineEnd, Color.LightGreen);
        }
    }
}