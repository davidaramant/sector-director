// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace SectorDirector.Engine.Input
{
    public sealed class LoadMapArgs : EventArgs
    {
        public int MapIndex { get; }

        public LoadMapArgs(int index) => MapIndex = index;
    }

    public sealed class KeyToggles
    {
        readonly KeyboardLatch _toggleFullscreenLatch = new KeyboardLatch(kb => (kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) && kb.IsKeyDown(Keys.Enter));
        readonly KeyboardLatch _decreaseFidelity = new KeyboardLatch(Keys.OemOpenBrackets);
        readonly KeyboardLatch _increaseFidelity = new KeyboardLatch(Keys.OemCloseBrackets);
        readonly KeyboardLatch _loadMap1 = new KeyboardLatch(Keys.D1);
        readonly KeyboardLatch _loadMap2 = new KeyboardLatch(Keys.D2);
        readonly KeyboardLatch _loadMap3 = new KeyboardLatch(Keys.D3);

        readonly List<(KeyboardLatch latch, DiscreteInput input)> _simpleToggles = new List<(KeyboardLatch latch, DiscreteInput input)>();

        public KeyToggles()
        {
            AddSimpleToggles(
                (Keys.F, DiscreteInput.ToggleFollowMode),
                (Keys.R, DiscreteInput.ToggleRotateMode),
                (Keys.A, DiscreteInput.ToggleShowRenderTime),
                (Keys.D, DiscreteInput.ToggleLineAntiAliasing),
                (Keys.T, DiscreteInput.SwitchRenderer),
                (Keys.Tab, DiscreteInput.ToggleOverheadMap)
            );
        }

        private void AddSimpleToggles(params (Keys key, DiscreteInput input)[] simpleToggles)
        {
            foreach (var simple in simpleToggles)
            {
                _simpleToggles.Add((new KeyboardLatch(simple.key), simple.input));
            }
        }

        public event EventHandler DecreaseFidelity;
        public event EventHandler IncreaseFidelity;
        public event EventHandler FullScreen;
        public event EventHandler<LoadMapArgs> LoadMap;

        public DiscreteInput Update(KeyboardState keyboardState)
        {
            foreach (var simpleToggle in _simpleToggles)
            {
                if (simpleToggle.latch.IsTriggered(keyboardState))
                {
                    return simpleToggle.input;
                }
            }

            if (_toggleFullscreenLatch.IsTriggered(keyboardState))
            {
                FullScreen?.Invoke(this, EventArgs.Empty);
            }
            else if (_decreaseFidelity.IsTriggered(keyboardState))
            {
                DecreaseFidelity?.Invoke(this, EventArgs.Empty);
            }
            else if (_increaseFidelity.IsTriggered(keyboardState))
            {
                IncreaseFidelity?.Invoke(this, EventArgs.Empty);
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

            return DiscreteInput.None;
        }
    }
}
