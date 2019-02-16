// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Engine
{
    [Flags]
    public enum MovementInputs
    {
        None = 0,
        Forward = 1 << 1,
        Backward = 1 << 2,
        TurnLeft = 1 << 3,
        TurnRight = 1 << 4,
        StrafeLeft = 1 << 5,
        StrafeRight = 1 << 6,
    }
}
