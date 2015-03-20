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
    class Ambulance
    {
        public Vector2 position;
        public float rotation;
        public Vector2 origin;

        Rectangle carRect;
        double topSpeed;
        double acceleration;
        double direction;
        double friction;
        double mouseDirection;
        double speed;
        const double maxTurnrate = 3;

        public Ambulance(int x, int y, double direction, double topSpeed, double acceleration, double friction)
        {
            position = new Vector2(x, y);
            this.direction = direction;
            origin = new Vector2(48, 32);
            carRect = new Rectangle(x, y, 96, 96);
            this.topSpeed = topSpeed;
            this.acceleration = acceleration;
            this.friction = friction;
            speed = 0;
        }

        public void Update(GameTime gameTime, World world)
        {
            //Left Click - Acceleration
                Console.WriteLine(speed);

                //bool nobutton = true;

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
            double turnrate = (Math.Abs(speed) > 1) ? ((maxTurnrate / topSpeed) * Math.Abs(speed)) : 0; //Don't turn when not moving

            mouseDirection = MathHelper.ToDegrees((float)(Math.Atan2((Mouse.GetState().Y - (position.Y - Camera.position.Y)), 
                                                                     (Mouse.GetState().X - (position.X - Camera.position.X)))));
            mouseDirection = GeometryUtils.NormAngle(mouseDirection);

            if (Math.Abs(mouseDirection - direction) > turnrate)
            {
                double refDir = direction;
                if (direction >= 180)
                {
                    refDir -= 180;
                    mouseDirection = GeometryUtils.NormAngle(mouseDirection - 180);
                }
                if (mouseDirection > refDir && mouseDirection < refDir + 180)
                    direction = GeometryUtils.NormAngle(direction + turnrate);
                else
                    direction = GeometryUtils.NormAngle(direction - turnrate);
            }

            //Slide according to friction is nothing is pressed
            if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Released)
                speed *= friction;

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
