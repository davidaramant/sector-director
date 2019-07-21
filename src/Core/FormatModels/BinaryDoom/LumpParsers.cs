// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.BinaryDoom
{
    public static class LumpParsers
    {
        public static List<Vertex> Vertex(Stream stream) =>
            ReadEntries(stream, LumpEntryParser.Vertex, entryByteSize: 4);

        public static List<LineDef> LineDef(Stream stream) =>
            ReadEntries(stream, LumpEntryParser.LineDef, entryByteSize: 14);

        public static List<SideDef> SideDef(Stream stream) =>
            ReadEntries(stream, LumpEntryParser.SideDef, entryByteSize: 30);

        public static List<Sector> Sector(Stream stream) =>
            ReadEntries(stream, LumpEntryParser.Sector, entryByteSize: 26);

        public static List<Thing> Thing(Stream stream) =>
            ReadEntries(stream, LumpEntryParser.Thing, entryByteSize: 10);

        private static List<T> ReadEntries<T>(
            Stream stream, 
            Func<BinaryReader, T> entryParser, 
            int entryByteSize)
        {
            if (stream.Length % entryByteSize != 0)
            {
                throw new ParsingException($"Corrupt {typeof(T).Name.ToUpperInvariant()} lump");
            }

            List<T> entries = new List<T>();
            using (var reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true))
            {
                while (stream.Position < stream.Length)
                {
                    entries.Add(entryParser(reader));
                }
            }

            return entries;
        }
    }
}