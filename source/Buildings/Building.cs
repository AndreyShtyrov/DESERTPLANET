using DesertPlanet.source.Ability;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class Building: IOwnedTokenWithAbilites
    {
        public string Name { get; }
        public string Description { get; set; }

        public int Id { get; }
        public int X { get; set; }

        public int Y { get; set; }

        public Vector2I Position => new Vector2I(X, Y);

        public ActionCounter Counter { get; }
        public Player Owner { get; }

        public int SourceLevel { get; }

        public List<AbilityPresset> Abilities { get; }

        public Building(string name, int x, int y, int layerId, int id, Player owner )
        {
            Owner = owner;
            Name = name;
            Abilities = new List<AbilityPresset>();
            Counter = new ActionCounter(1, 1);
            Id = id;
            SourceLevel = layerId;
            X = x;
            Y = y;
        }

        public AbilityPresset GetAbilityById(int id)
        {
            foreach (var ability in Abilities)
                if (ability.Id == id)
                    return ability;
            return null;
        }

        public virtual Vector2I TileShift => new Vector2I(-1, -1);

        public virtual bool CanMoving => false;
    }
}
