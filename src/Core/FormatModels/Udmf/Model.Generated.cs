// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
    public sealed partial class LineDef : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _v1HasBeenSet = false;
        private int _v1;
        public int V1
        {
            get { return _v1; }
            set
            {
                _v1HasBeenSet = true;
                _v1 = value;
            }
        }
        private bool _v2HasBeenSet = false;
        private int _v2;
        public int V2
        {
            get { return _v2; }
            set
            {
                _v2HasBeenSet = true;
                _v2 = value;
            }
        }
        private bool _sideFrontHasBeenSet = false;
        private int _sideFront;
        public int SideFront
        {
            get { return _sideFront; }
            set
            {
                _sideFrontHasBeenSet = true;
                _sideFront = value;
            }
        }
        public int Id { get; set; } = -1;
        public bool Blocking { get; set; } = false;
        public bool BlockMonsters { get; set; } = false;
        public bool TwoSided { get; set; } = false;
        public bool DontPegTop { get; set; } = false;
        public bool DontPegBottom { get; set; } = false;
        public bool Secret { get; set; } = false;
        public bool BlockSound { get; set; } = false;
        public bool DontDraw { get; set; } = false;
        public bool Mapped { get; set; } = false;
        public int Special { get; set; } = 0;
        public int Arg0 { get; set; } = 0;
        public int Arg1 { get; set; } = 0;
        public int Arg2 { get; set; } = 0;
        public int Arg3 { get; set; } = 0;
        public int Arg4 { get; set; } = 0;
        public int SideBack { get; set; } = -1;
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public LineDef() { }
        public LineDef(
            int v1,
            int v2,
            int sideFront,
            int id = -1,
            bool blocking = false,
            bool blockMonsters = false,
            bool twoSided = false,
            bool dontPegTop = false,
            bool dontPegBottom = false,
            bool secret = false,
            bool blockSound = false,
            bool dontDraw = false,
            bool mapped = false,
            int special = 0,
            int arg0 = 0,
            int arg1 = 0,
            int arg2 = 0,
            int arg3 = 0,
            int arg4 = 0,
            int sideBack = -1,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            V1 = v1;
            V2 = v2;
            SideFront = sideFront;
            Id = id;
            Blocking = blocking;
            BlockMonsters = blockMonsters;
            TwoSided = twoSided;
            DontPegTop = dontPegTop;
            DontPegBottom = dontPegBottom;
            Secret = secret;
            BlockSound = blockSound;
            DontDraw = dontDraw;
            Mapped = mapped;
            Special = special;
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
            SideBack = sideBack;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteLine(stream, "linedef");
            WriteLine(stream, "{");
            WriteProperty(stream, "v1", _v1, indent: true);
            WriteProperty(stream, "v2", _v2, indent: true);
            WriteProperty(stream, "sideFront", _sideFront, indent: true);
            if (Id != -1) WriteProperty(stream, "id", Id, indent: true);
            if (Blocking != false) WriteProperty(stream, "blocking", Blocking, indent: true);
            if (BlockMonsters != false) WriteProperty(stream, "blockMonsters", BlockMonsters, indent: true);
            if (TwoSided != false) WriteProperty(stream, "twoSided", TwoSided, indent: true);
            if (DontPegTop != false) WriteProperty(stream, "dontPegTop", DontPegTop, indent: true);
            if (DontPegBottom != false) WriteProperty(stream, "dontPegBottom", DontPegBottom, indent: true);
            if (Secret != false) WriteProperty(stream, "secret", Secret, indent: true);
            if (BlockSound != false) WriteProperty(stream, "blockSound", BlockSound, indent: true);
            if (DontDraw != false) WriteProperty(stream, "dontDraw", DontDraw, indent: true);
            if (Mapped != false) WriteProperty(stream, "mapped", Mapped, indent: true);
            if (Special != 0) WriteProperty(stream, "special", Special, indent: true);
            if (Arg0 != 0) WriteProperty(stream, "arg0", Arg0, indent: true);
            if (Arg1 != 0) WriteProperty(stream, "arg1", Arg1, indent: true);
            if (Arg2 != 0) WriteProperty(stream, "arg2", Arg2, indent: true);
            if (Arg3 != 0) WriteProperty(stream, "arg3", Arg3, indent: true);
            if (Arg4 != 0) WriteProperty(stream, "arg4", Arg4, indent: true);
            if (SideBack != -1) WriteProperty(stream, "sideBack", SideBack, indent: true);
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
            if (!_v1HasBeenSet) throw new InvalidUdmfException("Did not set V1 on LineDef");
            if (!_v2HasBeenSet) throw new InvalidUdmfException("Did not set V2 on LineDef");
            if (!_sideFrontHasBeenSet) throw new InvalidUdmfException("Did not set SideFront on LineDef");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public LineDef Clone()
        {
            return new LineDef(
                v1: V1,
                v2: V2,
                sideFront: SideFront,
                id: Id,
                blocking: Blocking,
                blockMonsters: BlockMonsters,
                twoSided: TwoSided,
                dontPegTop: DontPegTop,
                dontPegBottom: DontPegBottom,
                secret: Secret,
                blockSound: BlockSound,
                dontDraw: DontDraw,
                mapped: Mapped,
                special: Special,
                arg0: Arg0,
                arg1: Arg1,
                arg2: Arg2,
                arg3: Arg3,
                arg4: Arg4,
                sideBack: SideBack,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
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

    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
    public sealed partial class Vertex : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _xHasBeenSet = false;
        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _xHasBeenSet = true;
                _x = value;
            }
        }
        private bool _yHasBeenSet = false;
        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                _yHasBeenSet = true;
                _y = value;
            }
        }
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public Vertex() { }
        public Vertex(
            double x,
            double y,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            X = x;
            Y = y;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteLine(stream, "vertex");
            WriteLine(stream, "{");
            WriteProperty(stream, "x", _x, indent: true);
            WriteProperty(stream, "y", _y, indent: true);
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
            if (!_xHasBeenSet) throw new InvalidUdmfException("Did not set X on Vertex");
            if (!_yHasBeenSet) throw new InvalidUdmfException("Did not set Y on Vertex");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public Vertex Clone()
        {
            return new Vertex(
                x: X,
                y: Y,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
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

    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
    public sealed partial class Thing : BaseUdmfBlock, IWriteableUdmfBlock
    {
        private bool _xHasBeenSet = false;
        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _xHasBeenSet = true;
                _x = value;
            }
        }
        private bool _yHasBeenSet = false;
        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                _yHasBeenSet = true;
                _y = value;
            }
        }
        private bool _typeHasBeenSet = false;
        private int _type;
        public int Type
        {
            get { return _type; }
            set
            {
                _typeHasBeenSet = true;
                _type = value;
            }
        }
        public int Id { get; set; } = 0;
        public double Height { get; set; } = 0;
        public int Angle { get; set; } = 0;
        public bool Skill1 { get; set; } = false;
        public bool Skill2 { get; set; } = false;
        public bool Skill3 { get; set; } = false;
        public bool Skill4 { get; set; } = false;
        public bool Skill5 { get; set; } = false;
        public bool Ambush { get; set; } = false;
        public bool Single { get; set; } = false;
        public bool Dm { get; set; } = false;
        public bool Coop { get; set; } = false;
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public Thing() { }
        public Thing(
            double x,
            double y,
            int type,
            int id = 0,
            double height = 0,
            int angle = 0,
            bool skill1 = false,
            bool skill2 = false,
            bool skill3 = false,
            bool skill4 = false,
            bool skill5 = false,
            bool ambush = false,
            bool single = false,
            bool dm = false,
            bool coop = false,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            X = x;
            Y = y;
            Type = type;
            Id = id;
            Height = height;
            Angle = angle;
            Skill1 = skill1;
            Skill2 = skill2;
            Skill3 = skill3;
            Skill4 = skill4;
            Skill5 = skill5;
            Ambush = ambush;
            Single = single;
            Dm = dm;
            Coop = coop;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            WriteLine(stream, "thing");
            WriteLine(stream, "{");
            WriteProperty(stream, "x", _x, indent: true);
            WriteProperty(stream, "y", _y, indent: true);
            WriteProperty(stream, "type", _type, indent: true);
            if (Id != 0) WriteProperty(stream, "id", Id, indent: true);
            if (Height != 0) WriteProperty(stream, "height", Height, indent: true);
            if (Angle != 0) WriteProperty(stream, "angle", Angle, indent: true);
            if (Skill1 != false) WriteProperty(stream, "skill1", Skill1, indent: true);
            if (Skill2 != false) WriteProperty(stream, "skill2", Skill2, indent: true);
            if (Skill3 != false) WriteProperty(stream, "skill3", Skill3, indent: true);
            if (Skill4 != false) WriteProperty(stream, "skill4", Skill4, indent: true);
            if (Skill5 != false) WriteProperty(stream, "skill5", Skill5, indent: true);
            if (Ambush != false) WriteProperty(stream, "ambush", Ambush, indent: true);
            if (Single != false) WriteProperty(stream, "single", Single, indent: true);
            if (Dm != false) WriteProperty(stream, "dm", Dm, indent: true);
            if (Coop != false) WriteProperty(stream, "coop", Coop, indent: true);
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
            if (!_xHasBeenSet) throw new InvalidUdmfException("Did not set X on Thing");
            if (!_yHasBeenSet) throw new InvalidUdmfException("Did not set Y on Thing");
            if (!_typeHasBeenSet) throw new InvalidUdmfException("Did not set Type on Thing");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public Thing Clone()
        {
            return new Thing(
                x: X,
                y: Y,
                type: Type,
                id: Id,
                height: Height,
                angle: Angle,
                skill1: Skill1,
                skill2: Skill2,
                skill3: Skill3,
                skill4: Skill4,
                skill5: Skill5,
                ambush: Ambush,
                single: Single,
                dm: Dm,
                coop: Coop,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22564")]
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
