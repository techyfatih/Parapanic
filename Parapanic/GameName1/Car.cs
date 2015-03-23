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
        public float direction;
        public Vector2 origin;

        public double speed { get; protected set; }
        public double topSpeed { get; protected set; }
        protected double acceleration;
        protected double friction;
        public float scale { get; protected set; }

        protected double[] cornerAngles;
        protected double diagonal;

        protected const double MAX_TURN_RATE = 0.05;

        public Car(int x, int y, double speed, float direction, double topSpeed, double acceleration, double friction)
        {
            scale = 0.25f;
            int width = (int)(96*scale);
            int height = (int)(64*scale);

            position = new Vector2(x, y);
            this.direction = direction;
            origin = new Vector2(width / 2, height / 2);

            this.speed = speed;
            this.topSpeed = topSpeed;
            this.acceleration = acceleration;
            this.friction = friction;

            cornerAngles = new double[4]; //0: Front Left, 1: Front Right, 2: Rear Left, 3: Rear Right, 4: Hypotenuse
            cornerAngles[0] = Math.Atan((double)height / width);
            cornerAngles[1] = Math.Atan((double)height / -width) + 2*Math.PI;
            cornerAngles[2] = cornerAngles[1] + Math.PI;
            cornerAngles[3] = cornerAngles[0] + Math.PI;
            diagonal = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) / 2;
        }

        public virtual void Update(World world)
        {
            Vector2 speedV = new Vector2((float)(Math.Cos(direction)*speed), (float)(Math.Sin(direction)*speed));

            int[] newX = new int[4];
            int[] newY = new int[4];
            for (int i = 0; i < 4; i++)
            {
                cornerAngles[i] = Utilities.NormAngle(cornerAngles[i] + direction);
                newX[i] = (int)(Math.Cos(cornerAngles[i]) * diagonal + position.X + speedV.X);
                newY[i] = (int)(Math.Sin(cornerAngles[i]) * diagonal + position.Y + speedV.Y);
            }

            int boundX = Utilities.Minimum(newX);
            int boundY = Utilities.Minimum(newY);
            int boundWidth = Utilities.Maximum(newX) - boundX;
            int boundHeight = Utilities.Maximum(newY) - boundY;

            Rectangle boundingBox = new Rectangle(boundX, boundY, boundWidth, boundHeight);

            foreach (Block b in world.grid)
            {
                if (b.GetType().Equals(typeof(WallBlock)))
                {
                    if ((position.X > b.position.X && position.X < b.position.X + Block.size) && Utilities.CheckCollisionY(boundingBox, b.boundary))
                    {
                        speedV.Y = 0;
                        Console.WriteLine("X " + Math.Max(boundingBox.Left, b.boundary.Left) + " " + Math.Min(boundingBox.Right, b.boundary.Right));
                    }
                    if ((position.Y > b.position.Y && position.Y < b.position.Y + Block.size) && Utilities.CheckCollisionX(boundingBox, b.boundary))
                    {
                        speedV.X = 0;
                        Console.WriteLine("Y " + Math.Max(boundingBox.Left, b.boundary.Left) + " " + Math.Min(boundingBox.Right, b.boundary.Right));
                    }
                }
            }
            position.X += speedV.X;
            position.Y += speedV.Y;
        }
    }
}
