using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class CreateHarvester : Action
    {
        public int HarvesterId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int PlayerId { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var player = Map.GetPlayer(PlayerId);
            Map.Harvesters.Add(HarvesterId, new Harvester(HarvesterId, X, Y, player, Map.GetCompany(PlayerId)));
            Map.NeedRedraw = true;
            Map.NeedLoadAbilityGUI = true;
            Map.NeedLoadAbilityGUI = true;
            Map.UpdateAbilitiesGuiTargets.Add(HarvesterId);
            Map.UnitId = HarvesterId + 1;
        }

        [JsonConstructor]

        public CreateHarvester() { }
        public CreateHarvester(int harvesterId, int x, int y, Player player): base() { 
            HarvesterId = harvesterId;
            X = x;
            Y = y;
            PlayerId = player.Id;
        }
    }
}
