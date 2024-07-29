using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public sealed class ConstructFloatingPlatfrom : ConstructBuilding
    {
        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            var areas = new List<Vector2I>();
            foreach (var area in mode.Area(Unit.X, Unit.Y, 1))
                if (mode.Map[area.X, area.Y] is Water || mode.Map[area.X, area.Y] is WaterOil)
                    areas.Add(area);
            areas.Add(new Vector2I(Unit.X, Unit.Y));
            foreach(var area in areas)
            {
                bool hasBuilding = false;
                foreach (var token in mode.GetTokensByPos(area.X, area.Y))
                    if (token is Building)
                    {
                        hasBuilding = true;
                        break;
                    }
                if (!hasBuilding)
                    result.Add(area);
            }
            return result;
        }
        public ConstructFloatingPlatfrom(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, true)
        {
            Name = "B. Fl P";
        }
    }
}
