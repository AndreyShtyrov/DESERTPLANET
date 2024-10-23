using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesertPlanet.source.Ability;

namespace DesertPlanet.source.Interfaces
{
    public interface IHasAbilities: IOwnedToken
    {
        public List<AbilityPresset> Abilities { get; }

        public bool CanBuild { get; }
        public AbilityPresset GetAbilityById(int id);

        public bool HasAbility(int id);
        public ActionCounter Counter { get; }

        public bool CanMoving { get; }
    }
}
