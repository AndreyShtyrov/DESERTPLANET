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
            int count = 0;
            foreach (var harvester in Map.Harvesters.Values)
                if (harvester.Owner == player)
                    count++;
            var harvester1 = new Harvester(HarvesterId, X, Y, player, Map);
            Map.Harvesters.Add(HarvesterId, harvester1);
            switch (count)
            {
                case 0:
                    {
                        harvester1.Name = harvester1.Name + " A";
                        break;
                    }
                case 1:
                    {
                        harvester1.Name = harvester1.Name + " B";
                        break;
                    }
                case 2:
                    {
                        harvester1.Name = harvester1.Name + " C";
                        break;
                    }
                case 3:
                    {
                        harvester1.Name = harvester1.Name + " D";
                        break;
                    }
            }
            Map.NeedRedraw = true;
            Map.UnitId = HarvesterId + 1;
            Map.NeedUpdateHarvetersList = true;
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
