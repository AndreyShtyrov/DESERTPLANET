using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public class ActionCounter : IActionCountable
    {
        private int Zero = 0;
        private int Step = 0;

        public ActionCounter(int zero, int step)
        {
            Zero = zero;
            Step = step;
            Count = 0;
        }

        public int Count { get; private set; }

        public int PredictEnergyChange(int steps)
        {
            return ((2 * (Zero + Count * Step) + Step * (steps - 1)) * steps) / 2;
        }

        public void Refresh()
        {
            Count = 0;
        }

        public void RetainEnergy(int steps)
        {
            Count -= steps;
        }

        public void SpendActions(int steps)
        {
            Count += steps;
        }
    }
}
