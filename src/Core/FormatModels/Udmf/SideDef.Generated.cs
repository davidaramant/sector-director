// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.0.0")]
    public sealed partial class SideDef : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _sectorHasBeenSet = false;
        private int _sector;
        public int Sector
        {
            get { return _sector; }
            set
            {
                _sectorHasBeenSet = true;
                _sector = value;
            }
        }
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public string TextureTop { get; set; } = "-";
        public string TextureBottom { get; set; } = "-";
        public string TextureMiddle { get; set; } = "-";
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public SideDef() { }
        public SideDef(
            int sector,
            int offsetX = 0,
            int offsetY = 0,
            string textureTop = "-",
            string textureBottom = "-",
            string textureMiddle = "-",
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            Sector = sector;
            OffsetX = offsetX;
            OffsetY = offsetY;
            TextureTop = textureTop;
            TextureBottom = textureBottom;
            TextureMiddle = textureMiddle;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteLine(stream, "sidedef");
            WriteLine(stream, "{");
            WriteProperty(stream, "sector", _sector, indent: true);
            if (OffsetX != 0) WriteProperty(stream, "offsetX", OffsetX, indent: true);
            if (OffsetY != 0) WriteProperty(stream, "offsetY", OffsetY, indent: true);
            if (TextureTop != "-") WriteProperty(stream, "textureTop", TextureTop, indent: true);
            if (TextureBottom != "-") WriteProperty(stream, "textureBottom", TextureBottom, indent: true);
            if (TextureMiddle != "-") WriteProperty(stream, "textureMiddle", TextureMiddle, indent: true);
            if (Comment != "") WriteProperty(stream, "comment", Comment, indent: true);
            foreach (var property in UnknownProperties)
            {
                WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: true);
            }
            WriteLine(stream, "}");
            return stream;
        }
        public void CheckSemanticValidity()
        {
            if (!_sectorHasBeenSet) throw new InvalidUdmfException("Did not set Sector on SideDef");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public SideDef Clone()
        {
            return new SideDef(
                sector: Sector,
                offsetX: OffsetX,
                offsetY: OffsetY,
                textureTop: TextureTop,
                textureBottom: TextureBottom,
                textureMiddle: TextureMiddle,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

}
