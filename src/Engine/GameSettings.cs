// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using SectorDirector.Engine.Input;
using SectorDirector.Engine.Renderers;

namespace SectorDirector.Engine
{
    public sealed class GameSettings
    {
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

        public GameSettings(KeyToggles keyToggles, ScreenMessage message)
        {
            keyToggles.FollowMode += (s, e) =>
            {
                FollowMode = !FollowMode;
                message.ShowMessage($"Follow mode is {(FollowMode ? "ON" : "OFF")}");
                FollowModeChanged?.Invoke(this, EventArgs.Empty);
            };
            keyToggles.RotateMode += (s, e) =>
            {
                RotateMode = !RotateMode;
                message.ShowMessage($"Rotate mode is {(RotateMode ? "ON" : "OFF")}");
                RotateModeChanged?.Invoke(this, EventArgs.Empty);
            };
            keyToggles.DrawAntiAliased += (s, e) =>
            {
                DrawAntiAliased = !DrawAntiAliased;
                message.ShowMessage($"Draw antialiased lines is {(DrawAntiAliased ? "ON" : "OFF")}");
                DrawAntiAliasedModeChanged?.Invoke(this, EventArgs.Empty);
            };
            keyToggles.ShowRenderTime += (s, e) =>
            {
                ShowRenderTime = !ShowRenderTime;
                ShowRenderTimeChanged?.Invoke(this, EventArgs.Empty);
            };
            keyToggles.SwitchRenderer += (s, e) =>
            {
                Renderer = Renderer.Next();
                RendererChanged?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
