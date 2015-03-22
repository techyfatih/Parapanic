using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class FloorBlock : Block
    {
        public FloorBlock(int x, int y) : base(x, y) 
        {
            depth = 0;
        }
    }
}
