﻿// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.LogicalMap
{
    public sealed class Line
    {
        public Vertex Start { get; }
        public Vertex End { get; }
        public SideDef Side { get; }
        public bool IsFrontSide { get; }
        public LineDef Definition { get; }

        public Line(Vertex start, Vertex end, SideDef side, bool isFrontSide, LineDef definition)
        {
            Start = start;
            End = end;
            Side = side;
            IsFrontSide = isFrontSide;
            Definition = definition;
        }
    }
}