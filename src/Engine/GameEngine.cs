using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SectorDirector.Core.FormatModels.Udmf;
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
        PlayerInfo _playerInfo;
        List<MapData> _maps;
        MapGeometry _currentMap;
        private SpriteFont _messageFont;
        readonly KeyToggles _keyToggles = new KeyToggles();
        readonly ScreenMessage _screenMessage = new ScreenMessage();

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

            _keyToggles.DecreaseFidelity += KeyToggled_DecreaseFidelity;
            _keyToggles.IncreaseFidelity += KeyToggled_IncreaseFidelity;
            _keyToggles.ToggleFullscreen += KeyToggled_ToggleFullscreen;
            _keyToggles.FitToScreenZoom += (s, e) => _renderer.ResetZoom();
            _keyToggles.LoadMap += KeyToggled_LoadMap;
        }

        private void KeyToggled_DecreaseFidelity(object sender, System.EventArgs e)
        {
            _renderScale = _renderScale.DecreaseFidelity();
            var newSize = CurrentScreenSize.DivideBy((int)_renderScale);
            UpdateScreenBuffer(newSize);
        }

        private void KeyToggled_IncreaseFidelity(object sender, System.EventArgs e)
        {
            _renderScale = _renderScale.IncreaseFidelity();
            var newSize = CurrentScreenSize.DivideBy((int)_renderScale);
            UpdateScreenBuffer(newSize);
        }

        private void KeyToggled_ToggleFullscreen(object sender, System.EventArgs e)
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
            _renderer = new OverheadRenderer(_currentMap);
            _playerInfo = new PlayerInfo(_currentMap);
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

            _keyToggles.Update(keyboardState);

            var movementInputs = MovementInputs.None;

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                movementInputs |= MovementInputs.Forward;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                movementInputs |= MovementInputs.Backward;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                movementInputs |= MovementInputs.TurnLeft;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                movementInputs |= MovementInputs.TurnRight;
            }

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                movementInputs |= MovementInputs.StrafeLeft;
            }
            else if (keyboardState.IsKeyDown(Keys.E))
            {
                movementInputs |= MovementInputs.StrafeRight;
            }
            
            _playerInfo.Update(movementInputs, gameTime);

            if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                _renderer.ZoomOut(gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.OemPlus))
            {
                _renderer.ZoomIn(gameTime);
            }

            base.Update(gameTime);
        }

        void UpdateScreenBuffer(Point renderSize)
        {
            if (_screenBuffer.Dimensions != renderSize)
            {
                _screenMessage.ShowMessage($"Changing screen buffer to {renderSize.X}x{renderSize.Y}");
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

            _renderer.Render(_screenBuffer, _playerInfo);

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
