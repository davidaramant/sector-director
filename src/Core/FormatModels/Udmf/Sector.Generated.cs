// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22712")]
    public sealed partial class Sector : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _textureFloorHasBeenSet = false;
        private string _textureFloor;
        public string TextureFloor
        {
            get { return _textureFloor; }
            set
            {
                _textureFloorHasBeenSet = true;
                _textureFloor = value;
            }
        }
        private bool _textureCeilingHasBeenSet = false;
        private string _textureCeiling;
        public string TextureCeiling
        {
            get { return _textureCeiling; }
            set
            {
                _textureCeilingHasBeenSet = true;
                _textureCeiling = value;
            }
        }
        public int HeightFloor { get; set; } = 0;
        public int HeightCeiling { get; set; } = 0;
        public int LightLevel { get; set; } = 160;
        public int Special { get; set; } = 0;
        public int Id { get; set; } = 0;
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public Sector() { }
        public Sector(
            string textureFloor,
            string textureCeiling,
            int heightFloor = 0,
            int heightCeiling = 0,
            int lightLevel = 160,
            int special = 0,
            int id = 0,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            TextureFloor = textureFloor;
            TextureCeiling = textureCeiling;
            HeightFloor = heightFloor;
            HeightCeiling = heightCeiling;
            LightLevel = lightLevel;
            Special = special;
            Id = id;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteLine(stream, "sector");
            WriteLine(stream, "{");
            WriteProperty(stream, "textureFloor", _textureFloor, indent: true);
            WriteProperty(stream, "textureCeiling", _textureCeiling, indent: true);
            if (HeightFloor != 0) WriteProperty(stream, "heightFloor", HeightFloor, indent: true);
            if (HeightCeiling != 0) WriteProperty(stream, "heightCeiling", HeightCeiling, indent: true);
            if (LightLevel != 160) WriteProperty(stream, "lightLevel", LightLevel, indent: true);
            if (Special != 0) WriteProperty(stream, "special", Special, indent: true);
            if (Id != 0) WriteProperty(stream, "id", Id, indent: true);
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
            if (!_textureFloorHasBeenSet) throw new InvalidUdmfException("Did not set TextureFloor on Sector");
            if (!_textureCeilingHasBeenSet) throw new InvalidUdmfException("Did not set TextureCeiling on Sector");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public Sector Clone()
        {
            return new Sector(
                textureFloor: TextureFloor,
                textureCeiling: TextureCeiling,
                heightFloor: HeightFloor,
                heightCeiling: HeightCeiling,
                lightLevel: LightLevel,
                special: Special,
                id: Id,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

}
