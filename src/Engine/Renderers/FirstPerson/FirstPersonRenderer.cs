// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using SectorDirector.Engine.Drawing;
using SectorDirector.Engine.Input;
using SectorDirector.Engine.Renderers.FirstPerson;

namespace SectorDirector.Engine.Renderers
{
    public sealed class FirstPersonRenderer : IRenderer
    {
        delegate void DrawLine(IScreenBuffer buffer, Point p0, Point p1, Color c);

        readonly GameSettings _settings;
        readonly ScreenMessage _screenMessage;
        readonly MapGeometry _map;

        private FirstPersonWorldInterpreter Interpreter;

        DrawLine _drawLine = ScreenBufferExtensions.PlotLine;


        public FirstPersonRenderer(GameSettings settings, MapGeometry map, ScreenMessage screenMessage)
        {
            _settings = settings;
            _map = map;
            _screenMessage = screenMessage;
            Interpreter = new FirstPersonWorldInterpreter();
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {

        }

        public void Render(IScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();
            Interpreter = new FirstPersonWorldInterpreter(player.CameraSettings);

            void DrawLineFromScreenCoordinates(Point sc1, Point sc2, Color c)
            {
                var result = LineClipping.ClipToScreen(screen, sc1, sc2);
                if (result.shouldDraw)
                {
                    _drawLine(screen, result.p0, result.p1, c);
                }
            }

            foreach (SectorInfo sector in _map.Sectors)
            {
                foreach (Line line in sector.Lines)
                {

                    Vector3 cameraPosition = new Vector3(player.Position, player.VerticalPosition + player.ViewHeight);

                    Vector3 topLeft = new Vector3(line.Vertex1.X, line.Vertex1.Y, sector.Info.HeightCeiling);
                    Vector3 topRight = new Vector3(line.Vertex2.X, line.Vertex2.Y, sector.Info.HeightCeiling);
                    Vector3 bottomLeft = new Vector3(line.Vertex1.X, line.Vertex1.Y, sector.Info.HeightFloor);
                    Vector3 bottomRight = new Vector3(line.Vertex2.X, line.Vertex2.Y, sector.Info.HeightFloor);

                    var topLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, topLeft, topRight);
                    if (topLineResult.shouldDraw)
                    {
                        DrawLineFromScreenCoordinates(topLineResult.p1, topLineResult.p2, Color.Red);
                    }

                    var bottomLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, bottomLeft, bottomRight);
                    if (bottomLineResult.shouldDraw)
                    {
                        DrawLineFromScreenCoordinates(bottomLineResult.p1, bottomLineResult.p2, Color.Red);
                    }

                    var leftLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, topLeft, bottomLeft);
                    if (leftLineResult.shouldDraw)
                    {
                        DrawLineFromScreenCoordinates(leftLineResult.p1, leftLineResult.p2, Color.Red);
                    }

                    var rightLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, topRight, bottomRight);
                    if (rightLineResult.shouldDraw)
                    {
                        DrawLineFromScreenCoordinates(rightLineResult.p1, rightLineResult.p2, Color.Red);
                    }

                }
            }

            
        }
    }
}