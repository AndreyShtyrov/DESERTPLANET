using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class RemoveStartHarvester : Action
    {
        public int PlayerId { get; set;}
        public override void Backward()
        {
            var player = Map.GetPlayer(PlayerId);
            player.AmountStartHarvesters++;
        }

        public override void Forward()
        {
            var player = Map.GetPlayer(PlayerId);
            if (player.AmountStartHarvesters > 0)
                player.AmountStartHarvesters--;
        }

        public RemoveStartHarvester(int playerId): base() {
            PlayerId = playerId;
        }
    }
}
