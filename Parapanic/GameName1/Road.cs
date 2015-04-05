using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    enum RoadTypes
    {
        Horizontal, Vertical, 
        FourWay, 
        ThreeWayUDL, ThreeWayUDR, ThreeWayULR, ThreeWayDLR, 
        EndL, EndR, EndU, EndD, 
        TwoWayLU, TwoWayRU, TwoWayLD, TwoWayRD,
        Unknown
    }

    class Road : FloorBlock
    {
        public RoadTypes Type;
        
        public Road(int x, int y)
            : base(x, y)
        { }

        public void InitializeType(World world)
        {
            int gridX = (int)(position.X / Block.size);
            int gridY = (int)(position.Y / Block.size);

            bool rightRoad = false;
            bool leftRoad = false;
            bool upRoad = false;
            bool downRoad = false;

            int gWidth = world.grid.GetLength(0);
            int gHeight = world.grid.GetLength(1);

            if (gridX + 1 < gWidth && gridX >= 0 && gridY >= 0 && gridY < gHeight &&
                world.grid[gridX + 1, gridY] is FloorBlock)
            {
                rightRoad = true;
            }
            if (gridX > 0 && gridX - 1 < gWidth && gridY >= 0 && gridY < gHeight &&
                world.grid[gridX - 1, gridY] is FloorBlock)
            {
                leftRoad = true;
            }
            if (gridY + 1 < gHeight && gridY >= -1 && gridX >= 0 && gridX < gWidth &&
                world.grid[gridX, gridY + 1] is FloorBlock)
            {
                downRoad = true;
            }
            if (gridY > 0 && gridY - 1 < gHeight && gridX >= 0 && gridX < gWidth &&
                world.grid[gridX, gridY - 1] is FloorBlock)
            {
                upRoad = true;
            }

            Type = RoadTypes.Unknown;

            if (leftRoad && rightRoad && !upRoad && !downRoad)
                Type = RoadTypes.Horizontal;
            else if (!leftRoad && !rightRoad && upRoad && downRoad)
                Type = RoadTypes.Vertical;
            else if (leftRoad && rightRoad && upRoad && downRoad)
                Type = RoadTypes.FourWay;
            else if (!leftRoad && rightRoad && !upRoad && !downRoad)
                Type = RoadTypes.EndL;
            else if (leftRoad && !rightRoad && !upRoad && !downRoad)
                Type = RoadTypes.EndR;
            else if (!leftRoad && !rightRoad && !upRoad && downRoad)
                Type = RoadTypes.EndD;
            else if (!leftRoad && !rightRoad && upRoad && !downRoad)
                Type = RoadTypes.EndU;
        }
    }
}
