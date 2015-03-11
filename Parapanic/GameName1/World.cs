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
        public int Width;
        public int Height;

        public World(int width, int height)
        {
            grid = new Block[width, height];
            Width = width*Block.size;
            Height = height*Block.size;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++ )
                    grid[x, y] = new WallBlock(x * Block.size, y * Block.size);
        }
    }
}
