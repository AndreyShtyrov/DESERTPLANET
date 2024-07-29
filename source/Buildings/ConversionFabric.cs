using DesertPlanet.source.Ability;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class ConversionFabric : Building
    {
        public ConversionFabric(int x, int y, int layerId, int id, Player owner, GameMode mode) 
            : base("ConversionFabric", x, y, layerId, id, owner)
        {
            var company = mode.GetCompany(owner.Id);
            Abilities.Add(new RefineOil(company.AbilityRecepts["RefineOil"], this, 0));
            Abilities.Add(new RefineCement(company.AbilityRecepts["RefineCement"], this, 1));
            Abilities.Add(new RefineCementFromBaskit(company.AbilityRecepts["RefineCementFromBaskit"], this, 2));
        }

        public override Vector2I TileShift => new Vector2I(1, 1);
    }

    public class ConversionFabricRecept : BuildingRecipe
    {
        public ConversionFabricRecept(Player player) : base(6)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
        }
    }
}
