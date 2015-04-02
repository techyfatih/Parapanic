using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class PatientBlock : Block
    {
        public PointOfInterest POIHandle;
        public PatientBlock(int x, int y, PointOfInterest poi) : base(x, y) 
        {
            POIHandle = poi;
            solid = false;
        }
    }
}
