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

    class RoadBlock : FloorBlock
    {
        public RoadTypes Type;
        
        public RoadBlock(int x, int y)
            : base(x, y)
        { }

        public void InitializeType(World world)
        {
            int gridX = (int)(position.X / Block.size);
            int gridY = (int)(position.Y / Block.size);

            bool rightRoadBlock = false;
            bool leftRoadBlock = false;
            bool upRoadBlock = false;
            bool downRoadBlock = false;

            int gWidth = world.grid.GetLength(0);
            int gHeight = world.grid.GetLength(1);

            if (gridX + 1 < gWidth && gridX >= 0 && gridY >= 0 && gridY < gHeight &&
                world.grid[gridX + 1, gridY] is FloorBlock)
            {
                rightRoadBlock = true;
            }
            if (gridX > 0 && gridX - 1 < gWidth && gridY >= 0 && gridY < gHeight &&
                world.grid[gridX - 1, gridY] is FloorBlock)
            {
                leftRoadBlock = true;
            }
            if (gridY + 1 < gHeight && gridY >= -1 && gridX >= 0 && gridX < gWidth &&
                world.grid[gridX, gridY + 1] is FloorBlock)
            {
                downRoadBlock = true;
            }
            if (gridY > 0 && gridY - 1 < gHeight && gridX >= 0 && gridX < gWidth &&
                world.grid[gridX, gridY - 1] is FloorBlock)
            {
                upRoadBlock = true;
            }

            Type = RoadTypes.Unknown;

            if (leftRoadBlock && rightRoadBlock && !upRoadBlock && !downRoadBlock)
                Type = RoadTypes.Horizontal;
            else if (!leftRoadBlock && !rightRoadBlock && upRoadBlock && downRoadBlock)
                Type = RoadTypes.Vertical;
            else if (leftRoadBlock && rightRoadBlock && upRoadBlock && downRoadBlock)
                Type = RoadTypes.FourWay;
            else if (!leftRoadBlock && rightRoadBlock && !upRoadBlock && !downRoadBlock)
                Type = RoadTypes.EndL;
            else if (leftRoadBlock && !rightRoadBlock && !upRoadBlock && !downRoadBlock)
                Type = RoadTypes.EndR;
            else if (!leftRoadBlock && !rightRoadBlock && !upRoadBlock && downRoadBlock)
                Type = RoadTypes.EndD;
            else if (!leftRoadBlock && !rightRoadBlock && upRoadBlock && !downRoadBlock)
                Type = RoadTypes.EndU;
            else if (!leftRoadBlock && rightRoadBlock && upRoadBlock && !downRoadBlock)
                Type = RoadTypes.TwoWayRU;
            else if (!leftRoadBlock && rightRoadBlock && !upRoadBlock && downRoadBlock)
                Type = RoadTypes.TwoWayRD;
            else if (leftRoadBlock && !rightRoadBlock && upRoadBlock && !downRoadBlock)
                Type = RoadTypes.TwoWayLU;
            else if (leftRoadBlock && !rightRoadBlock && !upRoadBlock && downRoadBlock)
                Type = RoadTypes.TwoWayLD;
            else if (leftRoadBlock && rightRoadBlock && upRoadBlock && !downRoadBlock)
                Type = RoadTypes.ThreeWayULR;
            else if (!leftRoadBlock && rightRoadBlock && upRoadBlock && downRoadBlock)
                Type = RoadTypes.ThreeWayUDR;
            else if (leftRoadBlock && !rightRoadBlock && upRoadBlock && downRoadBlock)
                Type = RoadTypes.ThreeWayUDL;
            else if (leftRoadBlock && rightRoadBlock && !upRoadBlock && downRoadBlock)
                Type = RoadTypes.ThreeWayDLR;
        }
    }
}
