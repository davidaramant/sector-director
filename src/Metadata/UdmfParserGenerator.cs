﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;

namespace SectorDirector.Metadata
{
    public static class UdmfParserGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing").
OpenParen().
Line("public static partial class UdmfParser").
OpenParen();

            WriteGlobalAssignmentParsing(output);
            WriteBlockParsing(output);

            foreach (var block in UdmfDefinitions.Blocks.Where(_ => _.NormalParsing))
            {
                output.
                    Line($"public static {block.ClassName.ToPascalCase()} Parse{block.ClassName.ToPascalCase()}(IHaveAssignments block)").
                    OpenParen().
                    Line($"var parsedBlock = new {block.ClassName.ToPascalCase()}();");

                WritePropertyAssignments(block, output, assignmentHolder: "block", owner: "parsedBlock");

                output.Line("return parsedBlock;").CloseParen();
            }

            output.CloseParen();
            return output.CloseParen().GetString();
        }

        private static void WritePropertyAssignments(Block block, IndentedWriter output, string assignmentHolder, string owner)
        {
            foreach (var property in block.Properties.Where(p => p.IsScalarField))
            {
                var level = property.IsRequired ? "Required" : "Optional";

                output.Line(
                    $"{assignmentHolder}.GetValueFor(\"{property.ClassName.ToPascalCase()}\")" +
                    $".Set{level}{property.Type}(" +
                    $"value => {owner}.{property.ClassName.ToPascalCase()} = value, " +
                    $"\"{block.ClassName.ToPascalCase()}\", " +
                    $"\"{property.ClassName.ToPascalCase()}\");");
            }
        }

        private static void WriteGlobalAssignmentParsing(IndentedWriter output)
        {
            output.
                Line("static partial void SetGlobalAssignments(MapData map, UdmfSyntaxTree tree)").
                OpenParen();

            WritePropertyAssignments(
                UdmfDefinitions.Blocks.Single(b => b.ClassName.ToPascalCase() == "MapData"),
                output, assignmentHolder: "tree", owner: "map");

            output.CloseParen();
        }

        private static void WriteBlockParsing(IndentedWriter output)
        {
            output.
                Line("static partial void SetBlocks(MapData map, UdmfSyntaxTree tree)").
                OpenParen();

            output.
                Line("foreach (var block in tree.Blocks)").
                OpenParen();

            output.Line("switch(block.Name.ToLower())");
            output.OpenParen();

            foreach (var block in UdmfDefinitions.Blocks.Where(_ => _.NormalParsing))
            {
                output.
                    Line($"case \"{block.ClassName.ToCamelCase()}\":").
                    IncreaseIndent().
                    Line($"map.{block.ClassName.ToPluralPascalCase()}.Add(Parse{block.ClassName.ToPascalCase()}(block));").
                    Line("break;").
                    DecreaseIndent();
            }

            output.CloseParen();

            output.CloseParen().CloseParen();
        }
    }
}