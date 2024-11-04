using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Park : Building
    {
        public Park(int x, int y, int layerId, int id, Player owner) : base("Park", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(3, 1);
    }

    public class ParkRecipe : BuildingRecipe
    {
        public ParkRecipe(Player player) : base(17)
        {
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
        }
    }
}
