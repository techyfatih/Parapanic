using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Parapanic.Minimap
{
    class GPS
    {
        int gamePortWidth;
        int gamePortHeight;
        float gamePortScale;
        Rectangle screenPort;

        Map map;

        public GPS(int gamePortWidth, int gamePortHeight, float gamePortScale, Rectangle screenPort)
        {
            this.gamePortHeight = gamePortHeight;
            this.gamePortWidth = gamePortWidth;
            this.gamePortScale = gamePortScale;
            this.screenPort = screenPort;
            map = new Map();
        }

        public void Draw(SpriteBatch batch, Parapanic game, World world)
        {
            Rectangle gameBounds = new Rectangle();
            gameBounds.X = (int)(Camera.position.X - gamePortWidth * (gamePortScale - gamePortScale / 2));
            gameBounds.Y = (int)(Camera.position.Y - gamePortHeight * (gamePortScale - gamePortScale / 2));
            gameBounds.Width = (int)(gamePortWidth * (gamePortScale + 1));
            gameBounds.Height = (int)(gamePortHeight * (gamePortScale + 1));

            batch.Draw(map.GetMapTexture(game, world), screenPort, gameBounds, Color.White);
        }
    }
}
