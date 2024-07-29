using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Stone : FieldToken
    {
        public Stone(int x, int y) 
            : base(x, y, "Stone", new ResourceContainer())
        {
        }

        public Stone(int x, int y, ResourceContainer resources)
            : base(x, y, "Stone", resources)
        {
        }

        public Stone(int x, int y, ResourceContainer resources, int blockSeds)
            : base(x, y, "Stone", resources, blockSeds)
        {
        }
    }
}
