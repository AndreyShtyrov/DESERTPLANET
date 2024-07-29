﻿using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public sealed class ConstructConversionFabric : ConstructBuilding
    {
        public ConstructConversionFabric(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, false)
        {
            Name = "B. ConFabric";
        }
    }
}
