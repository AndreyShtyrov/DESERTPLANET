using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Empty : FieldToken
    {
        public Empty(int x, int y) 
            : base(x, y, "Empty", new ResourceContainer())
        {
        }
        public Empty(int x, int y, ResourceContainer resources)
            : base(x, y, "Empty", resources)
        {
        }

        public Empty(int x, int y, ResourceContainer resources, int blockSeds)
            : base(x, y, "Empty", resources, blockSeds)
        {
        }
    }
}
