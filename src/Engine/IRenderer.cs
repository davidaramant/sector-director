// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine
{
    public enum RendererType
    {
        Overhead,
        LineTest,
    }

    public static class RendererTypeExtensions
    {
        public static RendererType Next(this RendererType type) => 
            (RendererType)(((int)type + 1) % Enum.GetValues(typeof(RendererType)).Length);
    }

    public interface IRenderer
    {
        void Update(ContinuousInputs inputs, GameTime gameTime);

        void Render(ScreenBuffer screen, PlayerInfo player);
    }
}