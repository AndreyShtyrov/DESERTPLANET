using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Water : FieldToken
    {
        public Water(int x, int y)
            : base(x, y, "Water", new ResourceContainer())
        {
        }
        public Water(int x, int y, ResourceContainer resources) 
            : base(x, y, "Water", resources)
        {
        }

        public Water(int x, int y, ResourceContainer resources, int blockSeds)
        : base(x, y, "Water", resources, blockSeds)
        {
        }
    }
}
