using DesertPlanet.source.Action;
using DesertPlanet.source.Companies.Projects;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Manipulator : Building
    {
        public Manipulator(int x, int y, int layerId, int id, Player owner, GameMode gameMode) 
            : base("Manipulator", x, y, layerId, id, owner)
        {
            var company = gameMode.Companies[owner.Id];

            Abilities.Add(new Ability.ShiftResource(this, 41));
            foreach (var project in company.Projects)
            {
                if (project is ManipulatorDrill && project.IsSold)
                {
                    Abilities.Add(new Ability.DrillAction(this, 42));
                }
            }
        }

        public override Vector2I TileShift => new Vector2I(2, 0);
    }

    public class ManipulatorRecipe : BuildingRecipe
    {
        public ManipulatorRecipe(Player player) : base(14)
        {
            Resources.Add(new PlanetResource(ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, player.Id));
            Info.Name = "Build Manipulator";
            Info.Recipe = Resources;
        }
    }
}
