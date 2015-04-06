using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Parapanic
{
    public struct PointOfInterest
    {
        public enum Types
        {
            Hospital, Patient
        }

        public Types Type;
        public Vector2 Position;
    }

    class World
    {
        public Block[,] grid;
        public int Width;
        public int Height;
        public List<PointOfInterest> pointsOfInterest;

        const int BORDERWIDTH = 5;
        const int BORDERHEIGHT = 5;
        const int CITYBLOCKWIDTH = 5;
        const int CITYBLOCKHEIGHT = 2;
        public List<Car> Cars = new List<Car>();

        public World(int width, int height)
        {
            //Variables
            Random r = new Random();
            grid = new Block[width + 2 * BORDERWIDTH, height + 2 * BORDERHEIGHT];
            Width = (width + 2*BORDERWIDTH)*Block.size; //For camera
            Height = (height + 2*BORDERHEIGHT)* Block.size; //For camera
            pointsOfInterest = new List<PointOfInterest>();

            Rectangle[] outerRegions = 
                new Rectangle[4] {new Rectangle(0, 0, (int)(0.1*width), (int)(0.9*height)),
                                  new Rectangle((int)(0.1*width), 0, (int)(0.9*width), (int)(0.1*height)),
                                  new Rectangle((int)(0.9*width), (int)(0.1*height), (int)(0.1*width), (int)(0.9*height)),
                                  new Rectangle(0, (int)(0.9*height), (int)(0.9*width), (int)(0.1*height))};

            int innerWidth = (int)(0.4 * width);
            int innerHeight = (int)(0.4 * height);
            Rectangle[] innerRegions = 
                new Rectangle[4] {new Rectangle((int)(0.1*width), (int)(0.1*height), innerWidth, innerHeight),
                                  new Rectangle(width/2, (int)(0.1*height), innerWidth, innerHeight),
                                  new Rectangle((int)(0.1*width), height/2, innerWidth, innerHeight),
                                  new Rectangle(width/2, height/2, innerWidth, innerHeight)};
            Vector2[,] nodePairs = new Vector2[20,2];
            List<Vector2[]> connections = new List<Vector2[]>();

            //Patient and hospital points
            int patientRegion = r.Next(4);
            Vector2 patient =
                new Vector2(BORDERWIDTH + r.Next(outerRegions[patientRegion].Left, BORDERWIDTH + outerRegions[patientRegion].Right),
                            BORDERHEIGHT + r.Next(outerRegions[patientRegion].Top, BORDERHEIGHT + outerRegions[patientRegion].Bottom));

            PointOfInterest patientPoi = new PointOfInterest() 
                { Position = patient * Block.size, Type = PointOfInterest.Types.Patient };

            grid[(int)patient.X, (int)patient.Y] =
                new PatientBlock((int)patient.X * Block.size, (int)patient.Y * Block.size, patientPoi);
            pointsOfInterest.Add(patientPoi);

            int hospitalRegion = (patientRegion + 2) % 4;
            Vector2 hospital =
                new Vector2(BORDERWIDTH + r.Next(outerRegions[hospitalRegion].Left, BORDERWIDTH + outerRegions[hospitalRegion].Right),
                            BORDERHEIGHT + r.Next(outerRegions[hospitalRegion].Top, BORDERHEIGHT + outerRegions[hospitalRegion].Bottom));

            PointOfInterest hospitalPoi = new PointOfInterest() 
                { Position = hospital * Block.size, Type = PointOfInterest.Types.Hospital };

            grid[(int)hospital.X, (int)hospital.Y] =
                new HospitalBlock((int)hospital.X * Block.size, (int)hospital.Y * Block.size, hospitalPoi);
            pointsOfInterest.Add(hospitalPoi);

            //Create node pairs and connections
            for (int i = 0; i < 20; i++)
            {
                int region1 = r.Next(4);
                Vector2 node1 =
                    new Vector2(BORDERWIDTH + r.Next(innerRegions[region1].Left, BORDERWIDTH + innerRegions[region1].Right),
                                BORDERHEIGHT + r.Next(innerRegions[region1].Top, BORDERHEIGHT + innerRegions[region1].Bottom));
                nodePairs[i, 0] = node1;

                int region2 = r.Next(4);
                if (region2 == region1) region2 = (region2 == 3) ? 0 : region2 + 1;
                Vector2 node2 = 
                    new Vector2(BORDERWIDTH + r.Next(innerRegions[region2].Left, BORDERWIDTH + innerRegions[region2].Right),
                                BORDERHEIGHT + r.Next(innerRegions[region2].Top, BORDERHEIGHT + innerRegions[region2].Bottom));
                nodePairs[i, 1] = node2;

                connections.Add(new Vector2[] { patient, hospital });
                connections.Add(new Vector2[] { node1, node2 });
                connections.Add(new Vector2[] { node2, hospital });
            }

            //Put connections in grid
            foreach (Vector2[] connection in connections)
            {
                Console.WriteLine("Initial: " + connection[0]);
                Console.WriteLine("Vector: " + connection[1]);
                bool horizontalFirst = Convert.ToBoolean(r.Next(2)); //Draw horizontally or vertically first
                int xi = (int)connection[0].X;
                int yi = (int)connection[0].Y;
                int xf = (int)connection[1].X;
                int yf = (int)connection[1].Y;
                int deltaX = xf - xi;
                int deltaY = yf - yi;
                if (horizontalFirst)
                {
                    if (deltaX > 0)
                    {
                        for (int x = xi; x <= xf; x++)
                            if (grid[x, yi] == null)
                                grid[x, yi] = new RoadBlock(x * Block.size, yi * Block.size);
                    }
                    else
                    {
                        for (int x = xi; x >= xf; x--)
                            if (grid[x, yi] == null)
                                grid[x, yi] = new RoadBlock(x * Block.size, yi * Block.size);
                    }
                    if (deltaY > 0)
                    {
                        for (int y = yi; y <= yf; y++)
                            if (grid[xf, y] == null)
                                grid[xf, y] = new RoadBlock(xf * Block.size, y * Block.size);
                    }
                    else
                    {
                        for (int y = yi; y >= yf; y--)
                            if (grid[xf, y] == null)
                                grid[xf, y] = new RoadBlock(xf * Block.size, y * Block.size);
                    }
                }
                else
                {
                    if (deltaY > 0)
                    {
                        for (int y = yi; y <= yf; y++)
                            if (grid[xf, y] == null)
                                grid[xf, y] = new RoadBlock(xf * Block.size, y * Block.size);
                    }
                    else
                    {
                        for (int y = yi; y >= yf; y--)
                            if (grid[xf, y] == null)
                                grid[xf, y] = new RoadBlock(xf * Block.size, y * Block.size);
                    }
                    if (deltaX > 0)
                    {
                        for (int x = xi; x <= xf; x++)
                            if (grid[x, yi] == null)
                                grid[x, yi] = new RoadBlock(x * Block.size, yi * Block.size);
                    }
                    else
                    {
                        for (int x = xi; x >= xf; x--)
                            if (grid[x, yi] == null)
                                grid[x, yi] = new RoadBlock(x * Block.size, yi * Block.size);
                    }
                }
            }

            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] == null) grid[x, y] = new WallBlock(x * Block.size, y * Block.size);

            /*for (int i = 0; i < BORDERWIDTH; i++)
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
                        grid[i, u] = new RoadBlock(i * Block.size, u * Block.size);
                    }
                    else
                    {
                        grid[i, u] = new WallBlock(i * Block.size, u * Block.size);
                    }
                }
            }

            //fill up any remaining spots
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int u = 0; u < grid.GetLength(1); u++)
                    if (grid[i, u] == null)
                        grid[i, u] = new RoadBlock(i * Block.size, u * Block.size);*/
            
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
                        grid[x, y] = new RoadBlock(x * Block.size, y * Block.size);
                }
            }*/


            foreach (Block b in grid)
                if (b is RoadBlock)
                    ((RoadBlock)b).InitializeType(this);

            foreach (Block b in grid)
            {
                if (b is FloorBlock)
                    Cars.Add(new AiCar((int)b.position.X, (int)b.position.Y, 1, 1, 1, this));
            }
        }

        public Vector2 EmptySpace()
        {
            Random r = new Random();
            List<Vector2> empties = new List<Vector2>();
            foreach (Block b in grid)
                if (b is RoadBlock) empties.Add(b.position);
            return empties[r.Next(empties.Count)];
        }
    }
}
