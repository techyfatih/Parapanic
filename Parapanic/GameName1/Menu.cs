using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class Menu
    {

        //public Texture2D background;
        int width;
        int height;

        int buttonWidth = 200;
        int buttonHeight = 75;

        Parapanic game;

        Button startButton;
        Button continueButton;
        Button infoButton;
        Button creditsButton;
        Button quitButton;

        public Menu(GraphicsDevice g, Parapanic game1)
        {
            //background = Textures.testMenu;
            width = g.Viewport.Width;
            height = g.Viewport.Height;

            game = game1;

            startButton = new Button((int)((width / 2f) - (buttonWidth/2f)), (int)(height * .5),buttonWidth, buttonHeight);
            continueButton = new Button((int)((width / 2f) - (buttonWidth / 2f)), (int)(height * .3), buttonWidth, buttonHeight);

        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();

            if(startButton.highlighted() && mouse.LeftButton == ButtonState.Pressed && !game.inProgress)
            {
                game.gameState = 1;
                game.inProgress = true;
            }

            if (continueButton.highlighted() && mouse.LeftButton == ButtonState.Pressed && game.inProgress)
            {
                game.gameState = 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Parapanic game)
        {

            spriteBatch.Draw(Textures.testMenu,new Rectangle(0,0,width,height),Color.White);

            spriteBatch.Draw(!startButton.highlighted()?Textures.startButton:Textures.startButton_highlighted, startButton.rectangle, Color.White);

            spriteBatch.Draw(!continueButton.highlighted() ? Textures.continueButton : Textures.continueButton_highlighted, continueButton.rectangle, game.inProgress?Color.White:Color.Gray);

        }

    }
}
