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
    public class BurnOil : MakeRecipe
    {
        public BurnOil(AbilityRecipe recept, IOwnedTokenWithAbilites owner, int id) : base(recept, owner, id)
        {
            Name = "BurnOil";
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var company = mode.GetCompany(Unit.Owner.Id);
            var result = new List<IAction>();
            var res = company.GetAlignResource(ResourceType.Oil);
            result.Add(new SpendResource(Unit, res.Type, res.Alternative, Unit.Owner));
            IOwnedToken token = Unit;
            foreach (var unit in mode.GetTokensByPos(Unit.X, Unit.Y))
            {
                if (unit is Harvester harvester)
                {
                    token = harvester;
                    break;
                }
            }
            result.Add(new IncreaseEnergy(Unit.Id, 6));
            result.Add(new ForceUpdateUI(true, false));
            return result;
        }
    }

    public class BurnOilRecept: AbilityRecipe
    {

        public BurnOilRecept(Player player)
        {
            Resources.Add(new PlanetResource(ResourceType.Oil, ResourceType.None, player.Id));
        }
    }
}
