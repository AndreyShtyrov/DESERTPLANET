using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class IncreaseActionCount : Action
    {
        public int UnitId { get; set; }

        public int Count { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var unit = Map.GetObjectById(UnitId) as IOwnedTokenWithAbilites;
            unit.Counter.SpendActions(Count);
        }

        [JsonConstructor]
        public IncreaseActionCount() { }

        public IncreaseActionCount(int unitId, int count)
        {
            UnitId = unitId;
            Count = count;
        }
    }
}
