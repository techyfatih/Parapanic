using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class GeometryUtils
    {
        public static double NormAngle(double a)
        {
            double b = a;
            while (b > 360)
            {
                b -= 360;
            }
            while (b < 0)
            {
                b += 360;
            }
            return b;
        }
    }
}
