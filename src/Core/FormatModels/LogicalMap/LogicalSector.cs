// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;
using System.Collections;
using System.Collections.Generic;

namespace SectorDirector.Core.FormatModels.LogicalMap
{
    public sealed class LogicalSector : IEnumerable<SubSector>
    {
        public int SectorId { get; }
        public Sector ActualSector { get; }

        private readonly List<SubSector> _subSectors = new List<SubSector>();

        public LogicalSector(int sectorId, Sector actualSector)
        {
            SectorId = sectorId;
            ActualSector = actualSector;
        }

        public void Add(SubSector subSector) => _subSectors.Add(subSector);

        public IEnumerator<SubSector> GetEnumerator() => _subSectors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
