﻿using System;

namespace SectorDirector.MapGeneration
{
    [Flags]
    public enum ThingsTypes
    {
        None = 1,
        Monsters = 2,
        Bosses = 4,
        Items = 8
    }
}