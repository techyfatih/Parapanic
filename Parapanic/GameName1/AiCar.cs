using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Parapanic
{
    class AiCar : Car
    {
        const byte Right = 0;
        const byte Up = 1;
        const byte Left = 2;
        const byte Down = 3;
        const int farSide = Block.size * 3 / 4;
        const int closeSide = Block.size / 4;

        static byte turningNumber = 0;
        byte aiDir;
        bool turning = false;
        float targetDirection;
        float directionChange;
        float turningAngle;
        float turningAngleTarget;
        float turningAngleChange;
        Vector2 turningAngleOrigin;
        float turningAngleRadius;
        byte lastAiDir;

        int gridX;
        int gridY;

        public AiCar(int x, int y, double topSpeed, double acceleration, double friction, World world)
            : base(0, 0, 0, 0, topSpeed, acceleration, friction)
        {
            frictionEnabled = false;
            gridX = x / Block.size;
            gridY = y / Block.size;

            diagonal = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) / 2.5;

            bool rightRoadBlock = false;
            bool leftRoadBlock = false;
            bool upRoadBlock = false;
            bool downRoadBlock = false;


            if (gridX + 1 < world.grid.GetLength(0) && world.grid[gridX + 1, gridY] is FloorBlock)
            {
                rightRoadBlock = true;
            }
            if (gridX > 0 && world.grid[gridX - 1, gridY] is FloorBlock)
            {
                leftRoadBlock = true;
            }
            if (gridY + 1 < world.grid.GetLength(1) && world.grid[gridX, gridY + 1] is FloorBlock)
            {
                downRoadBlock = true;
            }
            if (gridY > 0 && world.grid[gridX, gridY - 1] is FloorBlock)
            {
                upRoadBlock = true;
            }

            while (!upRoadBlock || !leftRoadBlock || !downRoadBlock || !rightRoadBlock)
            {
                if (upRoadBlock && turningNumber % 4 == Up)
                {
                    aiDir = Up;
                    position.Y = gridY * Block.size + Block.size / 2;
                    position.X = gridX * Block.size + farSide;
                    direction = -MathHelper.PiOver2;
                    break;
                }
                if (leftRoadBlock && turningNumber % 4 == Left)
                {
                    aiDir = Left;
                    position.Y = gridY * Block.size + closeSide;
                    position.X = gridX * Block.size + Block.size / 2;
                    direction = MathHelper.Pi;
                    break;
                }
                if (downRoadBlock && turningNumber % 4 == Down)
                {
                    aiDir = Down;
                    position.Y = gridY * Block.size + Block.size / 2;
                    position.X = gridX * Block.size + closeSide;
                    direction = MathHelper.PiOver2;
                    break;
                }
                if (rightRoadBlock && turningNumber % 4 == Right)
                {
                    aiDir = Right;
                    position.Y = gridY * Block.size + farSide;
                    position.X = gridX * Block.size + Block.size / 2;
                    direction = 0;
                    break;
                }

                ++turningNumber;
            }
        }

        public override void Update(World world)
        {
            int[] newX = new int[4];
            int[] newY = new int[4];
            for (int i = 0; i < 4; i++)
            {
                cornerAngles[i] = Utilities.NormAngle(cornerAngles[i] + direction);
                newX[i] = (int)(Math.Cos(cornerAngles[i]) * diagonal + position.X);
                newY[i] = (int)(Math.Sin(cornerAngles[i]) * diagonal + position.Y);
            }

            int boundX = Utilities.Minimum(newX);
            int boundY = Utilities.Minimum(newY);
            int boundWidth = Utilities.Maximum(newX) - boundX;
            int boundHeight = Utilities.Maximum(newY) - boundY;

            boundingBox = new Rectangle(boundX, boundY, boundWidth, boundHeight);

            if (!turning)
            {
                Vector2 vSpeed = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
                position += vSpeed;

                if (gridX != (int)(position.X / Block.size) || gridY != (int)(position.Y / Block.size))
                {
                    gridX = (int)(position.X / Block.size);
                    gridY = (int)(position.Y / Block.size);
                    turning = true;

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

                    switch (aiDir)
                    {
                        case Left:
                        {
                            if (leftRoadBlock)
                            {
                                lastAiDir = aiDir;
                                aiDir = Left;
                                turning = false;
                                return;
                            }
                        } break;
                        case Right:
                        {
                            if (rightRoadBlock)
                            {
                                lastAiDir = aiDir;
                                aiDir = Right;
                                turning = false;
                                return;
                            }
                        } break;
                        case Up:
                        {
                            if (upRoadBlock)
                            {
                                lastAiDir = aiDir;
                                aiDir = Up;
                                turning = false;
                                return;
                            }
                        } break;
                        case Down:
                        {
                            if (downRoadBlock)
                            {
                                lastAiDir = aiDir;
                                aiDir = Down;
                                turning = false;
                                return;
                            }
                        } break;
                    }

                    while (upRoadBlock || leftRoadBlock || downRoadBlock || rightRoadBlock)
                    {
                        ++turningNumber;

                        if (upRoadBlock && turningNumber % 4 == Up)
                        {
                            lastAiDir = aiDir;
                            aiDir = Up;
                            targetDirection = -MathHelper.PiOver2 + MathHelper.TwoPi;
                            switch (lastAiDir)
                            {
                                case Right:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = -MathHelper.PiOver2;
                                    turningAngleTarget = 0;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size, gridY * Block.size);
                                    turningAngleRadius = Block.size * 3 / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Left:
                                {
                                    int timeToTurn = 60;
                                    turningAngle = -MathHelper.PiOver2;
                                    turningAngleTarget = -MathHelper.Pi;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2((gridX + 1) * Block.size, gridY * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Down:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = -MathHelper.Pi;
                                    turningAngleTarget = 0;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size + Block.size / 2, gridY * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = -Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;
                            }
                            turningNumber += (byte)Parapanic.Random.Next(255);
                            break;
                        }
                        if (leftRoadBlock && turningNumber % 4 == Left)
                        {
                            lastAiDir = aiDir;
                            aiDir = Left;
                            targetDirection = MathHelper.Pi;
                            switch (lastAiDir)
                            {
                                case Right:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = -MathHelper.PiOver2;
                                    turningAngleTarget = MathHelper.PiOver2;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size, gridY * Block.size + Block.size / 2);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = -Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Up:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = 0;
                                    turningAngleTarget = MathHelper.PiOver2;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size, (gridY + 1) * Block.size);
                                    turningAngleRadius = Block.size * 3 / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Down:
                                {
                                    int timeToTurn = 60;
                                    turningAngle = 0;
                                    turningAngleTarget = -MathHelper.PiOver2;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size, gridY * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;
                            }
                            turningNumber += (byte)Parapanic.Random.Next(255);
                            break;
                        }
                        if (downRoadBlock && turningNumber % 4 == Down)
                        {
                            lastAiDir = aiDir;
                            aiDir = Down;
                            targetDirection = MathHelper.PiOver2;
                            switch (lastAiDir)
                            {
                                case Right:
                                {
                                    int timeToTurn = 60;
                                    turningAngle = MathHelper.PiOver2;
                                    turningAngleTarget = 0;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size, (gridY + 1) * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Left:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = MathHelper.PiOver2;
                                    turningAngleTarget = MathHelper.Pi;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2((gridX + 1) * Block.size, (gridY + 1) * Block.size);
                                    turningAngleRadius = Block.size * 3 / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Up:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = 0;
                                    turningAngleTarget = MathHelper.Pi;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2(gridX * Block.size + Block.size / 2, (gridY + 1) * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = -Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;
                            }
                            turningNumber += (byte)Parapanic.Random.Next(255);
                            break;
                        }
                        if (rightRoadBlock && turningNumber % 4 == Right)
                        {
                            lastAiDir = aiDir;
                            aiDir = Right;
                            targetDirection = 0;
                            switch (lastAiDir)
                            {
                                case Left:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = MathHelper.PiOver2;
                                    turningAngleTarget = MathHelper.PiOver2 + MathHelper.Pi;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2((gridX + 1) * Block.size, gridY * Block.size + Block.size / 2);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = -Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Up:
                                {
                                    int timeToTurn = 60;
                                    turningAngle = MathHelper.Pi;
                                    turningAngleTarget = MathHelper.PiOver2;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2((gridX + 1) * Block.size, (gridY + 1) * Block.size);
                                    turningAngleRadius = Block.size / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;

                                case Down:
                                {
                                    int timeToTurn = 120;
                                    turningAngle = MathHelper.Pi;
                                    turningAngleTarget = MathHelper.Pi + MathHelper.PiOver2;
                                    turningAngleChange = (turningAngleTarget - turningAngle) / timeToTurn;
                                    turningAngleOrigin = new Vector2((gridX + 1) * Block.size, gridY * Block.size);
                                    turningAngleRadius = Block.size * 3 / 4;
                                    directionChange = Utilities.NormAnglePiToPi(targetDirection - direction) / timeToTurn;
                                } break;
                            }
                            turningNumber += (byte)Parapanic.Random.Next(255);
                            break;
                        }
                    }
                }
            }


            if (turning)
            {
                turningAngle += turningAngleChange;
                position = turningAngleOrigin + new Vector2((float)Math.Cos(turningAngle), -(float)Math.Sin(turningAngle)) * turningAngleRadius;
                direction += directionChange;
                if (Math.Abs(turningAngleTarget - turningAngle) < 0.1f)
                {
                    turning = false;
                    direction = targetDirection;
                }

            }
        }

        protected override void OnCollision(World world, int xCoord, int yCoord)
        {
            base.OnCollision(world, xCoord, yCoord);
        }
    }
}
