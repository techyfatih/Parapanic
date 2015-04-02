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
        bool intersected;
        const double MAX_TURN_RATE = 0.07;

        public Ambulance(int x, int y, float direction, double topSpeed, double acceleration, double friction)
            : base(x, y, 0, direction, topSpeed, acceleration, friction) { hasPatient = false; }

        public override void Update(World world)
        {
            intersected = false;
            MouseState mouse = Mouse.GetState();
            //Left Click - Acceleration
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (speed + acceleration < topSpeed)
                    speed += acceleration;
                else
                    speed = topSpeed;
            }

            //Right Click - Brake/Reverse
            if (mouse.RightButton == ButtonState.Pressed)
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


            base.Update(world);

            Console.WriteLine(intersected);
            
            //Mouse Direction - Turning
            double turnrate = (Math.Abs(speed) > 1) ? ((MAX_TURN_RATE / topSpeed) * Math.Abs(speed)) : 0; //Don't turn when not moving

            double mouseDirection = Utilities.NormAngle(Math.Atan2(mouse.Y - position.Y + Camera.position.Y,
                                                                   mouse.X - position.X + Camera.position.X));

            if (Math.Abs(mouseDirection - direction) > turnrate //&& !intersected)
                )
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

        }

        protected override void OnCollision(World world, int xCoord, int yCoord)
        {
            Block block = world.grid[xCoord, yCoord];

            if (block.GetType().Equals(typeof(WallBlock)))
                intersected = true;
            else if (block.GetType().Equals(typeof(PatientBlock)) &&
                     !hasPatient)
            {
                hasPatient = true;
                int xPos = (int)world.grid[xCoord, yCoord].position.X;
                int yPos = (int)world.grid[xCoord, yCoord].position.Y;
                world.pointsOfInterest.Remove(world.grid[xCoord, yCoord].position);
                world.grid[xCoord, yCoord] = new FloorBlock(xPos, yPos);
                Minimap.Map.DirtyFlag = true;
            }
            else if (block.GetType().Equals(typeof(HospitalBlock)) &&
                     hasPatient)
            {
                hasPatient = false;
                int xPos = (int)world.grid[xCoord, yCoord].position.X;
                int yPos = (int)world.grid[xCoord, yCoord].position.Y;
                world.pointsOfInterest.Remove(world.grid[xCoord, yCoord].position);
                world.grid[xCoord, yCoord] = new FloorBlock(xPos, yPos);
                Minimap.Map.DirtyFlag = true;
            }


            base.OnCollision(world, xCoord, yCoord);
        }
    }
}
