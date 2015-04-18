using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class WinScreen:Level
    {
        int saved;

        int width;
        int height;

        int score;

        public WinScreen(GraphicsDevice g, Parapanic game1, int saved)
        {
            this.saved = saved;

            width = g.Viewport.Width;
            height = g.Viewport.Height;

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            spriteBatch.Draw(Textures.winScreen, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.DrawString(Textures.font1, "Your score was: " + score, new Vector2(300, 400), Color.Red);
            spriteBatch.DrawString(Textures.font1, "You saved: " + saved + " people", new Vector2(300, 450), Color.Red);

            base.Draw(spriteBatch, game);
        }

    }
}
