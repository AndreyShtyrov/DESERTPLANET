using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ChangeAccessToSpecialBuildings : Action
    {
        public bool IsHydroponic { get; set; }

        public bool PrevIsHydroponic { get; set; }

        public bool IsOffice { get; set; }

        public bool PrevIsOffice { get; set; }

        public int PlayerId { get; set; }
        public override void Backward()
        {
            var player = Map.GetPlayer(PlayerId);
            player.IsOffice = PrevIsOffice;
            player.IsHydroponic = PrevIsHydroponic;
        }

        public override void Forward()
        {
            var player = Map.GetPlayer(PlayerId);
            player.IsOffice = IsOffice;
            player.IsHydroponic = IsHydroponic;
        }

        public ChangeAccessToSpecialBuildings(bool isHydroponic, bool isOffice, Player player) : base()
        {
            PrevIsHydroponic = player.IsHydroponic;
            PrevIsOffice = player.IsOffice;
            IsHydroponic = isHydroponic;
            IsOffice = isOffice;
        }

        [JsonConstructor]
        public ChangeAccessToSpecialBuildings() { }
    }
}
