using DesertPlanet.source.Action;
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
    public class DrillAction : AbilityPresset
    {
        public override List<Vector2I> Area(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            return true;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            List<IAction> result = new List<IAction>();
            var company = mode.GetCompany(Unit.Owner);
            var token = Unit;
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y)) 
                if (unit is Harvester)
                {
                    token = unit as IOwnedTokenWithAbilites;
                    break;
                }
            if (mode.Map[Unit.X, Unit.Y] is StoneIron)
            {
                var res = company.GetAlignResource(ResourceType.Iron);
                result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            }
            if (mode.Map[Unit.X, Unit.Y] is WaterOil || mode.Map[Unit.X, Unit.Y] is StoneOil)
            {
                var res = company.GetAlignResource(ResourceType.Oil);
                result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            }
            result.AddRange(base.Use(mode, target));
            return result;
        }

        public DrillAction(IOwnedTokenWithAbilites token, int id) : base(id, false)
        {
            Unit = token;
            Name = "Dig";
        }

    }
}
