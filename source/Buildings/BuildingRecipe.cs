using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class BuildingRecipe: IRecipe
    {
        public ResourceContainer Resources { get; private set; }

        public int Code { get; private set; }
        public BuildingRecipe(int code)
        {
            Resources = new ResourceContainer();
            Code = code;
        }
        public bool CheckBuildPrerequst()
        { return true; }

    }
}
