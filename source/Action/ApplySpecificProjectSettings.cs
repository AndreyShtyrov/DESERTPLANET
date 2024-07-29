using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ApplySpecificProjectSettings : Action
    {
        public int PlayerId { get; set; }

        public int ProjectId { get; set; }
        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var company = Map.GetCompany(PlayerId);
            var buildings = new List<Building>();
            var harvesters = new List<Harvester>();
            var player = Map.GetPlayer(PlayerId);
            foreach (var unit in Map.Harvesters.Values)
                if (unit is Harvester && unit.Id == PlayerId)
                    harvesters.Add(unit as Harvester);
            foreach (var unit in Map.Buildings.Values)
                if (unit is Building && unit.Id == PlayerId)
                    buildings.Add(unit as  Building);
            var project = company.GetCompanyProject(ProjectId);
            if (project is ManipulatorDrill)
            {

            }
        }           

        public ApplySpecificProjectSettings(Player player, CompanyProject project)
        {
            PlayerId = player.Id;
            ProjectId = project.Id;
        }
    }
}
