// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using Hime.Redist;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public static partial class UdmfSemanticAnalyzer
    {
        static partial void ProcessGlobalExpression(MapData map, ASTNode assignment)
        {
            var identifier = new Identifier(assignment.Children[0].Value);
            switch (identifier.ToLower())
            {
                case "namespace":
                    map.NameSpace = ReadString(assignment);
                    break;
                case "comment":
                    map.Comment = ReadString(assignment);
                    break;
                default:
                    map.UnknownProperties.Add(new UnknownProperty(identifier, ReadRawValue(assignment)));
                    break;
            }
        }
    }
}
