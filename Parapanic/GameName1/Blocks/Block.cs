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
        public const int size = 128;
        public Vector2 position;
        public Rectangle boundary;
        public bool solid = false;
        public int carsInside = 0;

        public Block(int x, int y)
        {
            position = new Vector2(x, y);
            boundary = new Rectangle(x, y, size, size);
        }

        //used for 3D code
        public int depth = 0;

        public virtual void OnCollision(World world, Car car)
        {
        }
    }
}
