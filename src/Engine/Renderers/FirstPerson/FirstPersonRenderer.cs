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
        delegate void DrawLine(ScreenBuffer buffer, Point p0, Point p1, Color c);

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

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();
            Interpreter = new FirstPersonWorldInterpreter(player.CameraSettings);

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
                        _drawLine(screen, topLineResult.p1, topLineResult.p2, Color.Red);
                    }

                    var bottomLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, bottomLeft, bottomRight);
                    if (bottomLineResult.shouldDraw)
                    {
                        _drawLine(screen, bottomLineResult.p1, bottomLineResult.p2, Color.Red);
                    }

                    var leftLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, topLeft, bottomLeft);
                    if (leftLineResult.shouldDraw)
                    {
                        _drawLine(screen, leftLineResult.p1, leftLineResult.p2, Color.Red);
                    }

                    var rightLineResult = Interpreter.ConvertWorldLineToScreenPoints(screen, cameraPosition, player.Direction, topRight, bottomRight);
                    if (rightLineResult.shouldDraw)
                    {
                        _drawLine(screen, rightLineResult.p1, rightLineResult.p2, Color.Red);
                    }

                }
            }

            
        }
    }
}