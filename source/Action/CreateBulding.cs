using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class CreateBulding : Action
    {
        public int UnitId { get; set; }

        public int Code { get; set; }
        public int PlayerId { get; set; }
        public int X { get; set; }

        public int Y { get; set; }

        public int BuildingCode { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var company = Map.GetCompany(PlayerId);
            var building = company.CreateBuilding(X, Y, UnitId, Code, Map.GetPlayer(PlayerId));
            Map.UnitId = UnitId + 1;
            Map.Buildings.Add(building.Id, building);
            Map.NeedRedraw = true;
            Map.NeedLoadAbilityGUI = true;
            Map.UpdateAbilitiesGuiTargets.Add(building.Id);
        }

        public CreateBulding(int unitId, int x, int y, int code, Player player): base()
        {
            Code = code;
            PlayerId = player.Id;
            UnitId = unitId;
            X = x;
            Y = y;
            BuildingCode = code;
        }
    }
}
