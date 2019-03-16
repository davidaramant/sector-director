// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.DataModelGenerator
{
    public class NamedItem
    {
        public NamedItem(string formatName, string codeName)
        {
            FormatName = formatName;
            CodeName = codeName;
        }

        public string FormatName { get; }
        public string CodeName { get; }
    }
}