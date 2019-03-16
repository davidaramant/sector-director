// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SectorDirector.DataModelGenerator.PropertyTypes;

namespace SectorDirector.DataModelGenerator
{
    [DebuggerDisplay("{" + nameof(CodeName) + "}")]
    public sealed class Block : NamedItem
    {
        private readonly List<IProperty> _properties = new List<IProperty>();

        public IEnumerable<IProperty> Properties => _properties;
        public IEnumerable<Field> Fields => _properties.OfType<Field>();
        public IEnumerable<BlockList> SubBlocks => _properties.OfType<BlockList>();
        public bool IsSubBlock { get; }

        public IEnumerable<IProperty> OrderedProperties() => 
            Properties.Where(p => p.IsRequired).
            Concat(Properties.Where(p => !p.IsRequired));

        public Block(
            string formatName,
            IEnumerable<IProperty> properties,
            string className = null,
            bool isSubBlock = true) :
            base(formatName, className ?? formatName)
        {
            IsSubBlock = isSubBlock;
            _properties.AddRange(properties);
        }
    }
}