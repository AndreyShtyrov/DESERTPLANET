using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Radar : Building
    {
        public Radar(int x, int y, int layerId, int id, Player owner) : base("Radar", x, y, layerId, id, owner)
        {
        }
    }

    public class RadarBuildingRecipe : BuildingRecipe
    {
        public RadarBuildingRecipe(Player player) : base(15)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
        }
    }
}
