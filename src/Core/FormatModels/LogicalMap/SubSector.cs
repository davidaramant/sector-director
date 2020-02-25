// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;

namespace SectorDirector.Core.FormatModels.LogicalMap
{
    public sealed class SubSector : IEnumerable<Line>
    {
        private readonly List<Line> _lines  = new List<Line>();
        
        public int SectorIndex { get; }
        public LogicalSector ParentSector { get; }

        public SubSector(int sectorIndex, LogicalSector parent, IEnumerable<Line> lines)
        {
            SectorIndex = sectorIndex;
            ParentSector = parent;
            _lines.AddRange(lines);
        }

        public IEnumerator<Line> GetEnumerator() => _lines.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}