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
    public class TidalElectrostation : Building
    {
        public TidalElectrostation(int x, int y, int layerId, int id, Player owner) : 
            base("TidalStation", x, y, layerId, id, owner)
        {
        }

        public override Vector2I TileShift => new Vector2I(4, 1);

        public List<IAction> StartTurnActions()
        {
            return new List<IAction>() { new IncreaseEnergy(Id, 2) };
        }
    }

    public class TidalElectrostationWaterPart: Building
    {
        public TidalElectrostationWaterPart(int x, int y, int layerId, int id, Player owner):
            base("TidalStationWaterPart", x, y, layerId, id, owner)
        {

        }

        public override Vector2I TileShift => new Vector2I(4, 2);
    }

    public class TidalElectroStationRecept : BuildingRecipe
    {
        public TidalElectroStationRecept(Player player) : base(3)
        {
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Resources.Add(new PlanetResource(ResourceType.Iron, ResourceType.Plastic, player.Id));
            Info.Name = "Build TidalElectroPlant";
            Info.Recipe = Resources;
        }
    }
}
