using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability
{
    public class AbilityInfo
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public ResourceContainer Recipe { get; set; }

        public AbilityInfo() {
            Name = "None";
            Description = "";
            
        }

    }

    public delegate void LoadInfo(object sender);
}
