// Copyright (c) 2016, David Aramant
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SectorDirector.DataModelGenerator
{
    [DebuggerDisplay("{" + nameof(CodeName) + "}")]
    public sealed class Block : NamedItem
    {
        private readonly List<Property> _properties = new List<Property>();

        public IEnumerable<Property> Properties => _properties;
        public bool IsSubBlock { get; }

        public IEnumerable<Property> OrderedProperties() => 
            Properties.Where(p => p.IsRequired).
            Concat(Properties.Where(p => !p.IsRequired));

        public Block(
            string formatName,
            IEnumerable<Property> properties,
            string className = null,
            bool isSubBlock = true) :
            base(formatName, className ?? formatName)
        {
            IsSubBlock = isSubBlock;
            _properties.AddRange(properties);
        }
    }
}