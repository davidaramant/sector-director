// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Engine.Input
{
    [Flags]
    public enum ContinuousInputs
    {
        None = 0,
        Forward = 1 << 1,
        Backward = 1 << 2,
        TurnLeft = 1 << 3,
        TurnRight = 1 << 4,
        StrafeLeft = 1 << 5,
        StrafeRight = 1 << 6,
        ZoomIn = 1 << 7,
        ZoomOut = 1 << 8,
        ResetZoom = 1 << 9,
    }
}
