using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class SandLime : FieldToken
    {
        public SandLime(int x, int y) :
            base(x, y, "SandLime", new ResourceContainer())
        {
        }

        public SandLime(int x, int y, ResourceContainer resources) :
            base(x, y, "SandLime", resources)
        {
        }

        public SandLime(int x, int y, ResourceContainer resources, int blockSeds) :
        base(x, y, "SandLime", resources, blockSeds)
        {
        }
    }
}
