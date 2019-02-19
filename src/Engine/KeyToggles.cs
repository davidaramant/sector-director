// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
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
        readonly KeyboardLatch _toggleFullscreenLatch = new KeyboardLatch(kb => (kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) && kb.IsKeyDown(Keys.Enter));
        readonly KeyboardLatch _loadMap1 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D1));
        readonly KeyboardLatch _loadMap2 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D2));
        readonly KeyboardLatch _loadMap3 = new KeyboardLatch(kb => kb.IsKeyDown(Keys.D3));

        readonly List<KeyboardLatch> _simpleToggles = new List<KeyboardLatch>();

        public KeyToggles()
        {
            AddSimpleToggles(
                (Keys.OemOpenBrackets, () => DecreaseFidelity),
                (Keys.OemCloseBrackets, () => IncreaseFidelity),
                (Keys.Z, () => FitToScreenZoom),
                (Keys.F, () => FollowMode),
                (Keys.R, () => RotateMode),
                (Keys.A, () => ShowFrameTime)
            );
        }

        private void AddSimpleToggles(params (Keys key, Func<EventHandler> signal)[] simpleToggles)
        {
            foreach (var simple in simpleToggles)
            {
                var latch = new KeyboardLatch(kb => kb.IsKeyDown(simple.key));
                latch.Triggered += (s, e) => simple.signal()?.Invoke(this, EventArgs.Empty);
                _simpleToggles.Add(latch);
            }
        }

        public event EventHandler DecreaseFidelity;
        public event EventHandler IncreaseFidelity;
        public event EventHandler FullScreen;
        public event EventHandler FollowMode;
        public event EventHandler RotateMode;
        public event EventHandler FitToScreenZoom;
        public event EventHandler ShowFrameTime;
        public event EventHandler<LoadMapArgs> LoadMap;

        public void Update(KeyboardState keyboardState)
        {
            foreach (var simpleToggle in _simpleToggles)
            {
                simpleToggle.IsTriggered(keyboardState);
            }

            if (_toggleFullscreenLatch.IsTriggered(keyboardState))
            {
                FullScreen?.Invoke(this, EventArgs.Empty);
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
