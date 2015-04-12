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
        //VectorAmbulance vambulance;
        World world;
        Minimap.GPS minimap;

        int width;
        int height;

        public Level(GraphicsDeviceManager g)
        {
            width = g.PreferredBackBufferWidth;
            height = g.PreferredBackBufferHeight;


            world = new World(100, 100);
            Vector2 empty = world.EmptySpace();
            //ambulance = new Ambulance((int)empty.X + Block.size/2, (int)empty.Y + Block.size/2, 0, 10, 0.1, 0.95);
            ambulance = new Ambulance((int)empty.X + Block.size / 2, (int)empty.Y + Block.size / 2, 0, 7, 0.1, 0.95);
            Camera.Initialize(width, height);
            world.ambulance = ambulance;

            minimap = new Minimap.GPS(width, height, 2f, new Rectangle(width - 150, height - 120, 120, 90));
        }

        byte carUpdateTimer = 0;
        public void Update()
        {
            ambulance.Update(world);
            Camera.Update(ambulance, world);
            foreach (Car c in world.Cars)
                c.Update(world);

            if (carUpdateTimer++ > 60)
            {
                ReplaceCars();
                carUpdateTimer = 0;
            }

            if (Minimap.Map.DirtyFlag)
                foreach (Block block in world.grid)
                    if (block is RoadBlock)
                        ((RoadBlock)block).InitializeType(world);
        }

        const int numCarsOnMap = 160;
        void ReplaceCars()
        {
            for (int i = 0; i < world.Cars.Count; i++)
            {
                if ((ambulance.position - world.Cars[i].position).LengthSquared() > 1000000)
                {
                    world.Cars.Remove(world.Cars[i--]);
                }
            }

            List<Block> roadsInRange = new List<Block>();

            foreach (Block b in world.grid)
            {
                if (b is RoadBlock &&
                   (ambulance.position - b.position).LengthSquared() > 400000 &&
                   (ambulance.position - b.position).LengthSquared() < 800000 &&
                    b.carsInside == 0)
                    roadsInRange.Add(b);
            }

            for (int i = world.Cars.Count; i < numCarsOnMap; i++)
            {
                if (roadsInRange.Count == 0)
                    break;
                Block spawnBlock = roadsInRange[Parapanic.Random.Next(roadsInRange.Count)];
                roadsInRange.Remove(spawnBlock);

                world.Cars.Add(new AiCar((int)spawnBlock.position.X, (int)spawnBlock.position.Y,
                                         1, 0.05, 1, world));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            if (Minimap.Map.DirtyFlag)
                minimap.Draw(spriteBatch, game, world);
            Camera.DrawScreen(game, ambulance, world);

            if(ambulance.hasPatient)
            {
                spriteBatch.Draw(Textures.patientFace, new Rectangle(110, height - 200, 120, 160), Color.White);
                spriteBatch.Draw(Textures.black, new Rectangle(110, height - 200, 120, 160), Color.White * (ambulance.patientTimer/ambulance.maxTime));

                spriteBatch.Draw(Textures.white, new Rectangle(110, height - 25, 120, 25), Color.Red);
                spriteBatch.Draw(Textures.white, new Rectangle(110 + (int)(120 * (ambulance.patientTimer / ambulance.maxTime)), height - 25, 120 - (int)(120 * (ambulance.patientTimer / ambulance.maxTime)), 25), Color.Green);
            }

            minimap.Draw(spriteBatch, game, world);
        }
    }
}
