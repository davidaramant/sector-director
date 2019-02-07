﻿using Microsoft.Xna.Framework;
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
        ScreenBuffer _screenBuffer;

        private Size ScreenSize { get; set; } = new Size(640, 480);
        private Size RenderSize => ScreenSize.Quarter().Quarter();

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

            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: RenderSize.Width, height: RenderSize.Height);
            _screenBuffer = new ScreenBuffer(RenderSize);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var currentScreenSize = new Size(
                width: GraphicsDevice.PresentationParameters.BackBufferWidth,
                height: GraphicsDevice.PresentationParameters.BackBufferHeight);

            if(ScreenSize != currentScreenSize)
            {
                _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: RenderSize.Width, height: RenderSize.Height);
                _screenBuffer = new ScreenBuffer(RenderSize);
            }
                        
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointWrap,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            _screenBuffer[0,0] = Color.AliceBlue;

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
