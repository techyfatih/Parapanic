using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class WallBlock : Block
    {
        public WallBlock(int x, int y) : base(x, y) 
        {
            depth = Parapanic.Random.Next(1, 10);
            solid = true;
        }
    }
}
