using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Interfaces
{
    public interface IHasStartTurnAction
    {
        public bool HasStartTurnAction { get; }

        public List<IAction> StartTurnActions();
    }
}
