// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;

namespace SectorDirector.DataModelGenerator
{
    public static class UdmfModelGenerator
    {
        public static void WriteToPath(string basePath)
        {
            foreach (var block in UdmfDefinitions.Blocks)
            {
                using (var blockStream = File.CreateText(Path.Combine(basePath, block.CodeName.ToPascalCase() + ".Generated.cs")))
                using (var output = new IndentedWriter(blockStream))
                {
                    output.Line(
                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectorDirector.Core.FormatModels.Udmf");

                    output.OpenParen();
                    output.Line(
                        $"[GeneratedCodeAttribute(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]");
                    output.Line(
                        $"public sealed partial class {block.CodeName.ToPascalCase()} : BaseUdmfBlock, IWriteableUdmfBlock");
                    output.OpenParen();

                    WriteProperties(block, output);
                    WriteConstructors(output, block);
                    WriteWriteToMethod(block, output);
                    WriteSemanticValidityMethods(output, block);
                    WriteCloneMethod(output, block);

                    output.CloseParen();
                    output.Line();
                    output.CloseParen(); // End namespace
                }
            }
        }

        private static void WriteCloneMethod(IndentedWriter output, Block block)
        {
            output.
                Line().
                Line($"public {block.CodeName.ToPascalCase()} Clone()").
                OpenParen().
                Line($"return new {block.CodeName.ToPascalCase()}(").IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                var postfix = indexed.index == block.Properties.Count() - 1 ? ");" : ",";

                if (!indexed.param.IsScalarField)
                {
                    postfix = ".Select(item => item.Clone())" + postfix;

                }
                output.Line(indexed.param.ArgumentName + ": " + indexed.param.PropertyName + postfix);
            }

            output.
                DecreaseIndent().
                CloseParen();
        }

        private static void WriteProperties(Block block, IndentedWriter sb)
        {
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                sb.Line($"private bool {property.CodeName.ToFieldName()}HasBeenSet = false;").
                    Line($"private {property.PropertyTypeString} {property.CodeName.ToFieldName()};").
                    Line($"public {property.PropertyTypeString} {property.CodeName.ToPascalCase()}").
                    OpenParen().
                    Line($"get {{ return {property.CodeName.ToFieldName()}; }}").
                    Line($"set").
                    OpenParen().
                    Line($"{property.CodeName.ToFieldName()}HasBeenSet = true;").
                    Line($"{property.CodeName.ToFieldName()} = value;").
                    CloseParen().
                    CloseParen();
            }

            foreach (var property in block.Properties.Where(_ => !(_.IsScalarField && _.IsRequired)))
            {
                sb.Line(property.PropertyDefinition);
            }
        }

        private static void WriteConstructors(IndentedWriter sb, Block block)
        {
            sb.Line($"public {block.CodeName.ToPascalCase()}() {{ }}");
            sb.Line($"public {block.CodeName.ToPascalCase()}(");
            sb.IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                sb.Line(indexed.param.ArgumentDefinition + (indexed.index == block.Properties.Count() - 1 ? ")" : ","));
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in block.OrderedProperties())
            {
                sb.Line(property.SetProperty);
            }

            sb.Line(@"AdditionalSemanticChecks();");
            sb.CloseParen();
        }

        private static void WriteWriteToMethod(Block block, IndentedWriter sb)
        {
            sb.Line(@"public Stream WriteTo(Stream stream)").
                OpenParen().
                Line("CheckSemanticValidity();");

            var indent = block.IsSubBlock ? "true" : "false";

            if (block.IsSubBlock)
            {
                sb.Line($"WriteLine(stream, \"{block.FormatName}\");");
                sb.Line("WriteLine(stream, \"{\");");
            }

            // WRITE ALL REQUIRED PROPERTIES
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                sb.Line(
                    $"WriteProperty(stream, \"{property.FormatName}\", {property.CodeName.ToFieldName()}, indent: {indent});");
            }
            // WRITE OPTIONAL PROPERTIES
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && !_.IsRequired))
            {
                sb.Line(
                    $"if ({property.CodeName.ToPascalCase()} != {property.DefaultAsString}) WriteProperty(stream, \"{property.FormatName}\", {property.CodeName.ToPascalCase()}, indent: {indent});");
            }

            // WRITE UNKNOWN PROPERTES
            sb.Line($"foreach (var property in UnknownProperties)").
                OpenParen().
                Line($"WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: {indent});").
                CloseParen();

            // WRITE SUBBLOCKS
            foreach (var subBlock in block.Properties.Where(p => p.IsUdmfSubBlockList))
            {
                sb.Line($"WriteBlocks(stream, {subBlock.PropertyName} );");
            }

            if (block.IsSubBlock)
            {
                sb.Line("WriteLine(stream, \"}\");");
            }
            sb.Line("return stream;").
                CloseParen();
        }

        private static void WriteSemanticValidityMethods(IndentedWriter output, Block block)
        {
            output.Line(@"public void CheckSemanticValidity()").
                OpenParen();

            // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                output.Line(
                    $"if (!{property.CodeName.ToFieldName()}HasBeenSet) throw new InvalidUdmfException(\"Did not set {property.CodeName.ToPascalCase()} on {block.CodeName.ToPascalCase()}\");");
            }

            output.Line(@"AdditionalSemanticChecks();").
                CloseParen().
                Line().
                Line("partial void AdditionalSemanticChecks();");
        }
    }
}