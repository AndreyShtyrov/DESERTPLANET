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
    public class MoveFloatingPlatform : AbilityPresset
    {
        public MoveFloatingPlatform(IOwnedTokenWithAbilites token, int id) : base(id, true)
        {
            Unit = token;
            Name = "Move Float P.";
        }

        public override List<Vector2I> Area(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            List<IOwnedToken> tokens = new List<IOwnedToken>();
            foreach (var token in mode.GetTokensByPos(Unit.X, Unit.Y))
                tokens.Add(token);
            foreach (var harvester in mode.Harvesters.Values)
                if (harvester.X == Unit.X && harvester.Y == Unit.Y)
                {
                    if (tokens.Contains(harvester))
                        continue;
                    tokens.Add(harvester);
                }
            if (mode.Map[target.X, target.Y] is Empty)
                return new List<IAction>();
            if (!(mode.Map[target.X, target.Y] is Water))
                return new List<IAction>();
            var result = new List<IAction>();
            foreach (var token in tokens)
            {
                new Move(token.Id, target.X, target.Y);
            }
            result.AddRange(base.Use(mode, target));
            result.Add(new ForceUpdateUI(true, true));
            return result;
        }
    }
}
