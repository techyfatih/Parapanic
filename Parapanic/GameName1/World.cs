using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Parapanic
{
    class World
    {
        public Block[,] grid;
        int width;
        int height;

        public World()
        {
            grid = new Block[300, 300];
            width = 0;
            height = 0;
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++ )
                {
                    grid[x, y] = new WallBlock(x * Block.size, y * Block.size);
                    width += Block.size;
                }
                height += Block.size;
            }
        }
    }
}
