// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.7008.22712")]
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

}
