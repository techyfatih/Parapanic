using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Parapanic
{
    class Textures
    {
        public static Texture2D ambulance;
        public static Texture2D aicar;
        public static Texture2D floor;
        public static Texture2D wall;
        public static Texture2D patient;
        public static Texture2D hospital;
        public static Texture2D patientFace;
        public static Texture2D black;
        public static Texture2D white;
        public static Texture2D testMenu;
        public static Texture2D startButton;
        public static Texture2D startButton_highlighted;
        public static Texture2D continueButton;
        public static Texture2D continueButton_highlighted;
        public static Texture2D infoButton;
        public static Texture2D infoButton_highlighted;
        public static Texture2D quitButton;
        public static Texture2D quitButton_highlighted;
        public static Texture2D loseScreen;
        public static SpriteFont font1;

        public static void LoadContent(ContentManager Content)
        {
            ambulance = Content.Load<Texture2D>("Ambulance");
            aicar = Content.Load<Texture2D>("AiCar");
            floor = Content.Load<Texture2D>("floor");
            wall = Content.Load<Texture2D>("building");
            patient = Content.Load<Texture2D>("Patient");
            hospital = Content.Load<Texture2D>("Hospital");
            patientFace = Content.Load<Texture2D>("PatientPlaceholder");
            black = Content.Load<Texture2D>("Black");
            white = Content.Load<Texture2D>("white");
            testMenu = Content.Load<Texture2D>("TestMenuBackground");
            startButton = Content.Load<Texture2D>("StartButton");
            startButton_highlighted = Content.Load<Texture2D>("StartButton_highlighted");
            continueButton = Content.Load<Texture2D>("ContinueButton");
            continueButton_highlighted = Content.Load<Texture2D>("ContinueButton_highlighted");
            infoButton = Content.Load<Texture2D>("infoButton");
            infoButton_highlighted = Content.Load<Texture2D>("infoButton_highlighted");
            quitButton = Content.Load<Texture2D>("quitButton");
            quitButton_highlighted = Content.Load<Texture2D>("quitButton_highlighted");
            loseScreen = Content.Load<Texture2D>("loseScreen");
            font1 = Content.Load<SpriteFont>("font");
        }
    }
}
