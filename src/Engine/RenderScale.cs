// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.Engine
{
    public enum RenderScale
    {
        Normal = 1,
        Quarter = 2,
        Eighth = 4
    }

    public static class RenderScaleExtensions
    {
        public static RenderScale DecreaseFidelity(this RenderScale scale) =>
            scale == RenderScale.Normal ? RenderScale.Quarter : RenderScale.Eighth;

        public static RenderScale IncreaseFidelity(this RenderScale scale) =>
            scale == RenderScale.Eighth ? RenderScale.Quarter : RenderScale.Normal;
    }
}