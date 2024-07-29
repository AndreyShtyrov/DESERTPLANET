using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class SandBoskit : FieldToken
    {
        public SandBoskit(int x, int y) :
            base(x, y, "SandBoskit", new ResourceContainer())
        {
        }

        public SandBoskit(int x, int y, ResourceContainer resources) :
            base(x, y, "SandBoskit", resources)
        {
        }

        public SandBoskit(int x, int y, ResourceContainer resources, int blockSeds) :
            base(x, y, "SandBoskit", resources, blockSeds)
        {
        }
    }
}
