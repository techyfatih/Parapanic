using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Parapanic.Minimap
{
    class Map
    {
        Texture2D mapTexture;

        //upon world modification, this flag is set.
        //this makes it so we don't have to generate the map
        //every frame;
        public static bool DirtyFlag = true;

        int maxTextureSize = 0;
        public int xScale = 0;
        public int yScale = 0;

        void CalculateMaxTextureSize(GraphicsDevice device)
        {
            maxTextureSize = 2;
            int i = 1;
            while (true)
            {
                try
                {
                    maxTextureSize = (int)Math.Pow(2, i);
                    Texture2D tex = new Texture2D(device, maxTextureSize, maxTextureSize);

                    tex.Dispose();
                }
                catch (Exception ex)
                {
                    maxTextureSize = (int)Math.Pow(2, i - 1);
                    break;
                }
                i++;
            }
        }

        public Texture2D GetMapTexture(Parapanic game, World world)
        {
            if (!DirtyFlag)
                return mapTexture;

            if (mapTexture != null)
                mapTexture.Dispose();

            if (maxTextureSize == 0)
                CalculateMaxTextureSize(game.GraphicsDevice);

            xScale = maxTextureSize * Block.size / world.Width;
            yScale = maxTextureSize * Block.size / world.Height;

            game.GraphicsDevice.Flush();
            RenderTarget2D render = new RenderTarget2D(game.GraphicsDevice, maxTextureSize, maxTextureSize);
            game.GraphicsDevice.SetRenderTarget(render);
            game.GraphicsDevice.Clear(Color.White);

            //NOTE(justin): we need our own spritebatch so that we don't end up drawing other stuff into our map
            SpriteBatch batch = new SpriteBatch(game.GraphicsDevice);
            batch.Begin();

            Texture2D texToDraw = game.Content.Load<Texture2D>("TestPicture"); //just using a test picture for now,
                                                                               //todo: replace test minimap picture with actual
                                                                               //pictures.
            foreach (Block b in world.grid)
            {
                if (b == null)
                    continue;

                Rectangle drawArea = new Rectangle();
                drawArea.X = ((int)b.position.X / Block.size) * xScale;
                drawArea.Y = ((int)b.position.Y / Block.size) * yScale;
                drawArea.Width = xScale;
                drawArea.Height = yScale;

                if (b.GetType().Equals(typeof(WallBlock)))
                {
                    batch.Draw(texToDraw, drawArea, Color.Green);
                }
                else if (b.GetType().Equals(typeof(FloorBlock)))
                {
                    batch.Draw(texToDraw, drawArea, Color.Blue);
                }
                else if (b.GetType().Equals(typeof(HospitalBlock)))
                {
                    batch.Draw(texToDraw, drawArea, Color.Yellow);
                }
                else if (b.GetType().Equals(typeof(PatientBlock)))
                {
                    batch.Draw(texToDraw, drawArea, Color.Pink);
                }

                //todo: fix weird issues on redraw
                //todo: other blocks
            }

            batch.End();
            game.GraphicsDevice.Flush();
            game.GraphicsDevice.SetRenderTarget(null);

            mapTexture = (Texture2D)render; //todo: look at speed impact of straight up casting this,
                                            //maybe there's some way to extract the data into a new object
                                            //or does it even matter?
            DirtyFlag = false;
            return mapTexture;
        }
    }
}
