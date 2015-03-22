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
            Random r = new Random();
            grid = new Block[width, height];
            Width = width*Block.size;
            Height = height*Block.size;
            int patientX = r.Next(width/2);
            int patientY = r.Next(height);
            int hospitalX = r.Next(width/2, width);
            int hospitalY = r.Next(height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++ )
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        grid[x, y] = new WallBlock(x * Block.size, y * Block.size);
                    else if (x == patientX && y == patientY)
                        grid[x, y] = new PatientBlock(x * Block.size, y * Block.size);
                    else if (x == hospitalX && y == hospitalY)
                        grid[x, y] = new HospitalBlock(x * Block.size, y * Block.size);
                    else
                        grid[x, y] = new FloorBlock(x * Block.size, y * Block.size);
                }
            }
        }
    }
}
