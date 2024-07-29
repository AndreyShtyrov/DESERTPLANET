using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability
{
    public class ConstructHarvester : ConstructBuilding
    {
        public override List<Vector2I> Area(GameMode mode)
        {
            List<Vector2I> result = new List<Vector2I>();
            foreach(var area in mode.Area(Unit.X, Unit.Y, 1))
            {
                if (mode.Map[area.X, area.Y] is Water)
                    result.Add(area);
            }
            return result;
        }

        public ConstructHarvester(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id, bool needSelectTarget = false) 
            : base(recept, token, id, needSelectTarget)
        {
            Name = "C. Harvester";
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            if (!base.IsReadyToUse(mode)) return false;
            return true;
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            return mode.Logic.CreateHarvester(Unit.X, Unit.Y, Unit.Owner);
        }
    }
}
