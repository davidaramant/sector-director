// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.Core.FormatModels.Udmf;

using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;

namespace SectorDirector.MapGenerator.Data
{
    public class Shape
    {
        public Polygon Polygon { get; set; }

        public IEnumerable<Vertex> Vertices
        {
            get
            {
                foreach (var point in Polygon)
                {
                    yield return new Vertex(point.X, point.Y);
                }
            }
        }

        public Shape(Polygon polygon)
        {
            Polygon = polygon;
        }
    }
}