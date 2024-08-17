using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class SolidPlatform : Building
    {
        public SolidPlatform(int x, int y, int layerId, int id, Player owner) 
            : base("SolidPlatform", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(0, 2);
    }

    public class SolidPlatfromRecept : BuildingRecipe
    {
        public SolidPlatfromRecept(Player player) : base(1)
        {
            Resources.Add(new PlanetResource(ResourceType.Cement, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.None, player.Id));
        }
    }

}
