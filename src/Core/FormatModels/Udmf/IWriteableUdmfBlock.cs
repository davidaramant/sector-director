// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace SectorDirector.Core.FormatModels.Udmf
{
    public interface IWriteableUdmfBlock
    {
        Stream WriteTo(Stream stream);
    }
}