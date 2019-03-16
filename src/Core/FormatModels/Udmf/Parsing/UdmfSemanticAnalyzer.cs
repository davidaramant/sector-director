// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Hime.Redist;
using Piglet.Parser;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing
{
    public static partial class UdmfSemanticAnalyzer
    {
        public static MapData Process(ParseResult result)
        {
            if (!result.IsSuccess)
            {
                throw new ParseException("Error parsing UDMF");
            }

            var map = new MapData();

            foreach (var globalExpression in result.Root.Children)
            {
                var child = globalExpression.Children[0];
                if (child.Symbol.Name == "assignment_expr")
                {
                    ProcessGlobalExpression(map, child);
                }
                else
                {

                }
            }

            return map;
        }

        static partial void ProcessGlobalExpression(MapData map, ASTNode assignment);
        //static void ProcessGlobalExpression(MapData map, ASTNode assignment)
        //{
        //    var identifier = new Identifier(assignment.Children[0].Value);
        //    switch (identifier.ToLower())
        //    {
        //        case "namespace":
        //            map.NameSpace = ReadString(assignment);
        //            break;

        //        case "comment":
        //            map.Comment = ReadString(assignment);
        //            break;

        //        default:
        //            map.UnknownProperties.Add(new UnknownProperty(identifier, ReadRawValue(assignment)));
        //            break;
        //    }
        //}

        static string ReadRawValue(ASTNode assignment) => assignment.Children[2].Children[0].Value;

        static string ReadString(ASTNode assignment)
        {
            var quotedString = ReadRawValue(assignment);
            return quotedString.Substring(1, quotedString.Length - 2);
        }

        static partial void ProcessExpression(MapData map, ASTNode globalExpression);
    }
}