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
        
        public OverheadRenderer(MapData map)
        {
            _map = map;
        }

        public void Render(ScreenBuffer buffer)
        {
            var verticesAsPoints = _map.Vertices.Select(v => v.ToPoint()).ToArray();

            var minX = verticesAsPoints.Min(p => p.X);
            var maxX = verticesAsPoints.Max(p => p.X);
            var minY = verticesAsPoints.Min(p => p.Y);
            var maxY = verticesAsPoints.Max(p => p.Y);

            var mapBounds = new Rectangle(x: minX, y: minY, width: maxX - minY, height: maxY - minY);

            const float desiredFit = 0.9f;

            var xScale = (float)mapBounds.Width / buffer.Width;
            var yScale = (float)mapBounds.Height / buffer.Height;

            var largestScale = MathHelper.Max(xScale, yScale);

            var scaleFactor = desiredFit / largestScale;

            var scaledMapWidth = (int)(scaleFactor * mapBounds.Width);
            var scaledMapHeight = (int)(scaleFactor * mapBounds.Height);

            var offset = new Point(
                x: (buffer.Width - scaledMapWidth) / 2,
                y: (buffer.Height - scaledMapHeight) / 2);

            Point ConvertToScreenCoords(Point gameCoordinate) =>
                (offset + gameCoordinate.Scale(scaleFactor)).InvertY(buffer.Height);

            foreach (var lineDef in _map.LineDefs)
            {
                var vertex1 = verticesAsPoints[lineDef.V1];
                var vertex2 = verticesAsPoints[lineDef.V2];

                var color = lineDef.TwoSided ? Color.Gray : Color.White;

                var p1 = ConvertToScreenCoords(vertex1);
                var p2 = ConvertToScreenCoords(vertex2);
                buffer.PlotLine(p1, p2, color);
            }
        }
    }
}