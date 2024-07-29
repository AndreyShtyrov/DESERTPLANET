using DesertPlanet.source.Ability;
using DesertPlanet.source.Action;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class ThermalPowerPlant : Building
    {
        public ThermalPowerPlant(int x, int y, int layerId, int id, Player owner, GameMode mode) 
            : base("ThermalPowerPlant", x, y, layerId, id, owner)
        {
            var company = mode.GetCompany(owner.Id);
            Abilities.Add(new BurnOil(company.AbilityRecepts["BurnOil"], this, 0));
        }

        public bool HasStartTurnAction => true;

        public override Vector2I TileShift => new Vector2I(3, 0);
    }

    public class ThermalPowerPlantRecept : BuildingRecipe
    {
        public ThermalPowerPlantRecept(Player player) : base(12)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
        }
    }
}
