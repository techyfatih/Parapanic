using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Parapanic
{
    class Ambulance : Car
    {
        bool hasPatient;

        public Ambulance(int x, int y, float direction, double topSpeed, double acceleration, double friction)
            : base(x, y, 0, direction, topSpeed, acceleration, friction) { hasPatient = false; }

        public override void Update(World world)
        {
            //Left Click - Acceleration
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (speed + acceleration < topSpeed)
                    speed += acceleration;
                else
                    speed = topSpeed;
            }

            //Right Click - Brake/Reverse
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (speed - acceleration > 0)
                    speed -= acceleration;
                else
                {
                    if (speed - acceleration > -topSpeed / 2)
                        speed -= acceleration / 2;
                    else
                        speed = -topSpeed / 2;
                }
            }

            //Mouse Direction - Turning
            double turnrate = (Math.Abs(speed) > 1) ? ((MAX_TURN_RATE / topSpeed) * Math.Abs(speed)) : 0; //Don't turn when not moving

            double mouseDirection = Utilities.NormAngle(Math.Atan2(Mouse.GetState().Y - position.Y + Camera.position.Y, 
                                                                   Mouse.GetState().X - position.X + Camera.position.X));

            if (Math.Abs(mouseDirection - direction) > turnrate)
            {
                double refDir = direction;
                if (direction >= Math.PI)
                {
                    refDir -= Math.PI;
                    mouseDirection = Utilities.NormAngle(mouseDirection - Math.PI);
                }
                if (mouseDirection > refDir && mouseDirection < refDir + Math.PI)
                    direction = (float)Utilities.NormAngle(direction + turnrate);
                else
                    direction = (float)Utilities.NormAngle(direction - turnrate);
            }

            //Slide according to friction is nothing is pressed
            if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Released)
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
                    if (world.grid[x, y].GetType().Equals(typeof(PatientBlock))
                        && (Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary)
                        && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary)))
                    {
                        hasPatient = true;
                        int xPos = (int)world.grid[x,y].position.X;
                        int yPos = (int)world.grid[x,y].position.Y;
                        world.PointsOfInterest.Remove(world.grid[x, y].position);
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
                        world.PointsOfInterest.Remove(world.grid[x, y].position);
                        world.grid[x, y] = new FloorBlock(xPos, yPos);
                        Minimap.Map.DirtyFlag = true;
                    }
                    else if (world.grid[x, y].GetType().Equals(typeof(WallBlock)))
                    {
                        if ((position.X > world.grid[x, y].position.X && position.X < world.grid[x, y].position.X + Block.size)
                            && Utilities.CheckCollisionY(boundingBox, world.grid[x, y].boundary))
                            speedV.Y = 0;
                        if ((position.Y > world.grid[x, y].position.Y && position.Y < world.grid[x, y].position.Y + Block.size)
                            && Utilities.CheckCollisionX(boundingBox, world.grid[x, y].boundary))
                            speedV.X = 0;
                    }
                }
            }
            position.X += speedV.X;
            position.Y += speedV.Y;
        }
    }
}
