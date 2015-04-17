using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Parapanic
{
    class WallBlock : Block
    {
        public WallBlock(int x, int y, Color color) : base(x, y) 
        {
            Random r = Parapanic.Random;
            depth = r.Next(1, 11);
            solid = true;
            Color = color;
            Color.R = (byte)MathHelper.Clamp(r.Next(-32, 33) + Color.R, 0, 255);
            Color.G = (byte)MathHelper.Clamp(r.Next(-32, 33) + Color.G, 0, 255);
            Color.B = (byte)MathHelper.Clamp(r.Next(-32, 33) + Color.B, 0, 255);
        }
    }
}
