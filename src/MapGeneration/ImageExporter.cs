//// Copyright (c) 2018, Aaron Alexander and Matt Moseng
//// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using SectorDirector.MapGeneration.Data;

//namespace SectorDirector.MapGeneration
//{
//    public static class ImageExporter
//    {
//        public static void CreateImage(Map map, string path, bool boundaryMode = false)
//        {
//            using (var streamWriter = new StreamWriter(path))
//            {
//                streamWriter.Write(FormatSvg(map, boundaryMode));
//            }
//        }

//        private static string FormatSvg(Map map, bool boundaryMode)
//        {
//            var builder = new StringBuilder();
//            builder.AppendLine("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">");

//            var maximumY = map.BoundingShape.Polygon.Select(polygon => polygon.Y).Max();

//            if (boundaryMode)
//            {
//                AddPath(builder, map.BoundingShape, true, maximumY);
//            }

//            foreach (var layer in map.Layers)
//            {
//                foreach (var shape in layer.Shapes)
//                {
//                    AddPath(builder, shape, false, maximumY);
//                }
//            }

//            builder.AppendLine("</svg>");

//            return builder.ToString();
//        }

//        private static void AddPath(StringBuilder builder, Shape shape, bool useBackgroundOpacity, long maximumY)
//        {
//            builder.Append("<path d=\"");

//            for (var i = 0; i < shape.Polygon.Count; i++)
//            {
//                var point = shape.Polygon[i];
//                builder.Append(i == 0 ? " M" : " L");
//                builder.AppendFormat(" {0}.00 {1}.00", point.X, maximumY - point.Y);
//            }

//            builder.Append(" z\"");
//            builder.AppendFormat(
//                " style=\"fill:{0}; fill-opacity:{1}; fill-rule:nonzero; stroke:#D3D3DA; stroke-opacity:1.00; stroke-width:0.80;\"",
//                RandomColor(),
//                useBackgroundOpacity ? "0.06" : "0.1");
//            builder.AppendLine("/>");

//        }

//        private static readonly Random ColorRandomizer = new Random();

//        private static string RandomColor()
//        {
//            var color = $"#{ColorRandomizer.Next(0x1000000):X6}";
//            return color;
//        }
//    }
//}
