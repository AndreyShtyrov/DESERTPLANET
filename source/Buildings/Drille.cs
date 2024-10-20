using DesertPlanet.source.Ability;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Drille : Building
    {
        public Drille(int x, int y, int layerId, int id, Player owner ) : base("Drille", x, y, layerId, id, owner)
        {
            Abilities.Add(new DrillAction(this, 0));
        }

        public override Vector2I TileShift => new Vector2I(0, 1);
    }

    public class DrilleRecept : BuildingRecipe
    {
        public DrilleRecept(Player player) : base(2)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build Drile";
            Info.Recipe = Resources;
        }
    }
}
