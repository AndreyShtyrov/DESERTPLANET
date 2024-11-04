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
    public class SolarPanel : Building, IHasStartTurnAction
    {
        public SolarPanel(int x, int y, int layerId, int id, Player owner) : base("SolarPanel", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(1, 0);

        public bool HasStartTurnAction => true;

        public List<IAction> StartTurnActions()
        {
            return new List<IAction>() { new IncreaseEnergy(Id, 2) };
        }
    }

    public class SolarPanelRecept : BuildingRecipe
    {
        public SolarPanelRecept(Player player):base(8)
        {
            Resources.Add(new PlanetResource(ResourceType.Glass, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build SolarPanel";
            Info.Recipe = Resources;
        }
    }
}
