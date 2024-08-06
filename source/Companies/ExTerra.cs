using DesertPlanet.source.Ability;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Companies
{
    public class ExTerra : Company
    {
        public new int BuildingLevel = 6;
        public override string Name { get; }
        public override int VerticalShiftForHarvesterTile { get; }
        public ExTerra(string name, Player player, GameMode mode) : base(player, mode)
        {
            Name = name;
            Player.Repos = 1;
            InitRecepts();
            InitStartResources();
            VerticalShiftForHarvesterTile = 1;
        }

        public override void InitStartResources()
        {
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Cement, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.Glass, Player.Id));


            Projects.Add(new ManipulatorDrill());
        }
    }
}
