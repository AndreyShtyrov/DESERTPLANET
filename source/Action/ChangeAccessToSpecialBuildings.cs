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
            var company = Map.GetCompany(PlayerId);
            company.IsOffice = PrevIsOffice;
            company.HasHydroponic = PrevIsHydroponic;
        }

        public override void Forward()
        {
            var company = Map.GetCompany(PlayerId);
            company.IsOffice = IsOffice;
            company.HasHydroponic = IsHydroponic;
        }

        public ChangeAccessToSpecialBuildings(bool isHydroponic, bool isOffice, Player player) : base()
        {
            var company = Map.GetCompany(player.Id);
            PrevIsHydroponic = company.HasHydroponic;
            PrevIsOffice = company.IsOffice;
            IsHydroponic = isHydroponic;
            IsOffice = isOffice;
        }

        [JsonConstructor]
        public ChangeAccessToSpecialBuildings() { }
    }
}
