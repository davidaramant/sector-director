// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.MapGenerator.Data
{
    public class Layer
    {
        public List<Shape> Shapes { get; set; } = new List<Shape>();

        public int Depth { get; set; }

        public IEnumerable<Vertex> Vertices
        {
            get
            {
                return Shapes.SelectMany(shape => shape.Vertices);
            }
        }
    }
}