// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using Microsoft.Xna.Framework.Input;

namespace SectorDirector.Engine
{
    public sealed class KeyboardLatch
    {
        readonly Func<KeyboardState, bool> _keyMatcher;
        bool _pressing = false;

        public KeyboardLatch(Func<KeyboardState, bool> keyMatcher)
        {
            _keyMatcher = keyMatcher;
        }

        public event EventHandler Triggered;

        public bool IsTriggered(KeyboardState state)
        {
            if (_keyMatcher(state))
            {
                if (!_pressing)
                {
                    _pressing = true;
                    Triggered?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }
            else
            {
                _pressing = false;
            }
            return false;
        }
    }
}