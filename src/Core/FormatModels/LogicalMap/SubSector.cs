// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.LogicalMap
{
    public sealed class SubSector
    {
        public List<Line> Lines { get; } = new List<Line>();
        
        public int SectorIndex { get; }
        public Sector ParentSector { get; }

        public SubSector(int sectorIndex, Sector parent, IEnumerable<Line> lines)
        {
            SectorIndex = sectorIndex;
            ParentSector = parent;
            Lines.AddRange(lines);
        }
    }
}