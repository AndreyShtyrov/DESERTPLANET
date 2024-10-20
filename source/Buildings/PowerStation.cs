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
    public class PowerStation : Building, IHasStartTurnAction
    {
        public PowerStation(int x, int y, int layerId, int id, Player owner) : base("PowerStation", x, y, layerId, id, owner)
        {
            
        }

        public bool HasStartTurnAction => true;

        public List<IAction> StartTurnActions()
        {
            return new List<IAction>() { new IncreaseEnergy(Id, 8) };
        }

        public override Vector2I TileShift => new Vector2I(3, 3);
    }

    public class PowerStationRecept : BuildingRecipe
    {
        public PowerStationRecept(Player player) : base(11)
        {
            Resources.Add(new PlanetResource(ResourceType.Uran, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Uran, ResourceType.None, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Cement, player.Id));
            Info.Name = "Build PowerStation";
            Info.Recipe = Resources;
        }
    }
}
