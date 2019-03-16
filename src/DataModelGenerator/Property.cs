﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Functional.Maybe;
using SectorDirector.DataModelGenerator.PropertyTypes;

namespace SectorDirector.DataModelGenerator
{
    public enum PropertyType
    {
        Flag,
        Identifier,
        Integer,
        Ushort,
        Double,
        Boolean,
        String,
        Char,
        Set,
        Block,
        List,
        ImmutableList,
        MappedBlockList,
        UnknownProperties,
        UnknownBlocks,
    }

    public sealed class Property : NamedItem
    {
        private string _collectionType;
        private readonly Maybe<string> _singularName;

        public PropertyType Type { get; }
        public bool IsMetaData { get; }
        public bool HasDefault => _defaultValue != null;

        public string CollectionType
        {
            get { return _collectionType ?? SingularName.ToPascalCase(); }
            private set { _collectionType = value; }
        }

        public string SingularName => _singularName.OrElse(CodeName);

        //public string PropertyType
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //                return "bool";
        //            case PropertyType.Char:
        //                return "char";
        //            case FieldType.Float:
        //                return "double";
        //            case FieldType.Integer:
        //                return "int";
        //            case PropertyType.Ushort:
        //                return "ushort";
        //            case PropertyType.Identifier:
        //            case FieldType.String:
        //                return "string";
        //            case PropertyType.Set:
        //                return $"HashSet<{CollectionType}>";
        //            case PropertyType.Block:
        //                return CollectionType ?? CodeName.ToPascalCase();
        //            case PropertyType.List:
        //                return $"List<{CollectionType}>";
        //            case PropertyType.ImmutableList:
        //                return $"ImmutableList<{CollectionType}>";
        //            case PropertyType.MappedBlockList:
        //                return $"Dictionary<ushort,{CollectionType}>";
        //            case PropertyType.UnknownProperties:
        //                return "List<UnknownProperty>";
        //            case PropertyType.UnknownBlocks:
        //                return "List<UnknownBlock>";
        //            default:
        //                throw new NotImplementedException("Unknown property type: " + Type);
        //        }
        //    }
        //}

        //public string ArgumentTypeString
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //                return "bool";
        //            case PropertyType.Char:
        //                return "char";
        //            case FieldType.Float:
        //                return "double";
        //            case FieldType.Integer:
        //                return "int";
        //            case PropertyType.Ushort:
        //                return "ushort";
        //            case PropertyType.Identifier:
        //            case FieldType.String:
        //                return "string";
        //            case PropertyType.Block:
        //                return CollectionType;
        //            case PropertyType.Set:
        //            case PropertyType.List:
        //            case PropertyType.ImmutableList:
        //                return $"IEnumerable<{CollectionType}>";
        //            case PropertyType.MappedBlockList:
        //                return $"Dictionary<ushort,{CollectionType}>";
        //            case PropertyType.UnknownProperties:
        //                return "IEnumerable<UnknownProperty>";
        //            case PropertyType.UnknownBlocks:
        //                return "IEnumerable<UnknownBlock>";
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string ArgumentName
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case FieldType.Integer:
        //            case PropertyType.Ushort:
        //            case FieldType.String:
        //            case PropertyType.Block:
        //            case PropertyType.Identifier:
        //                return CodeName.ToCamelCase();
        //            case PropertyType.List:
        //            case PropertyType.Set:
        //            case PropertyType.ImmutableList:
        //            case PropertyType.MappedBlockList:
        //                return _singularName.HasValue ? CodeName.ToCamelCase() : CodeName.ToPluralCamelCase();
        //            case PropertyType.UnknownProperties:
        //                return "unknownProperties";
        //            case PropertyType.UnknownBlocks:
        //                return "unknownBlocks";
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string PropertyName
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Block:
        //            case PropertyType.Identifier:
        //                return CodeName.ToPascalCase();
        //            case PropertyType.Set:
        //            case PropertyType.List:
        //            case PropertyType.ImmutableList:
        //            case PropertyType.MappedBlockList:
        //                return _singularName.HasValue ? CodeName.ToPascalCase() : CodeName.ToPluralPascalCase();
        //            case PropertyType.UnknownProperties:
        //                return "UnknownProperties";
        //            case PropertyType.UnknownBlocks:
        //                return "UnknownBlocks";
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string PropertyDefinition
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Identifier:
        //                return $"public {PropertyType} {PropertyName} {{ get; set; }} = {DefaultAsString};";
        //            case PropertyType.Set:
        //            case PropertyType.Block:
        //            case PropertyType.List:
        //            case PropertyType.MappedBlockList:
        //            case PropertyType.UnknownProperties:
        //            case PropertyType.UnknownBlocks:
        //                return $"public {PropertyType} {PropertyName} {{ get; }} = new {PropertyType}();";
        //            case PropertyType.ImmutableList:
        //                return $"public {PropertyType} {PropertyName} {{ get; }} = {PropertyType}.Empty;";

        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string ArgumentDefinition => $"{ArgumentTypeString} {ArgumentName}" + (IsRequired ? string.Empty : $" = {DefaultAsString}");

