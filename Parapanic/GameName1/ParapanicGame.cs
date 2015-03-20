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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ambulance ambulance;
        World world;
        Minimap.GPS Minimap;

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
            int width = graphics.PreferredBackBufferWidth;
            int height = graphics.PreferredBackBufferHeight;

            this.IsMouseVisible = true;
            ambulance = new Ambulance(200, 200, 0, 10, 0.1, 0.95);
            world = new World(25, 25);
            Camera.Initialize(width, height);

            Minimap = new Minimap.GPS(width, height, new Rectangle(width - 150, height - 120, 120, 90));
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
            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/ Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (IsActive) //Primitive way to pause the game on minimize, would like to improve later
            {
                ambulance.Update(gameTime, world);
                Camera.Update(gameTime, ambulance, world);
                
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

            Camera.DrawScreen(spriteBatch, ambulance, world);
            Minimap.Draw(spriteBatch, this, world);

            base.Draw(gameTime);
        }
    }
}
