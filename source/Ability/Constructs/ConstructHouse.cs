using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public class ConstructHouse : ConstructBuilding
    {
        public ConstructHouse(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id)
        {
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var result = base.Use(mode, target);
            if (result.Count > 0)
            {
                bool isNearWater = false;
                if (mode.Map[target] is Water)
                    isNearWater = true;
                foreach(var field in mode.Map[target].Neighbors)
                {
                    if (!mode.Map.InBound(field))
                        continue;
                    if (mode.Map[field] is Water)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                    result.Add(new ChangeRepo(Unit.Owner.Id, 3));
                else
                    result.Add(new ChangeRepo(Unit.Owner.Id, 2));
            }
            return result;
        }
    }
}
