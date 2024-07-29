using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class StoneIron : FieldToken
    {
        public StoneIron(int x, int y) :
            base(x, y, "StoneIron", new ResourceContainer())
        {
        }

        public StoneIron(int x, int y, ResourceContainer resources) :
            base(x, y, "StoneIron", resources)
        {
        }

        public StoneIron(int x, int y, ResourceContainer resources, int blockSeds) :
    base(x, y, "StoneIron", resources, blockSeds)
        {
        }
    }
}
