using SectorDirector.Core;
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
        RenderScale _renderScale = RenderScale.OneToOne;
        ScreenBuffer _screenBuffer;
        bool _increasingRenderFidelity = false;
        bool _decreasingRenderFidelity = false;
        OverheadRenderer _renderer;

        private Size ScreenSize { get; set; } = new Size(800, 600);

        public GameEngine()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenSize.Width,
                PreferredBackBufferHeight = ScreenSize.Height,
                IsFullScreen = false,
                //                IsFullScreen = true,
                SynchronizeWithVerticalRetrace = true,
            };
            Content.RootDirectory = "Content";
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
            ScreenSize = new Size(
                width: GraphicsDevice.PresentationParameters.BackBufferWidth,
                height: GraphicsDevice.PresentationParameters.BackBufferHeight);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: ScreenSize.Width, height: ScreenSize.Height);
            _screenBuffer = new ScreenBuffer(ScreenSize);

            _renderer = new OverheadRenderer(SimpleExampleMap.Create());
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

            if (keyboardState.IsKeyDown(Keys.OemOpenBrackets))
            {
                if (!_decreasingRenderFidelity)
                {
                    _decreasingRenderFidelity = true;
                    _renderScale = _renderScale.DecreaseFidelity();
                    var newSize = ScreenSize.DivideBy((int)_renderScale);
                    UpdateScreenBuffer(newSize);
                }
            }
            else
            {
                _decreasingRenderFidelity = false;
            }

            if (keyboardState.IsKeyDown(Keys.OemCloseBrackets))
            {
                if (!_increasingRenderFidelity)
                {
                    _increasingRenderFidelity = true;
                    _renderScale = _renderScale.IncreaseFidelity();
                    var newSize = ScreenSize.DivideBy((int)_renderScale);
                    UpdateScreenBuffer(newSize);
                }
            }
            else
            {
                _increasingRenderFidelity = false;
            }

            base.Update(gameTime);
        }

        void UpdateScreenBuffer(Size renderSize)
        {
            if (_screenBuffer.Dimensions != renderSize)
            {
                _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: renderSize.Width, height: renderSize.Height);
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
                    width: ScreenSize.Width,
                    height: ScreenSize.Height),
                color: Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
