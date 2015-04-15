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
        const int lookAheadDistance = 32;
        const int lookSidewaysDistance = 5;

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

        Rectangle turningBoundingBox;
        public bool proceedThroughCollision = false;
        public Car carToIgnore;

        public AiCar(int x, int y, double topSpeed, double acceleration, double friction, World world)
            : base(0, 0, 0, 0, topSpeed, acceleration, friction)
        {
            int colorIndex = Parapanic.Random.Next(10);
            switch (colorIndex)
            {
                case 0: color = Color.Blue; break;
                case 1: color = Color.Red; break;
                case 2: color = Color.DarkGreen; break;
                case 4: color = Color.DarkGray; break;
                case 5: color = Color.DarkBlue; break;
                case 6: color = Color.Brown; break;
                case 7: color = Color.LightSlateGray; break;
                case 8: color = Color.White; break;
                case 9: color = Color.WhiteSmoke; break;
            }
            frictionEnabled = false;
            gridX = x / Block.size;
            gridY = y / Block.size;

            world.grid[gridX, gridY].carsInside++;

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
                ++turningNumber;

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
            }
        }


        public override void Update(World world)
        {
            boundingBox = RecalculateBoundingBox();

            if (!turning)
            {
                Vector2 vSpeed = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
                
                Rectangle futureBounds = boundingBox;
                switch (aiDir)
                {
                    case Left:
                    futureBounds.X += (int)vSpeed.X * lookAheadDistance;
                    futureBounds.Width -= (int)vSpeed.X * lookAheadDistance;
                    futureBounds.Y -= lookSidewaysDistance;
                    futureBounds.Height += 2 * lookSidewaysDistance;
                    break;
                    case Right:
                    futureBounds.Width += (int)vSpeed.X * lookAheadDistance;
                    futureBounds.Y -= lookSidewaysDistance;
                    futureBounds.Height += 2 * lookSidewaysDistance;
                    break;
                    case Up:
                    futureBounds.Y += (int)vSpeed.Y * lookAheadDistance;
                    futureBounds.Height -= (int)vSpeed.Y * lookAheadDistance;
                    futureBounds.X -= lookSidewaysDistance;
                    futureBounds.Width += 2 * lookSidewaysDistance;
                    break;
                    case Down:
                    futureBounds.Height += (int)vSpeed.Y * lookAheadDistance;
                    futureBounds.X -= lookSidewaysDistance;
                    futureBounds.Width += 2 * lookSidewaysDistance;
                    break;
                }

                bool futureBoundsCrash = futureBounds.Intersects(world.ambulance.boundingBox);

                foreach (Car car in world.Cars)
                {
                    if (proceedThroughCollision && car == carToIgnore)
                    {
                        if (!futureBounds.Intersects(car.boundingBox))
                            proceedThroughCollision = false;
                    }
                    else if ((AiCar)car != this && futureBounds.Intersects(car.boundingBox))
                    {
                        futureBoundsCrash = true;
                        OnCollision(world, car); //TODO: i know this doesn't really detect collisions, just potential future ones, but it fits our purposes
                    }
                }

                if (futureBoundsCrash)
                    speed = Math.Max(0, speed - 2 * acceleration);
                else
                    speed = Math.Min(topSpeed, speed + acceleration);
                vSpeed *= (float)speed;
                position += vSpeed;

                if (gridX != (int)(position.X / Block.size) || gridY != (int)(position.Y / Block.size))
                {
                    world.grid[gridX, gridY].carsInside--;
                    gridX = (int)(position.X / Block.size);
                    gridY = (int)(position.Y / Block.size);
                    world.grid[gridX, gridY].carsInside++;
                    turning = true;

                    RecalculateDirection(world);
                }
            }


            if (turning)
            {
                bool potentialCrash = world.ambulance.boundingBox.Intersects(turningBoundingBox);
                foreach (Car c in world.Cars)
                {
                    if (proceedThroughCollision && c == carToIgnore)
                    {
                        if (!turningBoundingBox.Intersects(c.boundingBox))
                            proceedThroughCollision = false;
                    }
                    else if (c != this && c.boundingBox.Intersects(turningBoundingBox))
                    {
                        potentialCrash = true;
                        OnCollision(world, c);
                    }
                }

                if (!potentialCrash)
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

            boundingBox = RecalculateBoundingBox();
        }

        private void RecalculateDirection(World world)
        {
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
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
                            turningBoundingBox = new Rectangle(gridX * Block.size, gridY * Block.size, Block.size, Block.size);
                        } break;
                    }
                    turningNumber += (byte)Parapanic.Random.Next(255);
                    break;
                }
            }
            return;
        }

        protected override void OnCollision(World world, Car car)
        {
            if (car is AiCar)
            {
                ((AiCar)car).proceedThroughCollision = true;
                ((AiCar)car).carToIgnore = this;
            }
        }

        protected override void OnCollision(World world, int xCoord, int yCoord)
        {
            base.OnCollision(world, xCoord, yCoord);
        }
    }
}
