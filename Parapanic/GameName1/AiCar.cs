using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class AiCar : Car
    {
        public AiCar(int x, int y, double speed, float direction, double topSpeed, double acceleration, double friction)
            : base(x, y, speed, direction, topSpeed, acceleration, friction)
        {

        }

        public override void Update(World world)
        {
            base.Update(world);
        }

        protected override void OnCollision(World world, int xCoord, int yCoord)
        {
            base.OnCollision(world, xCoord, yCoord);
        }
    }
}
