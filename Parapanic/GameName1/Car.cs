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
        protected bool frictionEnabled = false;
        protected float crashFriction = 0.97f;
        public float scale { get; protected set; }
        protected int width;
        protected int height;

        protected double[] cornerAngles;
        protected double diagonal;

        protected bool collisionLeft = false;
        protected bool collisionRight = false;
        protected bool collisionUp = false;
        protected bool collisionDown = false;
        public Rectangle boundingBox;
        
        public Car(int x, int y, double speed, float direction, double topSpeed, double acceleration, double friction)
        {
            scale = 0.25f;
            width = (int)(96*scale);
            height = (int)(64*scale);

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
            diagonal = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) / 2.5;
        }

        protected Rectangle RecalculateBoundingBox(Vector2 speedV = new Vector2())
        {
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

            return new Rectangle(boundX, boundY, boundWidth, boundHeight);
        }

        public virtual void Update(World world)
        {
            //friction!!
            if (frictionEnabled)
                speed *= friction;
            
            Vector2 speedV = new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));

            boundingBox = RecalculateBoundingBox(speedV);

            bool topLeftCollision = false;
            bool topMiddleCollision = false;
            bool bottomLeftCollision = false;
            bool bottomMiddleCollision = false;
            bool topRightCollision = false;
            bool rightMiddleCollision = false;
            bool bottomRightCollision = false;
            bool leftMiddleCollision = false;

            for (int x = 0; x < world.grid.GetLength(0); x++)
            {
                for (int y = 0; y < world.grid.GetLength(1); y++)
                {
                    if (world.grid[x, y].solid && boundingBox.Intersects(world.grid[x,y].boundary))
                    {
                        OnCollision(world, x, y);
                        speedV *= crashFriction;

                        topLeftCollision = 
                            Utilities.CheckCollision(boundingBox.Location, world.grid[x, y].boundary) || topLeftCollision;
                        bottomLeftCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X, boundingBox.Y + boundingBox.Height), world.grid[x, y].boundary) || bottomLeftCollision;
                        topRightCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y), world.grid[x, y].boundary) || topRightCollision;
                        bottomRightCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height), world.grid[x, y].boundary) || bottomRightCollision;
                        
                        bottomMiddleCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width/2, boundingBox.Y + boundingBox.Height),
                                                     world.grid[x, y].boundary) || bottomMiddleCollision;
                        topMiddleCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y),
                                                     world.grid[x, y].boundary) || topMiddleCollision;
                        rightMiddleCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height/2),
                                                     world.grid[x, y].boundary) || rightMiddleCollision;
                        leftMiddleCollision =
                            Utilities.CheckCollision(new Point(boundingBox.X, boundingBox.Y + boundingBox.Height/2),
                                                     world.grid[x, y].boundary) || leftMiddleCollision;
                        

                    }
                    else if (boundingBox.Intersects(world.grid[x,y].boundary))
                    {
                        OnCollision(world, x, y);
                    }
                }
            }
            for (int i = 0; i < world.Cars.Count; i++)
            {
                if (boundingBox.Intersects(world.Cars[i].boundingBox))
                {
                    speedV *= crashFriction;

                    topLeftCollision =
                        Utilities.CheckCollision(boundingBox.Location, world.Cars[i].boundingBox) || topLeftCollision;
                    bottomLeftCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X, boundingBox.Y + boundingBox.Height), world.Cars[i].boundingBox) || bottomLeftCollision;
                    topRightCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y), world.Cars[i].boundingBox) || topRightCollision;
                    bottomRightCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height), world.Cars[i].boundingBox) || bottomRightCollision;

                    bottomMiddleCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height),
                                                 world.Cars[i].boundingBox) || bottomMiddleCollision;
                    topMiddleCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y),
                                                 world.Cars[i].boundingBox) || topMiddleCollision;
                    rightMiddleCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height / 2),
                                                 world.Cars[i].boundingBox) || rightMiddleCollision;
                    leftMiddleCollision =
                        Utilities.CheckCollision(new Point(boundingBox.X, boundingBox.Y + boundingBox.Height / 2),
                                                 world.Cars[i].boundingBox) || leftMiddleCollision;


                }
            }

            bool leftCollision = (topLeftCollision && bottomLeftCollision) || 
                (topLeftCollision && leftMiddleCollision) || 
                (bottomLeftCollision && leftMiddleCollision);
            bool rightCollision = (topRightCollision && bottomRightCollision) ||
                (topRightCollision && rightMiddleCollision) ||
                (bottomRightCollision && rightMiddleCollision);
            bool topCollision = (topRightCollision && topLeftCollision) ||
                (topRightCollision && topMiddleCollision) ||
                (topLeftCollision && topMiddleCollision);
            bool bottomCollision = (bottomRightCollision && bottomLeftCollision) ||
                (bottomLeftCollision && bottomMiddleCollision) ||
                (bottomRightCollision && bottomMiddleCollision);

            if (leftCollision)
                speedV.X = Math.Max(0, speedV.X);
            if (rightCollision)
                speedV.X = Math.Min(0, speedV.X);
            if (topCollision)
                speedV.Y = Math.Max(0, speedV.Y);
            if (bottomCollision)
                speedV.Y = Math.Min(0, speedV.Y);


            
            position.X += Utilities.Round(speedV.X, 2);
            position.Y += Utilities.Round(speedV.Y, 2);

            /*Console.WriteLine(position.X + " " + position.Y);
            Console.WriteLine(speedV.X + " " + speedV.Y);
            Console.WriteLine(Utilities.Round(speedV.X, 3) + " " + Utilities.Round(speedV.Y, 3));*/
            

            speed = (speed > 0) ? Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X) : -Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X);

            boundingBox = RecalculateBoundingBox();
        }

        protected virtual void OnCollision(World world, int xCoord, int yCoord)
        {
            world.grid[xCoord, yCoord].OnCollision(world, this);
        }
    }
}
