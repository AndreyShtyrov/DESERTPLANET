using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Blocked: FieldToken
    {
        public Blocked(int x, int y)
            : base(x, y, "Blocked", new ResourceContainer())
        {
        }
        public Blocked(int x, int y, ResourceContainer resources)
            : base(x, y, "Blocked", resources)
        {
        }

        public Blocked(int x, int y, ResourceContainer resources, int blockSeds)
        : base(x, y, "Blocked", resources, blockSeds)
        {
        }
    }
}
