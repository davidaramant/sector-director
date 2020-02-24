// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.LogicalMap;
using SectorDirector.Core.FormatModels.Udmf;
using System.IO;
using System.Linq;
using System.Text;

namespace SectorDirector.Core.FormatModels.Svg
{
    public static class SvgExporter
    {
        public static void Export(MapData mapData, string filePath)
        {
            using var outputStream = new FileStream(filePath, FileMode.Create);
            using var w = new StreamWriter(outputStream, Encoding.UTF8);

            var sectorGraph = SectorGraph.BuildFrom(mapData);

            const int Padding = 10;

            var height = mapData.Height;
            var minY = mapData.MinY;

            double FlipY(double y) => height - (y - minY);

            w.WriteLine($"<svg viewBox=\"{mapData.MinX - Padding} {FlipY(mapData.MaxY) - Padding} {mapData.Width + 2 * Padding} {mapData.Height + 2 * Padding}\" xmlns=\"http://www.w3.org/2000/svg\">");

            w.WriteLine($"<rect x=\"{mapData.MinX - Padding}\" y=\"{FlipY(mapData.MaxY) - Padding}\" width=\"{mapData.Width + 2 * Padding}\" height=\"{mapData.Height + 2 * Padding}\" fill=\"black\"/>");

            foreach (var sectorIndex in Enumerable.Range(0, mapData.Sectors.Count))
            {
                w.WriteLine($"<g id=\"{sectorIndex}\">");

                foreach (var subSector in sectorGraph.SubSectors.Where(ss => ss.SectorIndex == sectorIndex))
                {
                    var pointString = string.Join(" ", subSector.Lines.Select(line =>
                        {
                            var v = line.Start;
                            return $"{v.X},{FlipY(v.Y)}";
                        }));

                    w.WriteLine($"<polygon points=\"{pointString}\" stroke=\"none\" fill=\"#181818\"/>");

                    foreach (var line in subSector.Lines)
                    {
                        w.WriteLine($"<line x1=\"{line.Start.X}\" y1=\"{FlipY(line.Start.Y)}\" x2=\"{line.End.X}\" y2=\"{FlipY(line.End.Y)}\" stroke=\"{(line.Definition.TwoSided ? "yellow" : "red")}\"/>");
                    }
                }



                w.WriteLine("</g>");
            }

            w.WriteLine("</svg>");
        }
    }
}