// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public sealed class OverheadRenderer
    {
        readonly MapData _map;
        const float _mapToScreenFactor = 0.9f;
        readonly Point[] _vertices;

        readonly Point _mapCorner;
        readonly Point _mapSize;

        public OverheadRenderer(MapData map)
        {
            _map = map;
            _vertices = _map.Vertices.Select(v => v.ToPoint()).ToArray();

            var minX = _vertices.Min(p => p.X);
            var maxX = _vertices.Max(p => p.X);
            var minY = _vertices.Min(p => p.Y);
            var maxY = _vertices.Max(p => p.Y);

            _mapCorner = new Point(minX, minY);
            _mapSize = new Point(maxX - minX, maxY - minY);
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN

            // Maps have an origin in the bottom left.  Positive Y is UP

            var desiredMapScreenLength = screen.Dimensions.SmallestSide() * _mapToScreenFactor;
            var largestMapSide = _mapSize.LargestSide();

            var gameToScreenFactor = desiredMapScreenLength / largestMapSide;

            var mapSizeInScreenCoords = _mapSize.Scale(gameToScreenFactor);
            var centeringOffset = (screen.Dimensions - mapSizeInScreenCoords).DivideBy(2);

            Point ConvertToScreenCoords(Point gameCoordinate) =>
                (centeringOffset + (gameCoordinate - _mapCorner).Scale(gameToScreenFactor)).InvertY(screen.Height);

            foreach (var lineDef in _map.LineDefs)
            {
                var vertex1 = _vertices[lineDef.V1];
                var vertex2 = _vertices[lineDef.V2];

                var color = lineDef.TwoSided ? Color.Gray : Color.White;

                var p1 = ConvertToScreenCoords(vertex1);
                var p2 = ConvertToScreenCoords(vertex2);
                screen.PlotLineSafe(p1, p2, color);
            }

            var playerPositionInScreenCoords = ConvertToScreenCoords(player.Position.ToPoint());
            var playerRadiusInScreenSize = (int)(player.Radius * gameToScreenFactor);
            screen.PlotCircleSafe(playerPositionInScreenCoords, playerRadiusInScreenSize, Color.Green);
        }
    }
}