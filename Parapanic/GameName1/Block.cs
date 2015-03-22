using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parapanic
{
    abstract class Block
    {
        public static int size = 64;
        public Vector2 position;

        public Block(int x, int y)
        {
            position = new Vector2(x, y);
        }

        //used for 3D code
        public int depth;
    }
}
