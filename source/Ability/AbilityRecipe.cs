using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesertPlanet.source.Interfaces;

namespace DesertPlanet.source.Ability
{
    public class AbilityRecipe: IRecipe
    {
        public ResourceContainer Resources { get; private set; }

        public AbilityRecipe()
        {
            Resources = new ResourceContainer();
        }
    }
}
