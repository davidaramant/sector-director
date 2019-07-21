// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using SectorDirector.Core.BinaryReaderExtensions;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.BinaryDoom
{
    public static class LumpEntryParser
    {
        public static Vertex Vertex(BinaryReader reader)
        {
            return new Vertex(
                x: reader.ReadInt16(),
                y: reader.ReadInt16());
        }

        [Flags]
        private enum LineDefFlags : short
        {
            Nothing = 0,
            BlockPlayersAndMonsters = 1 << 0,
            BlockMonsters = 1 << 1,
            TwoSided = 1 << 2,
            UpperTextureUnpegged = 1 << 3,
            LowerTextureUnpegged = 1 << 4,
            Secret = 1 << 5,
            BlocksSound = 1 << 6,
            NeverShowOnAutomap = 1 << 7,
            AlwaysShowOnAutomap = 1 << 8,
        }

        public static LineDef LineDef(BinaryReader reader)
        {
            var ld = new LineDef
            {
                V1 = reader.ReadInt16(),
                V2 = reader.ReadInt16()
            };

            var flags = (LineDefFlags)reader.ReadInt16();

            ld.Blocking = flags.HasFlag(LineDefFlags.BlockPlayersAndMonsters);
            ld.BlockMonsters = flags.HasFlag(LineDefFlags.BlockMonsters);
            ld.TwoSided = flags.HasFlag(LineDefFlags.TwoSided);
            ld.DontPegTop = flags.HasFlag(LineDefFlags.UpperTextureUnpegged);
            ld.DontPegBottom = flags.HasFlag(LineDefFlags.LowerTextureUnpegged);
            ld.Secret = flags.HasFlag(LineDefFlags.Secret);
            ld.BlockSound = flags.HasFlag(LineDefFlags.BlocksSound);
            ld.DontDraw = flags.HasFlag(LineDefFlags.NeverShowOnAutomap);
            ld.Mapped = flags.HasFlag(LineDefFlags.AlwaysShowOnAutomap);

            ld.Special = reader.ReadInt16();
            ld.Id = reader.ReadInt16();
            ld.SideFront = reader.ReadInt16();
            ld.SideBack = reader.ReadInt16();

            return ld;
        }

        public static SideDef SideDef(BinaryReader reader)
        {
            return new SideDef
            {
                OffsetX = reader.ReadInt16(),
                OffsetY = reader.ReadInt16(),
                TextureTop = reader.ReadText(8),
                TextureBottom = reader.ReadText(8),
                TextureMiddle = reader.ReadText(8),
                Sector = reader.ReadInt16()
            };
        }

        public static Sector Sector(BinaryReader reader)
        {
            return new Sector(
                heightFloor: reader.ReadInt16(),
                heightCeiling: reader.ReadInt16(),
                textureFloor: reader.ReadText(8),
                textureCeiling: reader.ReadText(8),
                lightLevel: reader.ReadInt16(),
                special: reader.ReadInt16(),
                id: reader.ReadInt16());
        }

        [Flags]
        private enum ThingFlags : short
        {
            None = 0,
            SkillLevels1And2 = 1 << 0,
            SkillLevel3 = 1 << 1,
            SkillLevel4And5 = 1 << 2,
            Ambush = 1 << 3,
            DeathMatchOnly = 1 << 4,
        }

        public static Thing Thing(BinaryReader reader)
        {
            var t = new Thing(
                x: reader.ReadInt16(),
                y: reader.ReadInt16(),
                angle: reader.ReadInt16(),
                type: reader.ReadInt16());

            var flags = (ThingFlags)reader.ReadInt16();

            t.Skill1 = t.Skill2 = flags.HasFlag(ThingFlags.SkillLevels1And2);
            t.Skill3 = flags.HasFlag(ThingFlags.SkillLevel3);
            t.Skill4 = t.Skill5 = flags.HasFlag(ThingFlags.SkillLevel4And5);
            t.Ambush = flags.HasFlag(ThingFlags.Ambush);
            t.Single = t.Coop = !flags.HasFlag(ThingFlags.DeathMatchOnly);
            t.Dm = true;

            return t;
        }
    }
}