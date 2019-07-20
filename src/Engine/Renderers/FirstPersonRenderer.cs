// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine.Renderers
{
    public sealed class FirstPersonRenderer : IRenderer
    {
        readonly GameSettings _settings;
        readonly MapGeometry _map;

        public FirstPersonRenderer(GameSettings settings, MapGeometry map)
        {
            _settings = settings;
            _map = map;
        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {

        }

        public void Render(ScreenBuffer screen, PlayerInfo player)
        {
            screen.Clear();
        }
    }
}