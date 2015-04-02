using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parapanic
{
    class HospitalBlock : Block
    {
        public PointOfInterest POIHandle;
        public HospitalBlock(int x, int y, PointOfInterest poi) : base(x, y) 
        {
            POIHandle = poi;
            solid = false;
        }
    }
}
