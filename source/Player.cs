using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DesertPlanet.source
{
    public class Player
    {
        public List<Building> Buildings { get; }

        public int Id { get; }

        public int Repos { get; set; }

        public List<CompanyProject> Projects { get; }
        public int AmountStartHarvesters { get; set; } = 2;

        public Player(int id)
        {
            Projects = new List<CompanyProject>();
            Id = id;
        }
    }
}
