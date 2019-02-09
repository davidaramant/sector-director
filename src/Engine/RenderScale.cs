// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.Engine
{
    public enum RenderScale
    {
        OneToOne = 1,
        Half = 2,
        Quarter = 4
    }

    public static class RenderScaleExtensions
    {
        public static RenderScale DecreaseFidelity(this RenderScale scale) =>
            scale == RenderScale.OneToOne ? RenderScale.Half : RenderScale.Quarter;

        public static RenderScale IncreaseFidelity(this RenderScale scale) =>
            scale == RenderScale.Quarter ? RenderScale.Half : RenderScale.OneToOne;
    }
}