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
    public sealed class ConstructTidalStation : ConstructBuilding
    {
        public ConstructTidalStation(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, true)
        {
            Name = "B. Tidal";
        }

        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            var area = mode.Area(Unit.X, Unit.Y, 1);
            foreach(var field in area)
            {
                if (field.X == Unit.X && field.Y == Unit.Y)
                    continue;
                if (mode.Map[field.X, field.Y] is Water || mode.Map[field.X, field.Y] is WaterOil)
                    result.Add(field);
            }
            return result;
        }

        public override List<IAction> Use(GameMode mode, Vector2I target, List<PlanetResource> resources)
        {
            if (!(mode.Map[target.X, target.Y] is Water || mode.Map[target.X, target.Y] is WaterOil))
                return new List<IAction>();
            var result = base.Use(mode, resources);
            result.AddRange(mode.Logic.CreateBuilding(Recipe.Code, Unit.X, Unit.Y, Unit.Owner));
            result.AddRange(mode.Logic.CreateBuilding(Recipe.Code + 1, target.X, target.Y, Unit.Owner));
            return result; 
        }
    }
}
