using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class DigResource : Action
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ResourceType Type { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var field = Map.Map[X, Y];
            field.Resources.Remove(new PlanetResource(Type, -1));
            Map.NeedRedraw = true;
        }

        public DigResource(int x, int y, ResourceType type): base()
        {
            X = x;
            Y = y;
            Type = type;
        }
    }
}
