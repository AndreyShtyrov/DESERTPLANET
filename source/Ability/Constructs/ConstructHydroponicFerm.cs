using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public class ConstructHydroponicFerm : ConstructBuilding
    {
        public override List<Vector2I> Area(GameMode mode)
        {
            var areas = new List<Vector2I>();
            bool canBuild = false;
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                if (!(unit is FloatPlatform || unit is SolidPlatform || unit is Harvester))
                    return new List<Vector2I>();
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                if (unit is FloatPlatform || unit is SolidPlatform)
                    canBuild = true;
            if (mode.Map[Unit.X, Unit.Y] is Stone)
                canBuild = true;
            if (!canBuild)
                return new List<Vector2I>();
            foreach (var area in mode.Area(Unit.X, Unit.Y, 1, false))
                if (mode.Map[area.X, area.Y] is Stone)
                    areas.Add(area);
            foreach (var area in mode.Area(Unit.X, Unit.Y, 1, false))
                foreach (var unit in mode.GetTokensByPos(area.X, area.Y))
                    if (unit is FloatPlatform && unit is SolidPlatform)
                        areas.Add(area);
            return areas;
        }
        public ConstructHydroponicFerm(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, true)
        {
        }

        public override List<IAction> Use(GameMode mode, Vector2I target, List<PlanetResource> resources)
        {
            var result = base.Use(mode, resources);
            result.AddRange(mode.Logic.CreateBuilding(Recipe.Code, target.X, target.Y, Unit.Owner));
            if (result.Count > 0)
            {
                result.Add(new ChangeAccessToSpecialBuildings(true, Unit.Owner.IsOffice, Unit.Owner));
            }
            return result;
        }
    }
}
