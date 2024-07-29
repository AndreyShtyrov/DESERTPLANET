using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public sealed class ConstuctMill : ConstructBuilding
    {

        public ConstuctMill(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id)
        {
            Name = "B. Mill";
        }
    }
}
