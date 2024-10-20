using DesertPlanet.source.Ability;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class MeltingFurnace : Building
    {
        public MeltingFurnace(int x, int y, int layerId, int id, Player owner, GameMode mode) 
            : base("MeltingFurnace", x, y, layerId, id, owner)
        {
            var company = mode.GetCompany(owner.Id);
            Abilities.Add(new RefineGlass(company.AbilityRecepts["RefineGlass"], this, 0));
            Abilities.Add(new RefineAliminium(company.AbilityRecepts["RefineAliminium"], this, 1));
        }

        public override Vector2I TileShift => new Vector2I(1, 3);
    }

    public class MeltingFurnaceRecept: BuildingRecipe
    {

        public MeltingFurnaceRecept(Player player): base(9)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build Furnace";
            Info.Recipe = Resources;
        }
    }
}
