// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.MapGenerator.Data
{
    public sealed class Layer
    {
        public List<Shape> Shapes { get; } = new List<Shape>();

        public int Height { get; }

        public int LayerNumber { get; }

        public IEnumerable<Vertex> Vertices=> Shapes.SelectMany(shape => shape.Vertices);

        public Layer(IEnumerable<Shape> shapes, int height, int layerNumber)
        {
            Shapes.AddRange(shapes);
            Height = height;
            LayerNumber = layerNumber;
        }
    }
}