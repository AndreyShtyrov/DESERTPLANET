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

namespace DesertPlanet.source.Ability
{
    public class ConstructBuilding : AbilityPresset
    {
        public BuildingRecipe Recipe { get; set; }
        public ConstructBuilding(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(id, false)
        {
            Unit = token;
            AbilityPanelId = 1;
            Recipe = recept;
        }
        public ConstructBuilding(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id, bool needSelectTarget) : base(id, needSelectTarget)
        {
            Unit = token;
            AbilityPanelId = 1;
            Recipe = recept;
        }

        public override List<Vector2I> Area(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            if (!base.IsReadyToUse(mode))
                return false;
            bool IsFieldAcceptable = false;
            var field = mode.Map[Unit.X, Unit.Y];
            if (field is Stone && Recipe is not DrilleRecept)
                IsFieldAcceptable = true;
            if (Recipe is DrilleRecept)
            {
                if (field is StoneOil)
                    IsFieldAcceptable = true;
                if (field is StoneIron)
                    IsFieldAcceptable = true;
                if (field is WaterOil)
                    IsFieldAcceptable = true;
                if (!IsFieldAcceptable)
                    return false;
            }
            if (field is Sand)
            {
                foreach(var building in mode.Buildings.Values)
                {
                    if (!(building.X == Unit.X && building.Y == Unit.Y))
                        continue;
                    if (building is SolidPlatform)
                    {
                        IsFieldAcceptable = true; break;
                    }
                }
            }
            if (field is Water) {
                foreach (var building in mode.Buildings.Values)
                {
                    if (!(building.X == Unit.X && building.Y == Unit.Y))
                        continue;
                    if (building is FloatPlatform)
                    {
                        IsFieldAcceptable = true; break;
                    }
                }
            }
            if (Unit is Harvester harvester)
            {
                var source = harvester.Resources.Copy();
                source.AddRange(mode.Map[Unit.X, Unit.Y].Resources);
                if (source.HasSubSeqByTypes(Recipe.Resources))
                    return true;
            }
            else
            {
                var source = mode.Map[Unit.X, Unit.Y].Resources.Copy();
                if (source.HasSubSeq(Recipe.Resources))
                    return true;
            }
            return false;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            return mode.Logic.CreateBuilding(Recipe.Code, target.X, target.Y, Unit.Owner);
        }

        public List<IAction> Use(GameMode mode, List<PlanetResource> resources)
        {
            var result = new List<IAction>();
            if (!Recipe.Resources.HasSubSeqByTypes(resources))
                return new List<IAction>();
            result.AddRange(base.Use(mode, new Vector2I(Unit.X, Unit.Y)));
            foreach (var res in resources)
            {
                var rs = mode.GetCompany(Unit.Owner.Id).GetAlignResource(res.Type);
                result.Add(new SpendResource(Unit.Id, Unit.X, Unit.Y, rs.Type, rs.Alternative));
            }
            result.AddRange(Use(mode, new Vector2I(Unit.X, Unit.Y)));
            
            return result;
        }

        public virtual List<IAction> Use(GameMode mode, Vector2I target, List<PlanetResource> resources)
        {
            var result = new List<IAction>();
            if (!Area(mode).Contains(target))
                return new List<IAction>();
            if (!Recipe.Resources.HasSubSeqByTypes(resources))
                return new List<IAction>();
            foreach (var res in resources)
            {
                var rs = mode.GetCompany(Unit.Owner.Id).GetAlignResource(res.Type);
                result.Add(new SpendResource(Unit.Id, Unit.X, Unit.Y, rs.Type, rs.Alternative));
            }
            return result;
        }
    }
}
