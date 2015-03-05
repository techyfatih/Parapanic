using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Parapanic
{
    class Ambulance
    {
        Rectangle carRect;
        double topSpeed;
        double acceleration;
        double friction;
        public double direction;
        public double mouseDirection;
        double speed = 0;

        double turnrate = 3;


        public Ambulance(int X, int Y, int width, int height, double direction, double ts, double a, double f)
        {
            carRect.X = X;
            carRect.Y = Y;
            carRect.Width = width;
            carRect.Height = height;
            this.direction = direction;
            topSpeed = ts;
            acceleration = a;
            friction = f;
        }

        public Rectangle getRectangle()
        {
            return carRect;
        }

        public Vector2 getVector()
        {
            return new Vector2(carRect.X, carRect.Y);
        }

        public double getDirection()
        {
            return direction;
        }

        public void Update(GameTime gameTime)
        {
            /*if ((Keyboard.GetState().IsKeyDown(Keys.W)))
            {
                
            }*/

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (speed + acceleration < topSpeed)
                {
                    speed += acceleration;
                }
                else
                {
                    speed = topSpeed;
                }
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (speed - acceleration > 0)
                {
                    speed -= acceleration;
                }
                else
                {
                    if (speed - acceleration > -topSpeed / 2)
                    {
                        speed -= acceleration / 2;
                    }
                    else
                    {
                        speed = -topSpeed / 2;
                    }
                }
            }

            mouseDirection = MathHelper.ToDegrees((float)(Math.Atan2((Mouse.GetState().Y - (carRect.Y + carRect.Height / 2)), (Mouse.GetState().X - (carRect.X + carRect.Width / 2)))));
            //mouseDirection = MathHelper.ToDegrees((float)(Math.Atan2((Mouse.GetState().Y - (carRect.Y + carRect.Height + 500)), (Mouse.GetState().X - (carRect.X + carRect.Width + 500)))));

            mouseDirection = NormAngle(mouseDirection);


            if (Math.Abs(mouseDirection - direction) > turnrate)
            {
                if (Math.Abs(NormAngle(mouseDirection + 180) - direction) < 180)
                //if(Math.Abs(mouseDirection-direction) < 180)
                {
                    direction += (direction > 180)?turnrate:-turnrate;
                }
                else
                {
                    direction -= (direction > 180)?turnrate:-turnrate;
                }
            }

            direction = NormAngle(direction);

            //direction = mouseDirection;



            if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Released)
            {
                speed *= friction;
            }

            Vector2 a = new Vector2((float)(Math.Sin((float)MathHelper.ToRadians((float)direction))), (float)Math.Cos(MathHelper.ToRadians((float)direction)));
            a.Normalize();

            carRect.Y += (int)(a.X * speed);
            carRect.X += (int)(a.Y * speed);

        }

        public double NormAngle(double a)
        {
            double b = a;
            while (b > 360)
            {
                b -= 360;
            }
            while (b < 0)
            {
                b += 360;
            }
            return b;
        }

    }
}
