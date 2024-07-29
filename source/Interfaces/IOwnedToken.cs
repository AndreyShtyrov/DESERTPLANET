using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Interfaces
{
    public interface IOwnedToken
    {
        public string Name { get; }
        public int Id { get; }

        public int X { get; set; }

        public int Y { get; set; }

        public Player Owner { get; }

        public Vector2I Position { get; }

    }
}
