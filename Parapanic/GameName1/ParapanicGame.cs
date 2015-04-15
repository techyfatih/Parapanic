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
    public class Parapanic : Game
    {
        public static Random Random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level realLevel;

        Level level1;
        Level level2;
        Level level3;

        Menu menu;

        public int gameState;
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
            menu = new Menu(GraphicsDevice,this);

            level1 = new Level(graphics);
            level2 = new Level(graphics);
            level3 = new Level(graphics);

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

            if (IsActive && gameState == 0)
            {
                menu.Update();
                base.Update(gameTime);
            }

            if (IsActive && gameState == 2) //Primitive way to pause the game on minimize, would like to improve later
            {
                level1.Update();
                if(level1.ambulance.toMenu)
                {
                    gameState = 0;
                    level1.ambulance.toMenu = false;
                }
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
            if(gameState == 0)
            { 
                menu.Draw(spriteBatch, this);
            }
            if (gameState == 1)
            {
                level1.Draw(spriteBatch, this);
            }

            if (gameState == 1)
            {
                spriteBatch.Draw(level1.minimap.map.GetMapTexture(),)
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
