using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Parapanic
{
    class Car
    {
        public Vector2 position;
        public float rotation;
        public Vector2 origin;

        protected Rectangle carRect;
        protected double topSpeed;
        protected double acceleration;
        protected double direction;
        protected double friction;
        protected double mouseDirection;
        protected double speed;
        protected const double maxTurnrate = 3;

        public virtual void Update(GameTime gameTime, World world)
        {
            Vector2 a = new Vector2((float)Math.Sin(MathHelper.ToRadians((float)direction)),
                                    (float)Math.Cos(MathHelper.ToRadians((float)direction)));
            a.Normalize();

            float newY = position.Y + (int)(a.X * speed);
            float newX = position.X + (int)(a.Y * speed);
            foreach (Block b in world.grid)
            {
                if (b.GetType().Equals(typeof(WallBlock)))
                {
                    if (newY >= b.position.Y && newY <= b.position.Y + Block.size)
                        if (newX >= b.position.X && newX <= b.position.X + Block.size)
                            newX = position.X;
                    if (newX >= b.position.X && newX <= b.position.X + Block.size)
                        if (newY >= b.position.Y && newY <= b.position.Y + Block.size)
                            newY = position.Y;
                }
            }
            position.Y = newY;
            position.X = newX;
            carRect.Y = (int)newY;
            carRect.X = (int)newX;
            rotation = MathHelper.ToRadians((float)direction);
        }
    }
}
