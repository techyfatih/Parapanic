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
        Rectangle screenPort;

        Map map;
        
        public bool DirtyFlag
        {
            get { return map.DirtyFlag; }
            set { map.DirtyFlag = value; }
        }

        public GPS(int gamePortWidth, int gamePortHeight, Rectangle screenPort)
        {
            this.gamePortHeight = gamePortHeight;
            this.gamePortWidth = gamePortWidth;
            this.screenPort = screenPort;
            map = new Map();
        }

        public void Draw(SpriteBatch batch, Parapanic game, World world)
        {
            batch.Begin();

            Rectangle gameBounds = new Rectangle();
            gameBounds.X = (int)Camera.position.X;
            gameBounds.Y = (int)Camera.position.Y;
            gameBounds.Width = gamePortWidth;
            gameBounds.Height = gamePortHeight;

            batch.Draw(map.GetMapTexture(game, world), screenPort, gameBounds, Color.White);

            batch.End();
        }
    }
}
