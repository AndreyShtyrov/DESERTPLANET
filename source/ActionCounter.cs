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
        private Player Player;
        private bool IsHarvester;


        public ActionCounter(int zero, int step, Player player, bool isHarvester)
        {
            Zero = zero;
            Step = step;
            Count = 0;
            Player = player;
            IsHarvester = isHarvester;
        }

        public int Count { get; private set; }

        public int PredictEnergyChange(int steps)
        {
            if (!IsHarvester)
                return ((2 * (Zero + Count * Step) + Step * (steps - 1)) * steps) / 2;
            if (Player.IsHydroponic)
            {
                if (Count == 0)
                {
                    var count = 1;
                    if (steps > 1)
                        return 2 * (steps - 1) + count;
                    else
                        return count;
                }
                else
                    return 2 * steps;
            }
            else
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
