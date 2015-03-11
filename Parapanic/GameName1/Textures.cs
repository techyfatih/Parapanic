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
        public static Texture2D block;

        public static void LoadContent(ContentManager Content)
        {
            ambulance = Content.Load<Texture2D>("Ambulance.png");
            block = Content.Load<Texture2D>("floor.png");
        }
    }
}
