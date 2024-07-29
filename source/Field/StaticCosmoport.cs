using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class StaticCosmoport : FieldToken
    {
        public StaticCosmoport(int x, int y) 
            : base(x, y, "StaticCosmoport", new ResourceContainer())
        {
        }

        public StaticCosmoport(int x, int y, ResourceContainer resources)
            : base(x, y, "StaticCosmoport", resources)
        {
        }

        public StaticCosmoport(int x, int y, ResourceContainer resources, int blockSeds)
            : base(x, y, "StaticCosmoport", resources, blockSeds)
        {
        }
    }
}
