// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using SectorDirector.DataModelGenerator.Utilities;

namespace SectorDirector.DataModelGenerator
{
    public static class UdmfSemanticAnalyzerGenerator
    {
        public static void WriteTo(StreamWriter stream)
        {
            using (var output = new IndentedWriter(stream))
            {
                output.Line(
                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using Hime.Redist;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing").OpenParen()
                    .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                    .Line("public static partial class UdmfSemanticAnalyzer").OpenParen();

                WriteGlobalFieldParsing(output);
                //WriteBlockParsing(output);

                //foreach (var block in UdmfDefinitions.Blocks.Where(_ => _.NormalParsing))
                //{
                //    output.Line(
                //            $"static {block.CodeName.ToPascalCase()} Parse{block.CodeName.ToPascalCase()}(IHaveAssignments block)")
                //        .OpenParen().Line($"var parsedBlock = new {block.CodeName.ToPascalCase()}();");

                //    WritePropertyAssignments(block, output, assignmentHolder: "block", owner: "parsedBlock");

                //    output.Line("return parsedBlock;").CloseParen();
                //}

                output.CloseParen();
                output.CloseParen();
            }
        }

        //private static void WritePropertyAssignments(Block block, IndentedWriter output, string assignmentHolder, string owner)
        //{
        //    foreach (var property in block.Properties.of)
        //    {
        //        var level = property.IsRequired ? "Required" : "Optional";

        //        output.Line(
        //            $"{assignmentHolder}.GetValueFor(\"{property.CodeName.ToPascalCase()}\")" +
        //            $".Set{level}{property.Type}(" +
        //            $"value => {owner}.{property.CodeName.ToPascalCase()} = value, " +
        //            $"\"{block.CodeName.ToPascalCase()}\", " +
        //            $"\"{property.CodeName.ToPascalCase()}\");");
        //    }
        //}

        private static void WriteGlobalFieldParsing(IndentedWriter output)
        {
            output.
                Line("static partial void ProcessGlobalExpression(MapData map, ASTNode assignment)").
                OpenParen().
                Line("var identifier = new Identifier(assignment.Children[0].Value);").
                Line("switch (identifier.ToLower())").
                OpenParen();

            foreach (var field in UdmfDefinitions.Blocks.Single(b => b.CodeName.ToPascalCase() == "MapData").Fields)
            {
                output.
                    Line($"case \"{field.FormatName.ToLowerInvariant()}\":").
                    IncreaseIndent().
                    Line($"map.{field.PropertyName} = Read{field.PropertyType.ToPascalCase()}(assignment);").
                    Line("break;").
                    DecreaseIndent();
            }

            output.
                Line("default:").
                IncreaseIndent().
                Line("map.UnknownProperties.Add(new UnknownProperty(identifier, ReadRawValue(assignment)));").
                Line("break;").
                DecreaseIndent().
                CloseParen().
                CloseParen();
        }

        //private static void WriteBlockParsing(IndentedWriter output)
        //{
        //    output.
        //        Line("static void SetBlocks(MapData map, ASTNode tree)").
        //        OpenParen();

        //    output.
        //        Line("foreach (var block in tree.Blocks)").
        //        OpenParen();

        //    output.Line("switch(block.Name.ToLower())");
        //    output.OpenParen();

        //    // HACK: Get around the vertex/vertices problem
        //    foreach (var block in UdmfDefinitions.Blocks.Single(_ => !_.IsSubBlock).Properties.Where(p => p.IsUdmfSubBlockList && p.Type != PropertyType.UnknownBlocks))
        //    {
        //        output.
        //            Line($"case \"{block.SingularName.ToLower()}\":").
        //            IncreaseIndent().
        //            Line($"map.{block.PropertyName}.Add(Parse{block.CollectionType}(block));").
        //            Line("break;").
        //            DecreaseIndent();
        //    }

        //    output.CloseParen();

        //    output.CloseParen().CloseParen();
        //}
    }
}