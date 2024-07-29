using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Helicopter : Building
    {
        public Helicopter(int x, int y, int layerId, int id, Player owner) 
            : base("Helicopter", x, y, layerId, id, owner)
        {
            Abilities.Add(new Ability.TransportResource(this, 0));
        }

        public override Vector2I TileShift => new Vector2I(2, 2);
    }

    public class HelicopterRecipe : BuildingRecipe
    {
        public HelicopterRecipe(Player player) : base(13)
        {
            Resources.Add(new PlanetResource(ResourceType.Aliminium, player.Id));
        }
    }
}
