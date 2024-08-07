using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class SelectionTile : Tile
    {
        public int Value { get; set; }
        public int LayerSource { get; } 
        public SelectionTile(int value, int x, int y) : base(x, y)
        {
            Value = value;
            GetTile = new Vector2I(-1, -1);
            if (Value > 0 && Value <= 4)
                GetTile = new Vector2I(Value - 1, 1);
            if (Value > 4 && Value <= 8)
                GetTile = new Vector2I(Value - 1, 2);
        }

        public Vector2I GetTile { get; }
    }
}
