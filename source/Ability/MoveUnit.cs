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
    public class MoveUnit : AbilityPresset
    {

        public override List<Vector2I> Area(GameMode mode)
        {
            return new List<Vector2I>();
        }


        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            if (mode.Map[target.X, target.Y] is Empty)
                return new List<IAction>();
            var result = new List<IAction> { new Move(Unit.Id, target.X, target.Y) };
            result.AddRange(base.Use(mode, target));
            result.Add(new ForceUpdateUI(true, true));
            return result;
        }
         
        public MoveUnit(IOwnedTokenWithAbilites token, int id): base(id, true)
        {
            Unit = token;
            Name = "Move";
        }
    }
}
