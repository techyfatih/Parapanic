using Microsoft.Xna.Framework;
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
        readonly string[] PatientFirstNames = 
        {
            "John", "Michael", "Andrew", "Andrei", "Liam", "Noah", "Ethan", "Mason", "Bran", 
            "Logan", "Lucas", "Mateo", "Luis", "Daniel", "Diego", "Muhammad", "Elijah", "Patrick",
            "Eliott", "Justin", "Fatih", "Rohan", "Sebastian", "Jack", "Jeremiah", "Amir", "Richie",
            "Ishaan", "Ashwin", "Kyle", "Richard", "Arman", "Thomas", "William", "Jake", "Eli",
            "Gabriel", "Rafael", "Juan", "Samuel", "Carlos", "Ian", "Dylan", "Zack", "Rob", "Ned",

            "Alex", "Jordan", "Sam", "Bailey", "Taylor", 

            "Kate", "Alexis", "Olivia", "Sophia", "Emma", "Emily", "Chloe", "Ava", "Ashley",
            "Leah", "Isabella", "Maria", "Zoe", "Grace", "Anya", "Arya", "Sansa", "Madison",
            "London", "Mary", "Andrea", "Patricia", "Linda", "Barbara", "Nicole", "Natalia",
            "Natalie", "Shreya", "April", "Abril", "May", "Hannah", "Maya", "Kayla",
            "Victoria", "Samantha",  "Alexa", "Lily", "Bella", "Ella", "Rosalia", "Charlotte",
        };
        readonly string[] PatientLastNames =
        {
            "Smith", "Johnson", "Williams", "Martinez", "Brown", "Jones", "Watson",
            "Miller", "Stark", "Davis", "Anderson", "Lee", "Gonzalez", "Lewis",
            "Clark", "Perez", "Allen", "King", "Campbell", "Cook", "Rivera", "Bailey",
            "Cooper", "Bennett", "Ward", "Diaz", "Henderson", "Russo", "Rich", "Noble",
            "Donovan", "Zuniga", "Harding", "Yu", "Woodward", "Costa", "Stanton", "Bonilla",
        };

        readonly string[] CityNameFirst =
        {
            "Hill", "Beaver", "Lake", "Wilson", "Dimm", "Wheat", "Peter", "Jackson",
            "Washing", "Spring", "Chester", "Madison", "George", "Ash", "Burling", "Center",
            "Clay", "Day", "Lexing", "Will", "Cleve", "New",
        };
        readonly string[] CityNameLast =
        {
            "sboro", "ton", "sville", "town", "burg", "sdale", "opolis", "field", 
            "view", "land", "ford", "burn", "side", "port",
        };

        Parapanic game;
        public Ambulance ambulance;
        //VectorAmbulance vambulance;
        public World world;
        public Minimap.GPS minimap;
        public string Name;
        public string PatientOneName; //These patient names should probably be moved into patientblock, but due to how our code is set
        public string PatientTwoName; //up its way faster to do it this way. 
        public Color Color = Color.Red;

        int numCarsOnMap;

        int width;
        int height;

        Color[] colors = { Color.Blue, Color.White, Color.Red, Color.Green, Color.Yellow };
        Random r;// = new Random();

        Rectangle pause = new Rectangle(925, 25, 50, 50);

        public GameLevel(GraphicsDevice g, Parapanic game)
        {


            r = Parapanic.Random;
            this.game = game;

            numCarsOnMap = game.maxCars/(game.numLevels - game.ScoreMultiplier + 1);


            width = g.Viewport.Width;
            height = g.Viewport.Height;

            Name = CityNameFirst[r.Next(CityNameFirst.Length)]
                 + CityNameLast[r.Next(CityNameLast.Length)];

            PatientOneName = PatientFirstNames[r.Next(PatientFirstNames.Length)] + " "
                           + PatientLastNames[r.Next(PatientLastNames.Length)];
            PatientTwoName = PatientFirstNames[r.Next(PatientFirstNames.Length)] + " "
                           + PatientLastNames[r.Next(PatientLastNames.Length)];

            switch (game.ScoreMultiplier)
            {
                case 1:
                case 2:
                case 3: 
                        Color = Color.LightGray; break;
                case 4:
                case 5: 
                        Color = new Color(255, 96, 96); break;
                case 6: 
                case 7: 
                        Color = Color.Orange; break;
                case 8: Color = Color.DarkGoldenrod; break;
                case 9: Color = Color.Yellow; break;
                case 10: Color = Color.LightGreen; break;
            }

            Color.R = (byte)MathHelper.Clamp(r.Next(-64, 65) + Color.R, 0, 255);
            Color.G = (byte)MathHelper.Clamp(r.Next(-64, 65) + Color.G, 0, 255);
            Color.B = (byte)MathHelper.Clamp(r.Next(-64, 65) + Color.B, 0, 255);

            world = new World(100, 100, this);
            Vector2 empty = world.EmptySpace();
            //ambulance = new Ambulance((int)empty.X + Block.size/2, (int)empty.Y + Block.size/2, 0, 10, 0.1, 0.95);
            ambulance = new Ambulance((int)empty.X + Block.size / 2, (int)empty.Y + Block.size / 2, 0, 7, 0.1, 0.95, game);
            Camera.Initialize(width, height);
            world.ambulance = ambulance;

            minimap = new Minimap.GPS(width, height, 2f, new Rectangle(width - 150, height - 120, 120, 90));
            oldM = Mouse.GetState();
        }

        byte carUpdateTimer = 0;
        MouseState oldM;
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

            MouseState m = Mouse.GetState();
            if (m.LeftButton == ButtonState.Released && oldM.LeftButton == ButtonState.Pressed && Utilities.CheckCollision(m.Position, pause))
            {
                game.Level = new PauseMenu(game.GraphicsDevice, game, this);
            }

            if (ambulance.lost)
            {
                game.Level = new LoseScreen(game.GraphicsDevice, game, ambulance.patientsSaved, (ambulance.patientsSaved == 1) ? (PatientTwoName) : (PatientOneName),Name);
            }

            if (ambulance.won)
            {
                game.Level = new WinScreen(game.GraphicsDevice, game, ambulance.patientsSaved);
            }
            oldM = m;
        }

       //const int numCarsOnMap = game.maxCars/(game.numLevels - game.ScoreMultiplier + 1);

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

                spriteBatch.DrawString(Textures.font1, (ambulance.patientsSaved == 0) ? (PatientOneName) : (PatientTwoName), new Vector2(120, height - 235), Color.White);
            }

            spriteBatch.Draw(Textures.white, new Rectangle(15, 15, 200, 30), Color.DarkGray);
            spriteBatch.DrawString(font, "Score : " + game.Score, new Vector2(20, 15), Color.White);

            minimap.Draw(spriteBatch, game, world);

            spriteBatch.Draw(game.Content.Load<Texture2D>("pause"), pause, Color.White);
        }
    }
}