        //public string SetProperty
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Block:
        //            case PropertyType.Identifier:
        //                return $"{PropertyName} = {ArgumentName};";
        //            case PropertyType.ImmutableList:
        //                return $"{PropertyName} = {ArgumentName}.ToImmutableList();";
        //            case PropertyType.Set:
        //            case PropertyType.List:
        //            case PropertyType.MappedBlockList:
        //                return $"{PropertyName}.AddRange({ArgumentName});";
        //            case PropertyType.UnknownProperties:
        //                return $"{PropertyName}.AddRange({ArgumentName} ?? Enumerable.Empty<UnknownProperty>());";
        //            case PropertyType.UnknownBlocks:
        //                return $"{PropertyName}.AddRange({ArgumentName} ?? Enumerable.Empty<UnknownBlock>());";
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}
        
        public bool IsUdmfSubBlockList =>
            Type == PropertyType.List ||
            Type == PropertyType.UnknownBlocks;

        private readonly object _defaultValue;

        //public string DefaultAsString
        //{
        //    get
        //    {
        //        if (!IsScalarField)
        //        {
        //            return "null";
        //        }

        //        switch (Type)
        //        {
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.Float:
        //                return _defaultValue.ToString();

        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //                return _defaultValue.ToString().ToLowerInvariant();

        //            case FieldType.String:
        //            case PropertyType.Identifier:
        //                return "\"" + _defaultValue + "\"";

        //            case PropertyType.Char:
        //                return "'" + _defaultValue + "'";

        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string DefaultAssignment => IsRequired ? string.Empty : $" = {DefaultAsString}";

        //public bool IsRequired
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Block:
        //            case PropertyType.Identifier:
        //                return _defaultValue == null;
        //            case PropertyType.Set:
        //            case PropertyType.List:
        //            case PropertyType.ImmutableList:
        //            case PropertyType.MappedBlockList:
        //                return true;
        //            case PropertyType.UnknownProperties:
        //            case PropertyType.UnknownBlocks:
        //                return false;
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public bool IsScalarField
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Block:
        //            case PropertyType.Identifier:
        //                return true;
        //            case PropertyType.Set:
        //            case PropertyType.List:
        //            case PropertyType.ImmutableList:
        //            case PropertyType.MappedBlockList:
        //            case PropertyType.UnknownProperties:
        //            case PropertyType.UnknownBlocks:
        //                return false;
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        //public string ParsingTypeName
        //{
        //    get
        //    {
        //        switch (Type)
        //        {
        //            case PropertyType.Flag:
        //            case FieldType.Boolean:
        //            case PropertyType.Char:
        //            case FieldType.Float:
        //            case PropertyType.Ushort:
        //            case FieldType.Integer:
        //            case FieldType.String:
        //            case PropertyType.Identifier:
        //                return Type.ToString();
        //            case PropertyType.Block:
        //                return CollectionType;
        //            case PropertyType.Set:
        //                return CollectionType.ToPascalCase() + "Set";
        //            case PropertyType.List:
        //                return CollectionType.ToPascalCase() + "List";
        //            case PropertyType.ImmutableList:
        //                return CollectionType.ToPascalCase() + "ImmutableList";
        //            default:
        //                throw new NotImplementedException("Unknown property type.");
        //        }
        //    }
        //}

        public Property(
                string name,
                PropertyType type,
                bool isMetaData = false,
                string formatName = null,
                string singularName = null,
                object defaultValue = null,
                string collectionType = null) :
                    base(
                        formatName: formatName ?? name,
                        codeName: name)
        {
            Type = type;
            IsMetaData = isMetaData;
            _singularName = singularName.ToMaybe();
            _defaultValue = defaultValue;
            CollectionType = collectionType;
        }
    }
}