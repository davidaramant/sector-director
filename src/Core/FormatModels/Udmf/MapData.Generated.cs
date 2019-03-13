// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.0.0")]
    public sealed partial class MapData : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _nameSpaceHasBeenSet = false;
        private string _nameSpace;
        public string NameSpace
        {
            get { return _nameSpace; }
            set
            {
                _nameSpaceHasBeenSet = true;
                _nameSpace = value;
            }
        }
        public string Comment { get; set; } = "";
        public List<LineDef> LineDefs { get; } = new List<LineDef>();
        public List<SideDef> SideDefs { get; } = new List<SideDef>();
        public List<Vertex> Vertices { get; } = new List<Vertex>();
        public List<Sector> Sectors { get; } = new List<Sector>();
        public List<Thing> Things { get; } = new List<Thing>();
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();
        public MapData() { }
        public MapData(
            string nameSpace,
            IEnumerable<LineDef> lineDefs,
            IEnumerable<SideDef> sideDefs,
            IEnumerable<Vertex> vertices,
            IEnumerable<Sector> sectors,
            IEnumerable<Thing> things,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null,
            IEnumerable<UnknownBlock> unknownBlocks = null)
        {
            NameSpace = nameSpace;
            LineDefs.AddRange(lineDefs);
            SideDefs.AddRange(sideDefs);
            Vertices.AddRange(vertices);
            Sectors.AddRange(sectors);
            Things.AddRange(things);
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            UnknownBlocks.AddRange(unknownBlocks ?? Enumerable.Empty<UnknownBlock>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteProperty(stream, "namespace", _nameSpace, indent: false);
            if (Comment != "") WriteProperty(stream, "comment", Comment, indent: false);
            foreach (var property in UnknownProperties)
            {
                WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: false);
            }
            WriteBlocks(stream, LineDefs );
            WriteBlocks(stream, SideDefs );
            WriteBlocks(stream, Vertices );
            WriteBlocks(stream, Sectors );
            WriteBlocks(stream, Things );
            WriteBlocks(stream, UnknownBlocks );
            return stream;
        }
        public void CheckSemanticValidity()
        {
            if (!_nameSpaceHasBeenSet) throw new InvalidUdmfException("Did not set NameSpace on MapData");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public MapData Clone()
        {
            return new MapData(
                nameSpace: NameSpace,
                lineDefs: LineDefs.Select(item => item.Clone()),
                sideDefs: SideDefs.Select(item => item.Clone()),
                vertices: Vertices.Select(item => item.Clone()),
                sectors: Sectors.Select(item => item.Clone()),
                things: Things.Select(item => item.Clone()),
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()),
                unknownBlocks: UnknownBlocks.Select(item => item.Clone()));
        }
    }

}
