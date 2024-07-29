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
    public class TransportResource : AbilityPresset
    {
        public TransportResource(IOwnedTokenWithAbilites token, int id) : base(id, true)
        {
            Unit = token;
            Id = id;
            Name = "TransportResource";
        }

        public override List<Vector2I> Area(GameMode mode)
        {
            List<Vector2I> area = new List<Vector2I>();
            for (int i = 0; i < mode.Map.Horizontal; i++)
                for (int j = 0; j < mode.Map.Vertical; j++)
                        if (mode.Map[i, j] is not Empty)
                            area.Add(new Vector2I(i, j));
            return area;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            return base.Use(mode, Unit.Position);
        }

    }
}
