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
        private double lastSpeed;
        public double topSpeed { get; protected set; }
        protected double acceleration;
        protected double friction;
        public float scale { get; protected set; }

        protected double[] cornerAngles;
        protected double diagonal;

        
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
            //friction!!
            if (speed == lastSpeed)
                speed *= friction;


            Vector2 speedV = new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));

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

            for (int x = 0; x < world.grid.GetLength(0); x++)
            {
                for (int y = 0; y < world.grid.GetLength(1); y++)
                {
                    if (world.grid[x, y].solid && boundingBox.Intersects(world.grid[x,y].boundary))
                    {
                        bool collisionCalled = false;
                        if ((position.X > world.grid[x, y].position.X && position.X < world.grid[x, y].position.X + Block.size)
                          && Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary))
                        {
                            OnCollision(world, x, y);
                            collisionCalled = true;
                            speedV.Y = 0;
                        }
                        if ((position.Y > world.grid[x, y].position.Y && position.Y < world.grid[x, y].position.Y + Block.size)
                         && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary))
                        {
                            if (!collisionCalled)
                                OnCollision(world, x, y);
                            speedV.X = 0;
                        }
                    }
                    else if (Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary)
                            && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary))
                    {
                        OnCollision(world, x, y);
                    }
                    /*if (world.grid[x, y].GetType().Equals(typeof(PatientBlock))
                        && (Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary)
                        && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary)))
                    {
                        hasPatient = true;
                        int xPos = (int)world.grid[x, y].position.X;
                        int yPos = (int)world.grid[x, y].position.Y;
                        world.pointsOfInterest.Remove(world.grid[x, y].position);
                        world.grid[x, y] = new FloorBlock(xPos, yPos);
                        Minimap.Map.DirtyFlag = true;
                    }
                    else if (world.grid[x, y].GetType().Equals(typeof(HospitalBlock))
                        && (Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary)
                        && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary))
                        && hasPatient)
                    {
                        hasPatient = false;
                        int xPos = (int)world.grid[x, y].position.X;
                        int yPos = (int)world.grid[x, y].position.Y;
                        world.pointsOfInterest.Remove(world.grid[x, y].position);
                        world.grid[x, y] = new FloorBlock(xPos, yPos);
                        Minimap.Map.DirtyFlag = true;
                    }
                    else if (world.grid[x, y].GetType().Equals(typeof(WallBlock)))
                    {
                        if ((position.X > world.grid[x, y].position.X && position.X < world.grid[x, y].position.X + Block.size)
                            && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary))
                        {
                            speedV.Y = 0;
                            intersected = true;
                        }
                        if ((position.Y > world.grid[x, y].position.Y && position.Y < world.grid[x, y].position.Y + Block.size)
                         && Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary))
                        {
                            speedV.X = 0;
                            intersected = true;
                        }
                    }*/
                }
            }

            //Console.WriteLine(intersected);

            position.X += Utilities.Round(speedV.X, 2);
            position.Y += Utilities.Round(speedV.Y, 2);

            Console.WriteLine(position.X + " " + position.Y);
            Console.WriteLine(speedV.X + " " + speedV.Y);
            Console.WriteLine(Utilities.Round(speedV.X, 3) + " " + Utilities.Round(speedV.Y, 3));
            

            //Console.WriteLine(position.X + " " + position.Y);
            //Console.WriteLine(Utilities.round(101.12345f, 4));

            //Console.WriteLine(speedV.X + " " + speedV.Y);
            speed = (speed > 0) ? Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X) : -Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X);
            
            
            lastSpeed = speed;
        }

        protected virtual void OnCollision(World world, int xCoord, int yCoord)
        {
            world.grid[xCoord, yCoord].OnCollision(world, this);
        }
    }
}
