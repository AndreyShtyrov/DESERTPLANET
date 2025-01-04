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

        private bool CheckTileReadyToBuild(Vector2I pos, GameMode mode)
        {
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                if (!(unit is FloatPlatform || unit is SolidPlatform || unit is Harvester))
                    return false;
            if (mode.Map[pos.X, pos.Y] is Stone)
                return true;
            if (mode.Map[pos.X, pos.Y] is Water)
            {
                bool canBuild = false;
                foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                    if (unit is FloatPlatform)
                        canBuild = true;
                if (canBuild)
                    return true;
                else
                    return false;
            }
            if (mode.Map[pos.X, pos.Y] is Sand)
            {
                bool canBuild = false;
                foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                    if (unit is SolidPlatform)
                        canBuild = true;
                if (canBuild)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            if (!(CheckTileReadyToBuild(Unit.Position, mode)))
                return result;
            var area = mode.Map[Unit.X, Unit.Y].Neighbors;
            foreach (var field in area)
                if (CheckTileReadyToBuild(field, mode))
                    result.Add(field);
            return result;
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
