using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability
{
    public class RefineGlass : MakeRecipe
    {
        public RefineGlass(AbilityRecipe recept, IOwnedTokenWithAbilites owner, int id) : base(recept, owner, id)
        {
            Name = "RefineGlass";
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var company = mode.GetCompany(Unit.Owner.Id);
            var result = new List<IAction>();
            var res = company.GetAlignResource(ResourceType.Lime);
            result.Add(new SpendResource(Unit, res.Type, res.Alternative, Unit.Owner));
            IOwnedToken token = Unit;
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
            {
                if (unit is Harvester harvester)
                {
                    token = harvester;
                }
            }
            res = company.GetAlignResource(ResourceType.Glass);
            result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            var area = mode.Area(Unit.X, Unit.Y, 1, false);
            bool isNearFactory = false;
            int ConversionFabric = 0;
            foreach (var field in area)
                foreach (var building in mode.GetTokensByPos(field.X, field.Y))
                {
                    if (building is Fabric)
                        isNearFactory = true;
                    if (building is MeltingFurnace)
                        ConversionFabric += 1;
                }
            if (isNearFactory)
                result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            for (int i = 0; i < ConversionFabric; i++)
                result.Add(new IncomeResource(token, res.Type, res.Alternative, Unit.Owner));
            result.Add(new ForceUpdateUI(true, false));
            return result;
        }
    }

    public class RefineGlassRecept: AbilityRecipe
    {
        public RefineGlassRecept(Player player)
        {
            Resources.Add(new PlanetResource(ResourceType.Lime, ResourceType.None, player.Id));
        }
    }
}
