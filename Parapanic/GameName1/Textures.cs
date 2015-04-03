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
        public static Texture2D floor;
        public static Texture2D wall;
        public static Texture2D patient;
        public static Texture2D hospital;

        public static void LoadContent(ContentManager Content)
        {
            ambulance = Content.Load<Texture2D>("Ambulance");
            floor = Content.Load<Texture2D>("floor");
            wall = Content.Load<Texture2D>("building");
            patient = Content.Load<Texture2D>("Patient");
            hospital = Content.Load<Texture2D>("Hospital");
        }
    }
}
