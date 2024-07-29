using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class BuildHarvesterRecept : BuildingRecipe
    {
        public BuildHarvesterRecept(Player player) : base(10)
        {
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.None, player.Id));
        }
    }
}
