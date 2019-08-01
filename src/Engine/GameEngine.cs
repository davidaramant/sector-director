// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SectorDirector.Engine.Renderers;
using SectorDirector.Core.FormatModels.Wad;

namespace SectorDirector.Engine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public sealed class GameEngine : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D _outputTexture;
        RenderScale _renderScale = RenderScale.Normal;
        ScreenBuffer _screenBuffer;
        PlayerInfo _playerInfo;
        List<MapData> _maps;
        MapGeometry _currentMap;
        private SpriteFont _messageFont;
        readonly KeyToggles _keyToggles = new KeyToggles();
        readonly ContinuousInputs _continuousInputs = new ContinuousInputs();
        readonly ScreenMessage _screenMessage = new ScreenMessage();
        readonly FrameTimeAggregator _frameTimeAggregator = new FrameTimeAggregator();
        private readonly GameSettings _settings;
        IRenderer _renderer;

        private Point CurrentScreenSize => new Point(
                x: _graphics.PreferredBackBufferWidth,
                y: _graphics.PreferredBackBufferHeight);

        public GameEngine()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true,
            };
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (s, e) => UpdateScreenBuffer(CurrentScreenSize.DivideBy(_renderScale));

            _settings = new GameSettings(_screenMessage);
            _settings.RendererChanged += (s, e) => RecreateRenderer();

            _keyToggles.DecreaseFidelity += KeyToggled_DecreaseFidelity;
            _keyToggles.IncreaseFidelity += KeyToggled_IncreaseFidelity;
            _keyToggles.FullScreen += KeyToggled_FullScreen;
            _keyToggles.LoadMap += KeyToggled_LoadMap;
        }

        private void RecreateRenderer()
        {
            switch (_settings.Renderer)
            {
                case RendererType.LineTest:
                    _renderer = new LineTestRenderer(_settings, _screenMessage);
                    break;

                case RendererType.Overhead:
                    _renderer = new OverheadRenderer(_settings, _currentMap);
                    break;

                case RendererType.FirstPerson:
                    _renderer = new FirstPersonRenderer(_settings, _currentMap, _screenMessage);
                    break;

                case RendererType.Fire:
                    _renderer = new FireRenderer();
                    break;

                case RendererType.MapHistory:
                    _renderer = new MapHistoryRenderer(_currentMap, _screenMessage);
                    break;

                default:
                    throw new System.Exception("Unknown renderer type");
            }
        }

        private void KeyToggled_DecreaseFidelity(object sender, System.EventArgs e)
        {
            _renderScale = _renderScale.DecreaseFidelity();
            var newSize = CurrentScreenSize.DivideBy(_renderScale);
            UpdateScreenBuffer(newSize);
        }

        private void KeyToggled_IncreaseFidelity(object sender, System.EventArgs e)
        {
            _renderScale = _renderScale.IncreaseFidelity();
            var newSize = CurrentScreenSize.DivideBy(_renderScale);
            UpdateScreenBuffer(newSize);
        }

        private void KeyToggled_FullScreen(object sender, System.EventArgs e)
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            if (_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                _graphics.PreferredBackBufferWidth = 800;
                _graphics.PreferredBackBufferHeight = 600;
            }
            _graphics.ApplyChanges();
        }

        private void KeyToggled_LoadMap(object sender, LoadMapArgs e)
        {
            if (e.MapIndex < _maps.Count)
            {
                SwitchToMap(e.MapIndex);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            TargetElapsedTime = System.TimeSpan.FromSeconds(1 / 60.0);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: CurrentScreenSize.X, height: CurrentScreenSize.Y);
            _screenBuffer = new ScreenBuffer(CurrentScreenSize);

            _messageFont = Content.Load<SpriteFont>("Fonts/ScreenMessage");
            _maps = WadLoader.Load("testmaps.wad");

            SwitchToMap(0);
        }

        private void SwitchToMap(int index)
        {
            _screenMessage.ShowMessage($"Switching to map index {index}");
            var map = _maps[index];
            _currentMap = new MapGeometry(map);
            _playerInfo = PlayerInfo.Create(_currentMap);
            RecreateRenderer();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            _graphics.Dispose();
            _spriteBatch.Dispose();
            _outputTexture.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                // It randomly crashes if exiting in fullscreen for whatever reason
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();

                Exit();
            }

            var discreteInput = _keyToggles.Update(keyboard);
            _continuousInputs.Forward = keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W);
            _continuousInputs.Backward = keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S);
            _continuousInputs.TurnLeft = keyboard.IsKeyDown(Keys.Left);
            _continuousInputs.TurnRight = keyboard.IsKeyDown(Keys.Right);
            _continuousInputs.StrafeLeft = keyboard.IsKeyDown(Keys.Q);
            _continuousInputs.StrafeRight = keyboard.IsKeyDown(Keys.E);
            _continuousInputs.ZoomOut = keyboard.IsKeyDown(Keys.OemMinus);
            _continuousInputs.ZoomIn = keyboard.IsKeyDown(Keys.OemPlus);
            _continuousInputs.ResetZoom = keyboard.IsKeyDown(Keys.Z);

            if (_settings.FollowMode)
            {
                _playerInfo.Update(_continuousInputs, gameTime);
            }
            _renderer.Update(_continuousInputs, gameTime);
            _settings.Update(discreteInput);

            base.Update(gameTime);
        }

        void UpdateScreenBuffer(Point renderSize)
        {
            if (_screenBuffer.Dimensions != renderSize)
            {
                _frameTimeAggregator.Reset();
                _screenMessage.ShowMessage($"Changing screen buffer to {renderSize.X}x{renderSize.Y}");
                _outputTexture.Dispose();
                _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: renderSize.X, height: renderSize.Y);
                _screenBuffer = new ScreenBuffer(renderSize);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointWrap,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            if (_settings.ShowRenderTime)
                _frameTimeAggregator.StartTiming();

            _renderer.Render(_screenBuffer, _playerInfo);

            if (_settings.ShowRenderTime)
                _frameTimeAggregator.StopTiming();

            _screenBuffer.CopyToTexture(_outputTexture);

            _spriteBatch.Draw(
                texture: _outputTexture,
                destinationRectangle: new Rectangle(
                    x: 0,
                    y: 0,
                    width: CurrentScreenSize.X,
                    height: CurrentScreenSize.Y),
                color: Color.White);

            var message = _screenMessage.MaybeGetTextToShow(gameTime);
            if (message != null)
            {
                DrawShadowedString(_messageFont, message, new Vector2(0, 0), Color.White);
            }

            if (_settings.ShowRenderTime)
            {
                var text = $"Average render time: {_frameTimeAggregator.GetAverageFrameTimeInMs():#0.00}ms";
                var size = _messageFont.MeasureString(text);
                DrawShadowedString(_messageFont, text, new Vector2(0, CurrentScreenSize.Y - size.Y), Color.Red);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            _spriteBatch.DrawString(font, value, position, color);
        }
    }
}
