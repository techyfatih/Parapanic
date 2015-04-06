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
    class VectorAmbulance : Car
    {
        bool hasPatient;
        bool intersected;
        const double MAX_TURN_RATE = 0.12;

        Vector2 speedV = new Vector2(0,0);
        Vector2 driftV = new Vector2(0,0);
        bool forwards = true;
        float drawDirection;
        bool drifting = false;
        const float driftCoeff = .94f;

        public VectorAmbulance(int x, int y, float direction, double topSpeed, double acceleration, double friction)
            : base(x, y, 0, direction, topSpeed, acceleration, friction) { hasPatient = false; drawDirection = direction; }

        public override void Update(World world)
        {
            

            intersected = false;
            MouseState mouse = Mouse.GetState();

            //Left Click - Acceleration
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                
                if (speed + acceleration < topSpeed)
                    speed += (float)acceleration;
                else
                    speed = (float)topSpeed;

                speedV = Utilities.floatToVector((float)speed, direction);
            }

            //Right Click - Brake/Reverse
            if (mouse.RightButton == ButtonState.Pressed)
            {
                if (speed - acceleration > 0)
                    speed -= (float)acceleration;
                else
                {
                    if (speed - acceleration > -topSpeed / 2)
                        speed -= (float)acceleration / 2;
                    else
                        speed = -(float)topSpeed / 2;
                }

                speedV = Utilities.floatToVector((float)speed, direction);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                drifting = true;
                driftV = speedV * ((1 - driftCoeff)) / 2;
                speedV *= driftCoeff;
            }
            else 
            {
                driftV = new Vector2(0, 0);
                drifting = false;
            }

            forwards = (speed >= 0 )? true : false;

            //base.Update(world);

            //Console.WriteLine(intersected);

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
                    drawDirection = (float)Utilities.NormAngle(direction + turnrate);
                else
                    drawDirection = (float)Utilities.NormAngle(direction - turnrate);
            }
            Vector2 finalV = speedV + driftV;

            if(!drifting)
            {
                direction = drawDirection;
            }
            else
            {
                direction = Utilities.vectorToDirection(finalV);
            }

            speed = Utilities.vectorToFloat(finalV,forwards);

            base.Update(world);

            speedV = Utilities.floatToVector((float)speed, (float)direction);

        }

        protected override void OnCollision(World world, int xCoord, int yCoord)
        {
            Block block = world.grid[xCoord, yCoord];

            if (block is WallBlock)
                intersected = true;
            else if (block is PatientBlock &&
                     !hasPatient)
            {
                hasPatient = true;
                int xPos = (int)world.grid[xCoord, yCoord].position.X;
                int yPos = (int)world.grid[xCoord, yCoord].position.Y;
                world.pointsOfInterest.Remove(((PatientBlock)block).POIHandle);
                world.grid[xCoord, yCoord] = new RoadBlock(xPos, yPos);
                Minimap.Map.DirtyFlag = true;
            }
            else if (block is HospitalBlock &&
                     hasPatient)
            {
                hasPatient = false;
                int xPos = (int)world.grid[xCoord, yCoord].position.X;
                int yPos = (int)world.grid[xCoord, yCoord].position.Y;
                world.pointsOfInterest.Remove(((HospitalBlock)block).POIHandle);
                world.grid[xCoord, yCoord] = new RoadBlock(xPos, yPos);
                Minimap.Map.DirtyFlag = true;
            }


            base.OnCollision(world, xCoord, yCoord);
        }
    }
}
