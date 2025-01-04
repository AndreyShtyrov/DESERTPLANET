using DesertPlanet.source.Action;
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
    public class ConstructPark : ConstructBuilding
    {
        private bool CheckTileReadyToBuild(Vector2I pos, GameMode mode)
        {
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
                if (!(unit is FloatPlatform || unit is SolidPlatform || unit is Harvester))
                    return false;
            if (mode.Map[pos.X, pos.Y] is Stone || mode.Map[pos.X, pos.Y] is Sand)
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
            return false;
        }
        public override List<Vector2I> Area(GameMode mode)
        {
            var result = new List<Vector2I>();
            var area = mode.Map[Unit.X, Unit.Y].Neighbors;
            if (!CheckTileReadyToBuild(Unit.Position, mode))
                return result;
            for (int i = 0; i < area.Count - 1; i++)
            {
                var tile1 = area[i];
                var tile2 = area[i + 1];
                if (CheckTileReadyToBuild(tile1, mode) && CheckTileReadyToBuild(tile2, mode))
                    result.Add(tile2);
            }

            return result;
        }
        public ConstructPark(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id, true)
        {
        }

        public override List<IAction> Use(GameMode mode, Vector2I target, List<PlanetResource> resources)
        {
            var result = base.Use(mode, resources);
            var area = mode.Map[Unit.X, Unit.Y].Neighbors;
            result.AddRange(mode.Logic.CreateBuilding(Recipe.Code, target.X, target.Y, Unit.Owner));
            for (int i = 0; i < area.Count - 1; i++)
            {
                if (area[i].X == target.X && area[i].Y == target.Y)
                {
                    result.AddRange(mode.Logic.CreateBuilding(Recipe.Code, area[i + 1].X, area[i + 1].Y, Unit.Owner));
                    break;
                }
            }
            if (result.Count > 0)
            {
                result.Add(new ChangeRepo(Unit.Owner.Id, 3));
            }
            return result;
        }
    }
}
