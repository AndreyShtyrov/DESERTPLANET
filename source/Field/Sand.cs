using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Sand : FieldToken
    {
        public Sand(int x, int y) 
            : base(x, y, "Sand", new ResourceContainer())
        {
        }
        public Sand(int x, int y, ResourceContainer resources) 
            : base(x, y, "Sand", resources)
        {
        }

        public Sand(int x, int y, ResourceContainer resources, int blockSeds)
            : base(x, y, "Sand", resources, blockSeds)
        {
        }
    }
}
