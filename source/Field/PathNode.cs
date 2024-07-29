using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public struct PathNode
    {
        public Vector2I Arrow { get; set; }

        public int Energy { get; set; }

        public bool IsReachable { get; set; }

        public PathNode()
        {
            Arrow = new Vector2I(-1, -1);
            Energy = -1;
            IsReachable = false;
        }
        public PathNode(Vector2I arrow, int energy)
        {
            Arrow = arrow;
            Energy = energy;
            IsReachable = true;
        }

    
        public bool ValueIsSetted
        {
            get
            {
                if (Arrow.X >= 0 &&  Arrow.Y >= 0 && Energy >= 0)
                    return true;
                return false;
            }
        }
    }
}
