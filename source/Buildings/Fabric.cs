using DesertPlanet.source.Ability;
using DesertPlanet.source.Ability.Constructs;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Fabric : Building
    {
        public Fabric(int x, int y, int layerId, int id, Player owner, GameMode mode) 
            : base("Fabric", x, y, layerId, id, owner)
        {
            var company = mode.GetCompany(owner.Id);
            Abilities.Add(new ConstructFloatingPlatfrom(company.Recepts[5], this, 0));
            Abilities.Add(new ConstructHarvester(company.Recepts[10], this, 1));
            CanBuild = true;
        }

        public override Vector2I TileShift => new Vector2I(0, 3);
    }

    public class FabricRecept : BuildingRecipe
    {
        public FabricRecept(Player player) : base(7)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build Fabric";
            Info.Recipe = Resources;
        }
    }
}
