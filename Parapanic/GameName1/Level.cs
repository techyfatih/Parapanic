using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parapanic
{
    class Level
    {
        Ambulance ambulance;
        World world;
        Minimap.GPS minimap;

        public Level(GraphicsDeviceManager g)
        {
            int width = g.PreferredBackBufferWidth;
            int height = g.PreferredBackBufferHeight;

            ambulance = new Ambulance(200, 200, 0, 10, 0.1, 0.95);
            world = new World(25, 25);
            Camera.Initialize(width, height);

            minimap = new Minimap.GPS(width, height, new Rectangle(width - 150, height - 120, 120, 90));
        }

        public void Update()
        {
            ambulance.Update(world);
            Camera.Update(ambulance, world);
        }

        public void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            minimap.Draw(spriteBatch, game, world); //fixes screen flicker when the map is changed.
                                                    //will make a better solution later
            Camera.DrawScreen(game, ambulance, world);
            minimap.Draw(spriteBatch, game, world);
        }
    }
}
