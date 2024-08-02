using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class Move : Action
    {
        public int SourceId { get; set; }

        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var unit = Map.GetObjectById(SourceId);
            unit.X = X1;
            unit.Y = Y1;
            Map.NeedRedraw = true;
            Map.CleanPaths();
        }

        [JsonConstructor]
        public Move() { }
        public Move(int sourceId, int X, int Y): base()
        {
            SourceId = sourceId;
            var unit = Map.GetObjectById(sourceId);
            X0 = unit.X; Y0 = unit.Y;
            X1 = X; Y1 = Y;
        }
    }
}
