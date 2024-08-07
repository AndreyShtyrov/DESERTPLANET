using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public interface FieldTileShifts
    {

        public Vector2I TileShift { get; }

        public static Vector2I WaterTileShift = new Vector2I(3, 1);

        public static Vector2I WaterOilTileShift = new Vector2I(3, 2);

        public static Vector2I SandTileShift = new Vector2I(2, 0);

        public static Vector2I SandBokcitTileShift = new Vector2I(0, 1);

        public static Vector2I SandLimeTileShift = new Vector2I(1, 0);

        public static Vector2I StoneTileShift = new Vector2I(0, 2);

        public static Vector2I StoneOilTileShift = new Vector2I(1, 2);

        public static Vector2I StoneIronTileShift = new Vector2I(2, 2);

        public static Vector2I SpaceDockTileShift = new Vector2I(0, 0);

        public static Vector2I EmptyTileShift = new Vector2I(-1, -1);

        public static Vector2I BlockedTileShift = new Vector2I(3, 0);
    }
}
