using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class CompanyOffice : Building
    {
        public CompanyOffice(int x, int y, int layerId, int id, Player owner) : base("CompanyOffice", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(2, 1);
    }

    public class CompanyOfficeRecipe : BuildingRecipe
    {
        public CompanyOfficeRecipe(Player player) : base(18)
        {
            Resources.Add(new PlanetResource(ResourceType.Cement, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Aliminium, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Aliminium, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Aliminium, ResourceType.None, player.Id));
        }
    }
}
