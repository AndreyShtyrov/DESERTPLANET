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
    public class ComplexBuildings: IHasAbilities
    {
        public string Name { get; set; }

        public List<Vector2I> Positions { get; }

        public List<AbilityPresset> Abilities { get; }

        public ActionCounter Counter { get; set; }

        public bool CanMoving => false;

        public int Id { get; set; }

        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Player Owner => throw new NotImplementedException();

        public Vector2I Position => throw new NotImplementedException();

        public bool CanBuild { get; internal set; }

        public ComplexBuildings(int id) { 
            Id = id;
            Positions = new List<Vector2I>();
            Counter = new ActionCounter(0, 1);
            Abilities = new List<AbilityPresset>();
            CanBuild = false;
        }
        public bool InBound(int X, int Y)
        {
            throw new NotImplementedException();
        }

        public bool InBound(Vector2I pos)
        {
            throw new NotImplementedException ();
        }

        public AbilityPresset GetAbilityById(int id)
        {
            throw new NotImplementedException();
        }

        public bool HasAbility(int id)
        {
            throw new NotImplementedException();
        }
    }
}
