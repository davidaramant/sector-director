// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework.Input;

namespace SectorDirector.Engine
{
    public sealed class LoadMapArgs : EventArgs
    {
        public int MapIndex { get; }

        public LoadMapArgs(int index) => MapIndex = index;
    }

    public sealed class KeyToggles
    {
        readonly KeyboardLatch _decreaseRenderFidelityLatch = new KeyboardLatch(kb => kb.IsKeyDown(Keys.OemOpenBrackets));
        readonly KeyboardLatch _increaseRenderFidelityLatch = new KeyboardLatch(kb => kb.IsKeyDown(Keys.OemCloseBrackets));
        readonly KeyboardLatch _toggleFullscreenLatch = new KeyboardLatch(kb => (kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) && kb.IsKeyDown(Keys.Enter));
        readonly KeyboardLatch _fitToScreenZoom = new KeyboardLatch(kb=>kb.IsKeyDown(Keys.F));
        readonly KeyboardLatch _loadMap1 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D1));
        readonly KeyboardLatch _loadMap2 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D2));
        readonly KeyboardLatch _loadMap3 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D3));

        public event EventHandler DecreaseFidelity;
        public event EventHandler IncreaseFidelity;
        public event EventHandler ToggleFullscreen;
        public event EventHandler FitToScreenZoom;
        public event EventHandler<LoadMapArgs> LoadMap;

        public void Update(KeyboardState keyboardState)
        {
            if (_decreaseRenderFidelityLatch.IsTriggered(keyboardState))
            {
                DecreaseFidelity?.Invoke(this, EventArgs.Empty);
            }
            else if (_increaseRenderFidelityLatch.IsTriggered(keyboardState))
            {
                IncreaseFidelity?.Invoke(this, EventArgs.Empty);
            }
            else if (_toggleFullscreenLatch.IsTriggered(keyboardState))
            {
                ToggleFullscreen?.Invoke(this, EventArgs.Empty);
            }
            else if(_fitToScreenZoom.IsTriggered(keyboardState))
            {
                FitToScreenZoom?.Invoke(this,EventArgs.Empty);
            }
            else if (_loadMap1.IsTriggered(keyboardState))
            {
                LoadMap?.Invoke(this, new LoadMapArgs(0));
            }
            else if (_loadMap2.IsTriggered(keyboardState))
            {
                LoadMap?.Invoke(this, new LoadMapArgs(1));
            }
            else if (_loadMap3.IsTriggered(keyboardState))
            {
                LoadMap?.Invoke(this, new LoadMapArgs(2));
            }
        }
    }
}
