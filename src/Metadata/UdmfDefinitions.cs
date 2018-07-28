// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace SectorDirector.Metadata
{
    public static class UdmfDefinitions
    {
        public static readonly IEnumerable<Block> Blocks = new Block[]
        {
            new Block("lineDef",
                properties:new []
                {
                    new Property("id",PropertyType.Integer, defaultValue:-1),
                    
                    new Property("v1",PropertyType.Integer),
                    new Property("v2",PropertyType.Integer),

                    new Property("blocking",PropertyType.Boolean, defaultValue:false),
                    new Property("blockMonsters",PropertyType.Boolean, defaultValue:false),
                    new Property("twoSided",PropertyType.Boolean, defaultValue:false),
                    new Property("dontPegTop",PropertyType.Boolean, defaultValue:false),
                    new Property("dontPegBottom",PropertyType.Boolean, defaultValue:false),
                    new Property("secret",PropertyType.Boolean, defaultValue:false),
                    new Property("blockSound",PropertyType.Boolean, defaultValue:false),
                    new Property("dontDraw",PropertyType.Boolean, defaultValue:false),
                    new Property("mapped",PropertyType.Boolean, defaultValue:false),

                    new Property("special",PropertyType.Integer, defaultValue:0),
                    new Property("arg0",PropertyType.Integer, defaultValue:0),
                    new Property("arg1",PropertyType.Integer, defaultValue:0),
                    new Property("arg2",PropertyType.Integer, defaultValue:0),
                    new Property("arg3",PropertyType.Integer, defaultValue:0),
                    new Property("arg4",PropertyType.Integer, defaultValue:0),

                    new Property("sideFront",PropertyType.Integer),
                    new Property("sideBack",PropertyType.Integer, defaultValue:-1),

                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("sideDef",
                properties:new []
                {
                    new Property("offsetX",PropertyType.Integer, defaultValue:0),
                    new Property("offsetY",PropertyType.Integer, defaultValue:0),

                    new Property("textureTop",type:PropertyType.String, defaultValue:"-"),
                    new Property("textureBottom",type:PropertyType.String, defaultValue:"-"),
                    new Property("textureMiddle",type:PropertyType.String, defaultValue:"-"),

                    new Property("sector",PropertyType.Integer),

                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("vertex",
                properties:new []
                {
                    new Property("x",PropertyType.Double),
                    new Property("y",PropertyType.Double),

                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("sector",
                properties:new []
                {
                    new Property("heightFloor",PropertyType.Integer, defaultValue:0),
                    new Property("heightCeiling",PropertyType.Integer, defaultValue:0),

                    new Property("textureFloor",type:PropertyType.String),
                    new Property("textureCeiling",type:PropertyType.String),

                    new Property("lightLevel",PropertyType.Integer, defaultValue:160),

                    new Property("special",PropertyType.Integer, defaultValue:0),
                    new Property("id",PropertyType.Integer, defaultValue:0),

                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("thing",
                properties:new []
                {
                    new Property("id",PropertyType.Integer, defaultValue:0),
                    new Property("x",PropertyType.Double),
                    new Property("y",PropertyType.Double),
                    new Property("height",PropertyType.Double, defaultValue:0),
                    new Property("angle",PropertyType.Integer, defaultValue:0),
                    new Property("type",PropertyType.Integer),

                    new Property("skill1", PropertyType.Boolean, defaultValue:false),
                    new Property("skill2", PropertyType.Boolean, defaultValue:false),
                    new Property("skill3", PropertyType.Boolean, defaultValue:false),
                    new Property("skill4", PropertyType.Boolean, defaultValue:false),
                    new Property("skill5", PropertyType.Boolean, defaultValue:false),
                    new Property("ambush", PropertyType.Boolean, defaultValue:false),
                    new Property("single", PropertyType.Boolean, defaultValue:false),
                    new Property("dm", PropertyType.Boolean, defaultValue:false),
                    new Property("coop", PropertyType.Boolean, defaultValue:false),

                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("mapData",
                parsing:Parsing.Manual,
                isSubBlock:false,
                properties:new []
                {
                    new Property("nameSpace", formatName:"namespace", type:PropertyType.String),
                    new Property("comment", type:PropertyType.String, defaultValue:string.Empty),

                    new Property("lineDef",type:PropertyType.List),
                    new Property("sideDef",type:PropertyType.List),
                    new Property("vertices",singularName:"vertex",type:PropertyType.List),
                    new Property("sector",type:PropertyType.List),
                    new Property("thing",type:PropertyType.List),

                    new Property("unknownProperties", PropertyType.UnknownProperties),
                    new Property("unknownBlocks", PropertyType.UnknownBlocks),
                }),
        };
    }
}