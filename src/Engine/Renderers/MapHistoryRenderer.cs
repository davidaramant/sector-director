// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Drawing;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class MapHistoryRenderer : IRenderer
    {
        readonly MapGeometry _map;
        private readonly ScreenMessage _screenMessage;
        private readonly Point[] _verticesInScreenCoords;

        const float FrontSideMarkerLength = 5f;
        private const float DefaultMapToScreenRatio = 0.95f;

        private float _linesToDraw = 0;
        private const float MsToDrawSpeedDelta = 0.0001f;
        private const float DefaultMsToDrawSpeed = 0.05f;
        private float _msToDrawSpeed = DefaultMsToDrawSpeed;

        public MapHistoryRenderer(MapGeometry map, ScreenMessage screenMessage)
        {
            _map = map;
            _screenMessage = screenMessage;
            _verticesInScreenCoords = new Point[map.Vertices.Length];
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {
            var drawSpeedChangeDelta = gameTime.ElapsedGameTime.Milliseconds * MsToDrawSpeedDelta;

            if (inputs.ResetZoom)
            {
                _linesToDraw = 0;
                _msToDrawSpeed = DefaultMsToDrawSpeed;
                _screenMessage.ShowMessage($"Set speed to {_msToDrawSpeed}");
            }
            else if(inputs.ZoomIn)
            {
                _msToDrawSpeed += drawSpeedChangeDelta;
                _screenMessage.ShowMessage($"Set speed to {_msToDrawSpeed}");
            }
            else if(inputs.ZoomOut)
            {
                _msToDrawSpeed = Math.Max(0, _msToDrawSpeed - drawSpeedChangeDelta);
                _screenMessage.ShowMessage($"Set speed to {_msToDrawSpeed}");
            }

            _linesToDraw = Math.Min(_map.Lines.Length, _linesToDraw + _msToDrawSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();

            // The screen has an origin in the top left.  Positive Y is DOWN
            // Maps have an origin in the bottom left.  Positive Y is UP

            var screenDimensionsV = screen.Dimensions.ToVector2();
            var desiredMapScreenBounds = screenDimensionsV * DefaultMapToScreenRatio;

            var gameToScreenFactor = Math.Min( desiredMapScreenBounds.X/_map.Area.X, desiredMapScreenBounds.Y/_map.Area.Y);

            var screenAreaInMapCoords = screenDimensionsV / gameToScreenFactor;
            var mapCenteringOffset = (screenAreaInMapCoords - _map.Area) / 2 - _map.BottomLeftCorner;

            // Transform all vertices
            for (int v = 0; v < _map.Vertices.Length; v++)
            {
                _verticesInScreenCoords[v] = ToScreenCoords(_map.Vertices[v]);
            }

            Point ToScreenCoords(Vector2 worldCoordinate)
            {
                var shiftedWorldCoordinate = worldCoordinate + mapCenteringOffset;

                // This fixes jittering
                var pixelOffset = new Vector2(0.5f, 0.5f);

                return ((shiftedWorldCoordinate * gameToScreenFactor) + pixelOffset).ToPoint().InvertY(screen.Height);
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
                    screen.PlotLineSmooth(result.p0, result.p1, c);
                }
            }

            foreach (var lineDef in _map.Map.LineDefs.Take((int)_linesToDraw))
            {
                ref Vector2 vertex1 = ref _map.Vertices[lineDef.V1];
                ref Vector2 vertex2 = ref _map.Vertices[lineDef.V2];

                var lineColor = lineDef.TwoSided ? Color.Gray : Color.Red;

                DrawLineFromVertices(lineDef.V1, lineDef.V2, lineColor);

                // Draw front side indication
                var lineDirection = vertex2 - vertex1;
                var lineMidPoint = vertex1 + lineDirection / 2;

                var perpendicularDirection = lineDirection.PerpendicularClockwise();
                perpendicularDirection.Normalize();

                var frontMarkerLineEnd = lineMidPoint + perpendicularDirection * FrontSideMarkerLength;

                DrawLineFromWorldCoordinates(lineMidPoint, frontMarkerLineEnd, lineColor);
            }
        }
    }
}