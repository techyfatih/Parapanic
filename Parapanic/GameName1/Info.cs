using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Parapanic
{
    class Info : Level
    {
        Rectangle b = new Rectangle(450, 500, 100, 50);
        MouseState m;

        Parapanic game;
        public Info(Parapanic game)
        {
            this.game = game;
            m = Mouse.GetState();
        }

        public override void Update()
        {
            if (m.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released && Utilities.CheckCollision(Mouse.GetState().Position, b))
                game.Level = new Menu(game.GraphicsDevice, game);
            base.Update();

            m = Mouse.GetState();
        }

        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            game.GraphicsDevice.Clear(Color.White);
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("font"), "Collect patients at the ! icons\n you can only pick up one at a time Once you have a patient, race to the hospital, which is denoted\n by the H icon, and save the other one\n There are two patients per level, after which you will given a choice between three new levels\nThere are " +  game.numLevels + " levels\nYou lose if you fail to save a single patient\n\nParapanic is a game that is meant to celebrate everyday heroes, ambulance drivers.\nIt is all about the stresses of their every moment, and that someone's life\nis contingent on their actions. Parapanic will place the player in their choice\nof randomly generated city, and task them with saving both patients in each city.\nCan you save everyone?\n\n\nCredits:\nAndrei Blebea\nJustin Mellott\nFatih Ridha\nRohan Guliani\nSatvir Singh", new Vector2(), Color.Black);

            Texture2D tex = Utilities.CheckCollision(m.Position, b) ? game.Content.Load<Texture2D>("menubutton_highlighted") : game.Content.Load<Texture2D>("menubutton");
            spriteBatch.Draw(tex, b, Color.White);
            base.Draw(spriteBatch, game);
        }
    }
}
