﻿using SectorDirector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        readonly KeyboardLatch _decreaseRenderFidelityLatch = new KeyboardLatch(kb => kb.IsKeyDown(Keys.OemOpenBrackets));
        readonly KeyboardLatch _increaseRenderFidelityLatch = new KeyboardLatch(kb => kb.IsKeyDown(Keys.OemCloseBrackets));
        readonly KeyboardLatch _toggleFullscreenLatch = new KeyboardLatch(kb => (kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) && kb.IsKeyDown(Keys.Enter));
        OverheadRenderer _renderer;

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
            Window.ClientSizeChanged += (s, e) => UpdateScreenBuffer(CurrentScreenSize);
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

            _renderer = new OverheadRenderer(PyramidMap.Create());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (_decreaseRenderFidelityLatch.IsTriggered(keyboardState))
            {
                _renderScale = _renderScale.DecreaseFidelity();
                var newSize = CurrentScreenSize.DivideBy((int)_renderScale);
                UpdateScreenBuffer(newSize);
            }

            if (_increaseRenderFidelityLatch.IsTriggered(keyboardState))
            {
                _renderScale = _renderScale.IncreaseFidelity();
                var newSize = CurrentScreenSize.DivideBy((int)_renderScale);
                UpdateScreenBuffer(newSize);
            }

            if (_toggleFullscreenLatch.IsTriggered(keyboardState))
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
                _renderScale = RenderScale.Normal;
                _graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        void UpdateScreenBuffer(Point renderSize)
        {
            if (_screenBuffer.Dimensions != renderSize)
            {
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

            _renderer.Render(_screenBuffer);

            _screenBuffer.CopyToTexture(_outputTexture);

            _spriteBatch.Draw(
                texture: _outputTexture,
                destinationRectangle: new Rectangle(
                    x: 0,
                    y: 0,
                    width: CurrentScreenSize.X,
                    height: CurrentScreenSize.Y),
                color: Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
