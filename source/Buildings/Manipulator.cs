using DesertPlanet.source.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Manipulator : Building
    {
        public Manipulator(int x, int y, int layerId, int id, Player owner) 
            : base("Manipulator", x, y, layerId, id, owner)
        {
            Abilities.Add(new Ability.ShiftResource(this, 0));
        }
    }

    public class ManipulatorRecipe : BuildingRecipe
    {
        public ManipulatorRecipe(Player player) : base(14)
        {
            Resources.Add(new PlanetResource(ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, player.Id));
        }
    }
}
