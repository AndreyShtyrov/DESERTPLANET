using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class StoneOil : FieldToken
    {
        public StoneOil(int x, int y) :
            base(x, y, "StoneOil", new ResourceContainer())
        {
        }
        public StoneOil(int x, int y, ResourceContainer resources) :
        base(x, y, "StoneOil", resources)
        {
        }
        public StoneOil(int x, int y, ResourceContainer resources, int blockSeds) :
            base(x, y, "StoneOil", resources, blockSeds)
        {
        }
    }
}
