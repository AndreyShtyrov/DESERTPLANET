using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Buildings
{
    public class FloatPlatform : Building
    {
        public FloatPlatform(int x, int y, int layerId, int id, Player owner) 
            : base("FloatPlatform", x, y, layerId, id, owner)
        {

        }
        public override bool CanMoving => true;
        public override Vector2I TileShift => new Vector2I(0, 2);
    }

    public class FloatPlatformReceipt : BuildingRecipe
    {
        public FloatPlatformReceipt(Player player) : base(5)
        {
            Resources.Add(new PlanetResource(ResourceType.Plastic, ResourceType.None, player.Id));
            Info.Name = "Build Platform";
            Info.Recipe = Resources;
        }
    }

}
