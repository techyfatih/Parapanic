﻿using System;
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
        //VectorAmbulance vambulance;
        World world;
        Minimap.GPS minimap;

        public Level(GraphicsDeviceManager g)
        {
            int width = g.PreferredBackBufferWidth;
            int height = g.PreferredBackBufferHeight;

            world = new World(100, 100);
            Vector2 empty = world.EmptySpace();
            //ambulance = new Ambulance((int)empty.X + Block.size/2, (int)empty.Y + Block.size/2, 0, 10, 0.1, 0.95);
            ambulance = new Ambulance((int)empty.X + Block.size / 2, (int)empty.Y + Block.size / 2, 0, 10, 0.1, 0.95);
            Camera.Initialize(width, height);

            minimap = new Minimap.GPS(width, height, 2f, new Rectangle(width - 150, height - 120, 120, 90));
        }

        public void Update()
        {
            ambulance.Update(world);
            Camera.Update(ambulance, world);
            foreach (Car c in world.Cars)
                c.Update(world);

            if (Minimap.Map.DirtyFlag)
                foreach (Block block in world.grid)
                    if (block is RoadBlock)
                        ((RoadBlock)block).InitializeType(world);
        }

        public void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            if (Minimap.Map.DirtyFlag)
                minimap.Draw(spriteBatch, game, world);
            Camera.DrawScreen(game, ambulance, world);
            minimap.Draw(spriteBatch, game, world);
        }
    }
}
