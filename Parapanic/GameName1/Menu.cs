using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class Menu : Level
    {

        //public Texture2D background;
        int width;
        int height;

        int buttonWidth = 200;
        int buttonHeight = 75;

        Parapanic game;

        Button startButton;
        Button infoButton;
        Button creditsButton;
        Button quitButton;

        public Menu(GraphicsDevice g, Parapanic game1)
        {
            //background = Textures.testMenu;
            width = g.Viewport.Width;
            height = g.Viewport.Height;

            game = game1;

            startButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .4), buttonWidth, buttonHeight);
            infoButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .6), buttonWidth, buttonHeight);
            quitButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .8), buttonWidth, buttonHeight);
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();

            if(startButton.highlighted() && mouse.LeftButton == ButtonState.Pressed)
            {
                game.gameState = Parapanic.State.PickALevel;
                game.Level = new PickALevel(game);
            }

            if (quitButton.highlighted() && mouse.LeftButton == ButtonState.Pressed)
            {
                game.Exit();
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {

            spriteBatch.Draw(Textures.testMenu,new Rectangle(0,0,width,height),Color.White);

            spriteBatch.Draw(!startButton.highlighted()?Textures.startButton:Textures.startButton_highlighted, startButton.rectangle, Color.White);
            
            spriteBatch.Draw(!infoButton.highlighted() ? Textures.infoButton : Textures.infoButton_highlighted, infoButton.rectangle, Color.White);

            spriteBatch.Draw(!quitButton.highlighted() ? Textures.quitButton : Textures.quitButton_highlighted, quitButton.rectangle, Color.White);

        }

    }

    class PauseMenu : Level
    {
        //public Texture2D background;
        int width;
        int height;

        int buttonWidth = 200;
        int buttonHeight = 75;

        Parapanic game;
        Level levelToReturnTo;

        Button startButton;
        Button continueButton;
        Button infoButton;
        Button creditsButton;
        Button quitButton;

        public PauseMenu(GraphicsDevice g, Parapanic game1, Level currentLevel)
        {
            //background = Textures.testMenu;
            width = g.Viewport.Width;
            height = g.Viewport.Height;
            levelToReturnTo = currentLevel;

            game = game1;

            continueButton = new Button((int)((width / 2f) - (buttonWidth/2f)), (int)(height * .4),buttonWidth, buttonHeight);
            //continueButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .2), buttonWidth, buttonHeight);
            infoButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .6), buttonWidth, buttonHeight);
            quitButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .8), buttonWidth, buttonHeight);
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();
            
            if (continueButton.highlighted() && mouse.LeftButton == ButtonState.Pressed)
            {
                game.gameState = Parapanic.State.Game;
                game.Level = levelToReturnTo;
            }

            if (quitButton.highlighted() && mouse.LeftButton == ButtonState.Pressed)
            {
                game.Exit();
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {

            spriteBatch.Draw(Textures.testMenu,new Rectangle(0,0,width,height),Color.White);

            spriteBatch.Draw(!continueButton.highlighted() ? Textures.continueButton : Textures.continueButton_highlighted, continueButton.rectangle, Color.White);

            spriteBatch.Draw(!infoButton.highlighted() ? Textures.infoButton : Textures.infoButton_highlighted, infoButton.rectangle, Color.White);

            spriteBatch.Draw(!quitButton.highlighted() ? Textures.quitButton : Textures.quitButton_highlighted, quitButton.rectangle, Color.White);

        }
    }
}
