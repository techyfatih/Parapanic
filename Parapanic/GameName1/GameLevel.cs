﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class GameLevel : Level
    {
        Parapanic game;
        public Ambulance ambulance;
        //VectorAmbulance vambulance;
        public World world;
        public Minimap.GPS minimap;
        public string Name;
        public string PatientOneName;
        public string PatientTwoName;
        public Color Color = Color.Red;

        int width;
        int height;

        Color[] colors = { Color.Blue, Color.White, Color.Red, Color.Green, Color.Yellow };
        Random r = new Random();

        public GameLevel(GraphicsDevice g, Parapanic game)
        {
            this.game = game;
            width = g.Viewport.Width;
            height = g.Viewport.Height;


            world = new World(100, 100, this);
            Vector2 empty = world.EmptySpace();
            //ambulance = new Ambulance((int)empty.X + Block.size/2, (int)empty.Y + Block.size/2, 0, 10, 0.1, 0.95);
            ambulance = new Ambulance((int)empty.X + Block.size / 2, (int)empty.Y + Block.size / 2, 0, 7, 0.1, 0.95, game);
            Camera.Initialize(width, height);
            world.ambulance = ambulance;

            minimap = new Minimap.GPS(width, height, 2f, new Rectangle(width - 150, height - 120, 120, 90));
        }

        byte carUpdateTimer = 0;
        public override void Update()
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.Level = new PauseMenu(game.GraphicsDevice, game, this);
            }

            if (ambulance.lost)
            {
                game.Level = new LoseScreen(game.GraphicsDevice, game, 0, "Leeroy Jenkins");
            }
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

        public override void Draw(SpriteBatch spriteBatch, Parapanic game)
        {
            SpriteFont font = game.Content.Load<SpriteFont>("font");
            if (Minimap.Map.DirtyFlag)
                minimap.Draw(spriteBatch, game, world);
            Camera.DrawScreen(game, ambulance, world);

            if(ambulance.hasPatient)
            {
                spriteBatch.Draw(Textures.patientFace, new Rectangle(110, height - 200, 120, 160), Color.White);
                spriteBatch.Draw(Textures.black, new Rectangle(110, height - 200, 120, 160), Color.White * ((float)ambulance.patientTimer/ambulance.maxTime));

                spriteBatch.Draw(Textures.white, new Rectangle(110, height - 25, 120, 25), Color.Red);
                spriteBatch.Draw(Textures.white, new Rectangle(110 + (int)(120 * ((float)ambulance.patientTimer / ambulance.maxTime)), height - 25, 120 - (int)(120 * ((float)ambulance.patientTimer / ambulance.maxTime)), 25), Color.Green);
            }

            spriteBatch.Draw(Textures.white, new Rectangle(15, 15, 200, 30), Color.DarkGray);
            spriteBatch.DrawString(font, "Score : " + game.Score, new Vector2(20, 15), Color.White);

            minimap.Draw(spriteBatch, game, world);
        }
    }
}
