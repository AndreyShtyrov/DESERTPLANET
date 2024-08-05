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
    public class Dig : AbilityPresset
    {
        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            if (!NeedSelecTarget)
                return new List<Vector2I>() { new Vector2I(Unit.X, Unit.Y) };
            foreach (var point in mode.Map[Unit.X, Unit.Y].Neighbors)
            {
                var field = mode.Map[point.X, point.Y];
                if (field is SandLime && field is SandBoskit)
                    result.Add(point);
            }
            return result;
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            if (!base.IsReadyToUse(mode)) return false;
            var field = mode.Map[Unit.X, Unit.Y];
            if (field.Resources.CountMinerals > 0) return true;
            if (field is SandBoskit || field is SandLime)
                return true;
            return false;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var result = new List<IAction>();
            var field = mode.Map[target.X, target.Y];
            if (field.Resources.CountMinerals > 0)
            {
                var type = new Nullable<ResourceType>();
                foreach (var res in field.Resources)
                    if (res.OwnerId == -1)
                    {
                        type = res.Type;
                        break;
                    }
                result.Add(new DigResource(field.X, field.Y, type.Value));
                if ( Unit is Harvester harvester)
                {
                    var res = mode.GetCompany(harvester.Owner.Id).GetAlignResource(type.Value);
                    var action = new IncomeResource(Unit.Id, Unit.X, Unit.Y, res.Type, res.Alternative);
                    result.Add(action);
                }
                else
                {
                    var res = mode.GetCompany(Unit.Owner.Id).GetAlignResource(type.Value);
                    var action = new IncomeResource(Unit.Id, Unit.X, Unit.Y, res.Type, res.Alternative);
                    result.Add(action);
                }
                result.AddRange(base.Use(mode, target));
                return result;
            }
            if (field is SandLime || field is SandBoskit)
            {
                if (Unit is Harvester harvester)
                {
                    var res = mode.GetCompany(Unit.Owner.Id).GetAlignResource(ResourceType.Lime);
                    if (field is SandBoskit)
                        res = mode.GetCompany(Unit.Owner.Id).GetAlignResource(ResourceType.Baksits);
                    result.Add(new IncomeResource(Unit.Id, Unit.X, Unit.Y, res.Type, res.Alternative));
                }
            }
            result.AddRange(base.Use(mode, target));
            result.Add(new ForceUpdateUI(true, false));
            return result;
        }

        public Dig(IOwnedTokenWithAbilites token, int id): base(id, false)
        {
            Unit = token;
            Name = "Dig";
            AbilityPanelId = 0;
        }

        public Dig(IOwnedTokenWithAbilites token, int id, bool needSelectTarget) : base(id, needSelectTarget)
        {
            Unit = token;
            Name = "Dig";
            AbilityPanelId = 0;
        }
    }
}
