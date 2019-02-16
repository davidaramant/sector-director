// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class ScreenMessage
    {
        enum State
        {
            NothingToShow,
            NewText,
            TimerRunning,
        }

        private static readonly TimeSpan TimeToShow = TimeSpan.FromSeconds(1);
        private State _currentState = State.NothingToShow;
        private TimeSpan _firstShown;
        private string _currentMessage;

        public void ShowMessage(string message)
        {
            _currentMessage = message;
            _currentState = State.NewText;
        }

        public string MaybeGetTextToShow(GameTime currentTime)
        {
            switch (_currentState)
            {
                case State.NewText:
                    _firstShown = currentTime.TotalGameTime;
                    _currentState = State.TimerRunning;
                    return _currentMessage;

                case State.TimerRunning:
                    if (currentTime.TotalGameTime - _firstShown >= TimeToShow)
                    {
                        _currentState = State.NothingToShow;
                    }
                    return _currentMessage;

                case State.NothingToShow:
                default:
                    return null;
            }
        }

    }
}
