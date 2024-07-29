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
    public class ShiftResource : AbilityPresset
    {
        public ShiftResource(IOwnedTokenWithAbilites unit, int id) : base(id, true)
        {
            Unit = unit;
            Name = "ShiftResource";
            Id = id;
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            return true;
        }

        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            foreach (var field in mode.Map[Unit.X, Unit.Y].Neighbors)
                if (mode.Map[field.X, field.Y] is not Empty)
                    result.Add(field);
            result.Add(Unit.Position);
            return result;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            return new List<IAction>();
        }
    }
}
