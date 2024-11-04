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
    public class ConstructPark : ConstructBuilding
    {
        public ConstructPark(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, true)
        {
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var result = base.Use(mode, target);
            if (result.Count > 0)
            {
                result.Add(new ChangeRepo(Unit.Owner.Id, 3));
            }
            return result;
        }
    }
}
