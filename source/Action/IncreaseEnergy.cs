using DesertPlanet.source.Buildings;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class IncreaseEnergy: Action
    {
        public int SourceId { get; set; }
        public int Energy { get; set; }

        public override void Backward()
        {
            
        }

        public override void Forward()
        {
            var source = Map.GetObjectById(SourceId);
            if (source == null)
            {
                GD.Print("!!IncreaseEnergy Action has been initiated by null object");
                return;
            }
            var field = Map.Map[source.X, source.Y];
            if (source is Harvester harvester)
            {
                for(int i = 0; i < Energy; i++)
                    field.Resources.Add(new PlanetResource(ResourceType.Energy, source.Owner.Id));
            }
            else
                for(int i = 0; i < Energy; i++)
                    field.Resources.Add(new PlanetResource(ResourceType.Energy, source.Owner.Id));
        }

        public IncreaseEnergy(int id, int energy): base()
        {
            SourceId = id;
            Energy = energy;
        }
    }
}
