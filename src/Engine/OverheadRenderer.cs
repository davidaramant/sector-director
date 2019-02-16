// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class OverheadRenderer
    {
        readonly MapGeometry _map;
        const float _mapToScreenFactor = 0.9f;

        public OverheadRenderer(MapGeometry map)
        {
            _map = map;
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN

            // Maps have an origin in the bottom left.  Positive Y is UP

            var screenDimensionsV = screen.Dimensions.ToVector2();

            var desiredMapScreenLength = screen.Dimensions.SmallestSide() * _mapToScreenFactor;
            var largestMapSide = _map.Area.LargestSide();

            var gameToScreenFactor = desiredMapScreenLength / largestMapSide;

            var mapSizeInScreenCoords = _map.Area * gameToScreenFactor;
            var centeringOffset = (screenDimensionsV - mapSizeInScreenCoords) / 2;

            Point ConvertToScreenCoords(Vector2 gameCoordinate) =>
                (centeringOffset + (gameCoordinate - _map.BottomLeftCorner) * gameToScreenFactor).ToPoint().InvertY(screen.Height);

            foreach (var lineDef in _map.Map.LineDefs)
            {
                var vertex1 = _map.Vertices[lineDef.V1];
                var vertex2 = _map.Vertices[lineDef.V2];

                var color = lineDef.TwoSided ? Color.Gray : Color.Red;

                var p1 = ConvertToScreenCoords(vertex1);
                var p2 = ConvertToScreenCoords(vertex2);
                screen.PlotLineSafe(p1, p2, color);
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