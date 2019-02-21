// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using SectorDirector.Engine.Input;
using SectorDirector.Engine.Renderers;

namespace SectorDirector.Engine
{
    public sealed class GameSettings
    {
        private readonly ScreenMessage _message;
        public bool FollowMode { get; private set; } = true;
        public bool RotateMode { get; private set; } = false;
        public bool DrawAntiAliased { get; private set; } = false;
        public bool ShowRenderTime { get; private set; } = false;
        public RendererType Renderer { get; private set; } = RendererType.Overhead;

        public event EventHandler FollowModeChanged;
        public event EventHandler RotateModeChanged;
        public event EventHandler DrawAntiAliasedModeChanged;
        public event EventHandler ShowRenderTimeChanged;
        public event EventHandler RendererChanged;

        public GameSettings(ScreenMessage message)
        {
            _message = message;
        }

        public void Update(DiscreteInput input)
        {
            switch (input)
            {
                case DiscreteInput.ToggleFollowMode:
                    FollowMode = !FollowMode;
                    _message.ShowMessage($"Follow mode is {(FollowMode ? "ON" : "OFF")}");
                    FollowModeChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case DiscreteInput.ToggleRotateMode:
                    RotateMode = !RotateMode;
                    _message.ShowMessage($"Rotate mode is {(RotateMode ? "ON" : "OFF")}");
                    RotateModeChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case DiscreteInput.ToggleLineAntiAliasing:
                    DrawAntiAliased = !DrawAntiAliased;
                    _message.ShowMessage($"Draw antialiased lines is {(DrawAntiAliased ? "ON" : "OFF")}");
                    DrawAntiAliasedModeChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case DiscreteInput.ToggleShowRenderTime:
                    ShowRenderTime = !ShowRenderTime;
                    ShowRenderTimeChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case DiscreteInput.SwitchRenderer:
                    Renderer = Renderer.Next();
                    RendererChanged?.Invoke(this, EventArgs.Empty);
                    break;
                default:
                    break;
            }
        }
    }
}
