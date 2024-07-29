using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Interfaces
{
    public interface IAction
    {
        public int Idx
        { get; set; }
        public void Forward();
        public void Backward();
    }
}
