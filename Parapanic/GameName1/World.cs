using System;
using System.Collections.Generic;
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
        public List<Vector2> PointsOfInterest = new List<Vector2>();

        #region GenerationConstants

        const int BORDERWIDTH = 5;
        const int BORDERHEIGHT = 5;
        const int CITYBLOCKWIDTH = 5;
        const int CITYBLOCKHEIGHT = 2;

        #endregion 

        public World(int width, int height)
        {
            Random r = new Random();
            grid = new Block[width, height];
            Width = width*Block.size;
            Height = height*Block.size;

            for (int i = 0; i < BORDERWIDTH; i++)
            {
                for (int u = 0; u < height; u++)
                {
                    grid[i, u] = new WallBlock(i * Block.size, u * Block.size);
                    grid[width - i - 1, u] = new WallBlock((width - i - 1) * Block.size, u * Block.size);
                }
            }
            for (int i = 0; i < BORDERHEIGHT; i++)
            {
                for (int u = 0; u < width; u++)
                {
                    grid[u, i] = new WallBlock(u * Block.size, i * Block.size);
                    grid[u, height - i - 1] = new WallBlock(u * Block.size, (height - i - 1) * Block.size);
                }
            }

            //use BORDERWIDTH and BORDERHEIGHT as offsets to create the rest of our grid
            for (int i = BORDERWIDTH; i < width - BORDERWIDTH; i++)
            {
                for (int u = BORDERHEIGHT; u < height - BORDERHEIGHT; u++)
                {
                    if ((i - BORDERWIDTH) % (CITYBLOCKWIDTH + 1) == 0 || (u - BORDERHEIGHT) % (CITYBLOCKHEIGHT + 1) == 0
                        || i == width - BORDERWIDTH - 1 || u == height - BORDERHEIGHT - 1)
                    {
                        grid[i, u] = new FloorBlock(i * Block.size, u * Block.size);
                    }
                    else
                    {
                        grid[i, u] = new WallBlock(i * Block.size, u * Block.size);
                    }
                }
            }
            //spawn patients, hospitals, etc
            int patientX = r.Next(BORDERWIDTH, width - BORDERWIDTH);
            int patientY = r.Next(BORDERHEIGHT, height - BORDERHEIGHT);

            int hospitalX = r.Next(BORDERWIDTH, width - BORDERWIDTH);
            int hospitalY = r.Next(BORDERHEIGHT, height - BORDERHEIGHT);

            grid[patientX, patientY] =
                new PatientBlock(patientX * Block.size, patientY * Block.size);
            PointsOfInterest.Add(new Vector2(patientX * Block.size, patientY * Block.size));

            grid[hospitalX, hospitalY] =
                new HospitalBlock(hospitalX * Block.size, hospitalY * Block.size);
            PointsOfInterest.Add(new Vector2(hospitalX * Block.size, hospitalY * Block.size));

            //fill up any remaining spots
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int u = 0; u < grid.GetLength(1); u++)
                    if (grid[i, u] == null)
                        grid[i, u] = new FloorBlock(i * Block.size, u * Block.size);
            
            /*int patientX = r.Next(width/2);
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
            }*/
        }
    }
}
