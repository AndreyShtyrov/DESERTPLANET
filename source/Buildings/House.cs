using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class House : Building
    {
        public House(int x, int y, int layerId, int id, Player owner) : base("House", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(1, 2);
    }

    public class HouseRecipe : BuildingRecipe
    {
        public HouseRecipe(Player player) : base(16)
        {
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
        }
    }
}
