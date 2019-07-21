// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;

namespace SectorDirector.Core.BinaryReaderExtensions
{
    public static class Extensions
    {
        public static string ReadText(this BinaryReader reader, int length)
        {
            return Encoding.ASCII.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }
    }
}
