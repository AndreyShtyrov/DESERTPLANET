using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class HydroponicFerm : Building
    {
        public HydroponicFerm(int x, int y, int layerId, int id, Player owner) 
            : base("HydroponicFerm", x, y, layerId, id, owner)
        {
        }
        public override Vector2I TileShift => new Vector2I(4, 0);
    }

    public class HydroponicFermRecipe : BuildingRecipe
    {
        public HydroponicFermRecipe(Player player) : base(19)
        {
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.Iron, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.Iron, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.Iron, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.Iron, player.Id));
            Info.Name = "HydroponicFerm";
            Info.Recipe = Resources;
        }
    }
}
