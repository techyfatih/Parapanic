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

            Texture2D gpsBackground = game.Content.Load<Texture2D>("gps");
            
            Rectangle backgroundBounds = screenPort;
            backgroundBounds.X -= 10;
            backgroundBounds.Width += 20;
            backgroundBounds.Y -= 50;
            backgroundBounds.Height += 60;
            batch.Draw(gpsBackground, backgroundBounds, Color.White);
            batch.Draw(map.GetMapTexture(game, world), screenPort, gameBounds, Color.White);

            //we probably don't need to keep reloading this texture every frame.
            //todo: look into speed impacts of reloading this vs memory impact of keeping a reference
            Texture2D alertTex = game.Content.Load<Texture2D>("alert");
            Vector2 origin = new Vector2(alertTex.Width/2, alertTex.Height/2);

            foreach (Vector2 i in world.pointsOfInterest)
            {
                Vector2 posToDraw = new Vector2();
                posToDraw.X = MathHelper.Clamp(i.X - gameBounds.X, 0, gameBounds.Width) / gameBounds.Width * screenPort.Width;
                posToDraw.Y = MathHelper.Clamp(i.Y - gameBounds.Y, 0, gameBounds.Height) / gameBounds.Height * screenPort.Height;
                posToDraw += screenPort.Location.ToVector2();

                batch.Draw(alertTex, posToDraw, null, Color.White, 0, origin, .25f, SpriteEffects.None, 0);
            }

        }
    }
}
