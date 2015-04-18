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
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("font"), "***TEXT HERE***", new Vector2(), Color.Black);
            Texture2D tex = Utilities.CheckCollision(m.Position, b) ? game.Content.Load<Texture2D>("menubutton_highlighted") : game.Content.Load<Texture2D>("menubutton");
            spriteBatch.Draw(tex, b, Color.White);
            base.Draw(spriteBatch, game);
        }
    }
}
