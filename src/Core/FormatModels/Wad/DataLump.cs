// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace SectorDirector.Core.FormatModels.Wad
{
    public sealed class DataLump : ILump
    {
        private readonly byte[] _data;
        public LumpName Name { get; }
        public bool HasData => true;

        public DataLump(LumpName name, byte[] data)
        {
            Name = name;
            _data = data;
        }

        public void WriteTo(Stream stream)
        {
            stream.Write(_data, 0, _data.Length);
        }

        public byte[] GetData()
        {
            return _data;
        }
    }
}