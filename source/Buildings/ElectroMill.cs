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
    public class ElectroMill : Building, IHasStartTurnAction
    {

        public ElectroMill(int x, int y, int layerId, int id, Player owner ) 
            : base("ElectroMill", x, y, layerId, id, owner) {
            
        }

        public int SourceLayer { get; }

        public List<IAction> StartTurnActions()
        {
            return new List<IAction>() { new IncreaseEnergy(Id, 1) };
        }

        public override Vector2I TileShift => new Vector2I(0, 0);

        public bool HasStartTurnAction => true;
    }

    public class ElectroMillRecept: BuildingRecipe
    {
        public ElectroMillRecept(Player player): base(0) {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build ElectroMill";
            Info.Recipe = Resources;
        }

    }
}
