using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class WaterOil : FieldToken
    {
        public WaterOil(int x, int y) 
            : base(x, y, "WaterOil", new ResourceContainer())
        {
        }

        public WaterOil(int x, int y, ResourceContainer resources)
            : base(x, y, "WaterOil", resources)
        {
        }

        public WaterOil(int x, int y, ResourceContainer resources, int blockSeds)
    : base(x, y, "WaterOil", resources, blockSeds)
        {
        }
    }
}
