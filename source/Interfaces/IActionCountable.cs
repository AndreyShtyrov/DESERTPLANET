using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Interfaces
{
    public interface IActionCountable
    {
        public int Count { get; }

        public int PredictEnergyChange(int steps);

        public void Refresh ();

        public void RetainEnergy(int steps);

        public void SpendActions(int steps);
        
    }
}
