// Copyright (c) 2018, Aaron Alexander and Matt Moseng
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.MapGenerator.Data
{
    public sealed class Map
    {
        public List<Layer> Layers { get; } = new List<Layer>();
        public List<Shape> OuterShapes { get; } = new List<Shape>();
        public IEnumerable<Vertex> Vertices=> Layers.SelectMany(layer => layer.Vertices);
    }
}