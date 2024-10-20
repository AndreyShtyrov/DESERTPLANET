using DesertPlanet.source.Ability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Interfaces
{
    public interface ICanBuilding: IHasAbilities
    {
        public AbilityPresset GetAbilityByRecipt(IRecipe recipe);
    }
}
