using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class LoseScreen:Level
    {
        int saved;
        string patientLost;

        int width;
        int height;

        int score;

        public LoseScreen(GraphicsDevice g, Parapanic game1, int saved, string patientLost, int score)
        {
            this.saved = saved;
            this.patientLost = patientLost;

            width = g.Viewport.Width;
            height = g.Viewport.Height;

            this.score = score;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            spriteBatch.Draw(Textures.loseScreen, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.DrawString(Textures.font1, "Your score was: " + score, new Vector2(300,400), Color.Red);

            base.Draw(spriteBatch, game);
        }
    }
}
