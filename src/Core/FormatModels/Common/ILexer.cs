﻿// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.Core.FormatModels.Common
{
    public interface ILexer
    {
        Token MustReadTokenOfTypes(params TokenType[] types);
    }
}