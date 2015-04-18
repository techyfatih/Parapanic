#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace Parapanic
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Parapanic : Game
    {
        public enum State
        {
            Menu, Game, Pause, PickALevel, Lose
        }

        public static Random Random = new Random();
        public int Score = 0;
        public int ScoreMultiplier = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Level Level;

        public State gameState;
        public bool inProgress = false;

        public Parapanic()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
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
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            Level = new Menu(GraphicsDevice, this);
            //menu = new Menu(GraphicsDevice,this);

            //level1 = new Level(graphics);
            //level2 = new Level(graphics);
            //level3 = new Level(graphics);

            gameState = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Content.Load<SpriteFont>("font");
            Content.Load<Texture2D>("TestPicture");
            Textures.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/ Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            if (IsActive)
            {
                Level.Update();
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            Level.Draw(spriteBatch, this);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
