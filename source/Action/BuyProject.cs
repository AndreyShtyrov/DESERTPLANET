using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class BuyProject : Action
    {
        public int Player { get; set; }

        public int Project { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {

            var project = Map.GetCompany(Player).GetCompanyProject(Project);
            var player = Map.GetPlayer(Player);
            player.Projects.Add(project);
            project.IsSold = true;
        }

        [JsonConstructor]
        public BuyProject() { }

        public BuyProject(int PlayerId, int ProjectId)
        {
            Player = PlayerId; Project = ProjectId;
        }
    }
}
