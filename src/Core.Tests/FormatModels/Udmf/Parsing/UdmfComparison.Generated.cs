// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;
using NUnit.Framework;

namespace SectorDirector.Core.Tests.FormatModels.Udmf.Parsing
{
    public static partial class UdmfComparison
    {
        public static void AssertEqual( LineDef actual, LineDef expected )
        {
            Assert.That(
                actual.Id,
                Is.EqualTo( expected.Id ),
                "Found difference in LineDef Id" );
            Assert.That(
                actual.V1,
                Is.EqualTo( expected.V1 ),
                "Found difference in LineDef V1" );
            Assert.That(
                actual.V2,
                Is.EqualTo( expected.V2 ),
                "Found difference in LineDef V2" );
            Assert.That(
                actual.Blocking,
                Is.EqualTo( expected.Blocking ),
                "Found difference in LineDef Blocking" );
            Assert.That(
                actual.BlockMonsters,
                Is.EqualTo( expected.BlockMonsters ),
                "Found difference in LineDef BlockMonsters" );
            Assert.That(
                actual.TwoSided,
                Is.EqualTo( expected.TwoSided ),
                "Found difference in LineDef TwoSided" );
            Assert.That(
                actual.DontPegTop,
                Is.EqualTo( expected.DontPegTop ),
                "Found difference in LineDef DontPegTop" );
            Assert.That(
                actual.DontPegBottom,
                Is.EqualTo( expected.DontPegBottom ),
                "Found difference in LineDef DontPegBottom" );
            Assert.That(
                actual.Secret,
                Is.EqualTo( expected.Secret ),
                "Found difference in LineDef Secret" );
            Assert.That(
                actual.BlockSound,
                Is.EqualTo( expected.BlockSound ),
                "Found difference in LineDef BlockSound" );
            Assert.That(
                actual.DontDraw,
                Is.EqualTo( expected.DontDraw ),
                "Found difference in LineDef DontDraw" );
            Assert.That(
                actual.Mapped,
                Is.EqualTo( expected.Mapped ),
                "Found difference in LineDef Mapped" );
            Assert.That(
                actual.Special,
                Is.EqualTo( expected.Special ),
                "Found difference in LineDef Special" );
            Assert.That(
                actual.Arg0,
                Is.EqualTo( expected.Arg0 ),
                "Found difference in LineDef Arg0" );
            Assert.That(
                actual.Arg1,
                Is.EqualTo( expected.Arg1 ),
                "Found difference in LineDef Arg1" );
            Assert.That(
                actual.Arg2,
                Is.EqualTo( expected.Arg2 ),
                "Found difference in LineDef Arg2" );
            Assert.That(
                actual.Arg3,
                Is.EqualTo( expected.Arg3 ),
                "Found difference in LineDef Arg3" );
            Assert.That(
                actual.Arg4,
                Is.EqualTo( expected.Arg4 ),
                "Found difference in LineDef Arg4" );
            Assert.That(
                actual.SideFront,
                Is.EqualTo( expected.SideFront ),
                "Found difference in LineDef SideFront" );
            Assert.That(
                actual.SideBack,
                Is.EqualTo( expected.SideBack ),
                "Found difference in LineDef SideBack" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in LineDef Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of LineDef UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( SideDef actual, SideDef expected )
        {
            Assert.That(
                actual.OffsetX,
                Is.EqualTo( expected.OffsetX ),
                "Found difference in SideDef OffsetX" );
            Assert.That(
                actual.OffsetY,
                Is.EqualTo( expected.OffsetY ),
                "Found difference in SideDef OffsetY" );
            Assert.That(
                actual.TextureTop,
                Is.EqualTo( expected.TextureTop ),
                "Found difference in SideDef TextureTop" );
            Assert.That(
                actual.TextureBottom,
                Is.EqualTo( expected.TextureBottom ),
                "Found difference in SideDef TextureBottom" );
            Assert.That(
                actual.TextureMiddle,
                Is.EqualTo( expected.TextureMiddle ),
                "Found difference in SideDef TextureMiddle" );
            Assert.That(
                actual.Sector,
                Is.EqualTo( expected.Sector ),
                "Found difference in SideDef Sector" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in SideDef Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of SideDef UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Vertex actual, Vertex expected )
        {
            Assert.That(
                actual.X,
                Is.EqualTo( expected.X ),
                "Found difference in Vertex X" );
            Assert.That(
                actual.Y,
                Is.EqualTo( expected.Y ),
                "Found difference in Vertex Y" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Vertex Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Vertex UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Sector actual, Sector expected )
        {
            Assert.That(
                actual.HeightFloor,
                Is.EqualTo( expected.HeightFloor ),
                "Found difference in Sector HeightFloor" );
            Assert.That(
                actual.HeightCeiling,
                Is.EqualTo( expected.HeightCeiling ),
                "Found difference in Sector HeightCeiling" );
            Assert.That(
                actual.TextureFloor,
                Is.EqualTo( expected.TextureFloor ),
                "Found difference in Sector TextureFloor" );
            Assert.That(
                actual.TextureCeiling,
                Is.EqualTo( expected.TextureCeiling ),
                "Found difference in Sector TextureCeiling" );
            Assert.That(
                actual.LightLevel,
                Is.EqualTo( expected.LightLevel ),
                "Found difference in Sector LightLevel" );
            Assert.That(
                actual.Special,
                Is.EqualTo( expected.Special ),
                "Found difference in Sector Special" );
            Assert.That(
                actual.Id,
                Is.EqualTo( expected.Id ),
                "Found difference in Sector Id" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Sector Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Sector UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Thing actual, Thing expected )
        {
            Assert.That(
                actual.Id,
                Is.EqualTo( expected.Id ),
                "Found difference in Thing Id" );
            Assert.That(
                actual.X,
                Is.EqualTo( expected.X ),
                "Found difference in Thing X" );
            Assert.That(
                actual.Y,
                Is.EqualTo( expected.Y ),
                "Found difference in Thing Y" );
            Assert.That(
                actual.Height,
                Is.EqualTo( expected.Height ),
                "Found difference in Thing Height" );
            Assert.That(
                actual.Angle,
                Is.EqualTo( expected.Angle ),
                "Found difference in Thing Angle" );
            Assert.That(
                actual.Type,
                Is.EqualTo( expected.Type ),
                "Found difference in Thing Type" );
            Assert.That(
                actual.Skill1,
                Is.EqualTo( expected.Skill1 ),
                "Found difference in Thing Skill1" );
            Assert.That(
                actual.Skill2,
                Is.EqualTo( expected.Skill2 ),
                "Found difference in Thing Skill2" );
            Assert.That(
                actual.Skill3,
                Is.EqualTo( expected.Skill3 ),
                "Found difference in Thing Skill3" );
            Assert.That(
                actual.Skill4,
                Is.EqualTo( expected.Skill4 ),
                "Found difference in Thing Skill4" );
            Assert.That(
                actual.Skill5,
                Is.EqualTo( expected.Skill5 ),
                "Found difference in Thing Skill5" );
            Assert.That(
                actual.Ambush,
                Is.EqualTo( expected.Ambush ),
                "Found difference in Thing Ambush" );
            Assert.That(
                actual.Single,
                Is.EqualTo( expected.Single ),
                "Found difference in Thing Single" );
            Assert.That(
                actual.Dm,
                Is.EqualTo( expected.Dm ),
                "Found difference in Thing Dm" );
            Assert.That(
                actual.Coop,
                Is.EqualTo( expected.Coop ),
                "Found difference in Thing Coop" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Thing Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Thing UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( MapData actual, MapData expected )
        {
            Assert.That(
                actual.NameSpace,
                Is.EqualTo( expected.NameSpace ),
                "Found difference in MapData NameSpace" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in MapData Comment" );
            Assert.That(
                actual.LineDefs.Count,
                Is.EqualTo( expected.LineDefs.Count ),
                "Found unequal number of MapData LineDefs" );
            for( int i = 0; i < expected.LineDefs.Count; i++ )
            {
                AssertEqual( 
                    actual.LineDefs[i],
                    expected.LineDefs[i] );
            }
            Assert.That(
                actual.SideDefs.Count,
                Is.EqualTo( expected.SideDefs.Count ),
                "Found unequal number of MapData SideDefs" );
            for( int i = 0; i < expected.SideDefs.Count; i++ )
            {
                AssertEqual( 
                    actual.SideDefs[i],
                    expected.SideDefs[i] );
            }
            Assert.That(
                actual.Vertices.Count,
                Is.EqualTo( expected.Vertices.Count ),
                "Found unequal number of MapData Vertices" );
            for( int i = 0; i < expected.Vertices.Count; i++ )
            {
                AssertEqual( 
                    actual.Vertices[i],
                    expected.Vertices[i] );
            }
            Assert.That(
                actual.Sectors.Count,
                Is.EqualTo( expected.Sectors.Count ),
                "Found unequal number of MapData Sectors" );
            for( int i = 0; i < expected.Sectors.Count; i++ )
            {
                AssertEqual( 
                    actual.Sectors[i],
                    expected.Sectors[i] );
            }
            Assert.That(
                actual.Things.Count,
                Is.EqualTo( expected.Things.Count ),
                "Found unequal number of MapData Things" );
            for( int i = 0; i < expected.Things.Count; i++ )
            {
                AssertEqual( 
                    actual.Things[i],
                    expected.Things[i] );
            }
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of MapData UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
            Assert.That(
                actual.UnknownBlocks.Count,
                Is.EqualTo( expected.UnknownBlocks.Count ),
                "Found unequal number of MapData UnknownBlocks" );
            for( int i = 0; i < expected.UnknownBlocks.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownBlocks[i],
                    expected.UnknownBlocks[i] );
            }
        }
    }
}