using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class Button
    {
        public Rectangle rectangle;

        public Button(int x, int y, int width, int height)
        {
            rectangle = new Rectangle(x, y, width, height);
        }

        public bool highlighted()
        {
            MouseState mouse = Mouse.GetState();
            return Utilities.CheckCollision(mouse.Position, rectangle);
        }

    }
}
