using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ForceUpdateUI : Action
    {
        public bool Containers { get; set; }

        public bool Electrisity { get; set; }
        public override void Backward()
        {
            if (Containers) 
                Map.RebuildContainers();
            if (Electrisity)
                Map.RebuildElectrisity();
        }

        public override void Forward()
        {
            if (Containers)
                Map.RebuildContainers();
            if (Electrisity)
                Map.RebuildElectrisity();
        }

        public ForceUpdateUI(bool updateContainers, bool updateElectrisity)
        {
            Containers = updateContainers;
            Electrisity = updateElectrisity;
        }
    }
}
