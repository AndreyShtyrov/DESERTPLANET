using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class CloseStartActions : Action
    {
        public int PlayerId { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            if (Map.ActivePlayer == Map.Player)
                Map.NeedApplyTurnStartActions = false;
            Map.NeedRedraw = true;  
        }

        [JsonConstructor]
        public CloseStartActions() { }
        public CloseStartActions(Player player)
        {
            PlayerId = player.Id;
        }
    }
}
