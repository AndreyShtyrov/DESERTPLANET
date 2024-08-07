using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class Tile
    {
        public int X { get; }

        public int Y { get; }

        private List<Vector2I> neighbors = null;

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

        [JsonIgnore]
        public List<Vector2I> Neighbors
        {
            get
            {
                if (neighbors != null)
                    return neighbors;
                var result = new List<Vector2I>();
                if (X % 2 == 0)
                {
                    result.Add(new Vector2I(X + 0, Y - 1));
                    result.Add(new Vector2I(X + 1, Y));
                    result.Add(new Vector2I(X + 1, Y + 1));
                    result.Add(new Vector2I(X, Y + 1));
                    result.Add(new Vector2I(X - 1, Y + 1));
                    result.Add(new Vector2I(X - 1, Y));
                }
                else
                {
                    result.Add(new Vector2I(X + 0, Y - 1));
                    result.Add(new Vector2I(X + 1, Y - 1));
                    result.Add(new Vector2I(X + 1, Y));
                    result.Add(new Vector2I(X, Y + 1));
                    result.Add(new Vector2I(X - 1, Y));
                    result.Add(new Vector2I(X - 1, Y - 1));
                }
                neighbors = result;
                return result;
            }
        }
    }
}
