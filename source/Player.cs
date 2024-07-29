using DesertPlanet.source.Buildings;
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

        public ResourceContainer Repos { get; }

        public int AmountStartHarvesters { get; set; } = 2;
    }
}
