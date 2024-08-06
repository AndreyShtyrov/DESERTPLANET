using DesertPlanet.source.Ability;
using DesertPlanet.source.Action;
using DesertPlanet.source.Companies.Projects;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

namespace DesertPlanet.source
{
    public class GameLogic
    {
        private GameMode Game;

        public static int MAXHARVESTERS = 4;

        public GameLogic(GameMode mode) {
            Game = mode;
        }

        public void UseAbility(AbilityPresset ability)
        {
            if (Game.ActivePlayer != Game.Player)
                return;
            UseAbility(ability, new Vector2I(ability.Unit.X, ability.Unit.Y));
        }

        public void UseAbility(AbilityPresset ability, Vector2I vector2I)
        {
            if (Game.ActivePlayer != Game.Player)
                return;
            if (ability.IsReadyToUse(Game))
            {
                var actions = ability.Use(Game, vector2I);
                Game.ActionManager.ApplyActions(actions);
            }
        }

        public List<IAction> UseConstructAbility(ConstructBuilding ability, List<PlanetResource> resources)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            List<IAction> result = new List<IAction>();
            if (ability.IsReadyToUse(Game))
            {
                result.AddRange(ability.Use(Game, resources));
            }
            return result;
        }

        public List<IAction> UseMakeReceptAbility(MakeRecipe ability, List<PlanetResource> resources)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            List<IAction> result = new List<IAction>();
            if (ability.IsReadyToUse(Game, resources))
            {
                result.AddRange(ability.Use(Game, resources));
            }
            return result;
        }
        public List<IAction> CreateHarvester(int x, int y, Player player)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            int count = 0;
            foreach (var harvester in Game.Harvesters.Values)
                if (player == harvester.Owner)
                    count++;
            if (count > MAXHARVESTERS)
                return new List<IAction>();
            var result = new List<IAction>() { new CreateHarvester(Game.UnitId, x, y, player) };
            result.Add(new RemoveStartHarvester(player.Id));
            Game.UnitId++;
            result.Add(new ForceUpdateUI(true, false));
            return result;
        }

        public List<IAction> BuyProject(Player player, CompanyProject project)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            var result = new List<IAction>();
            if (player.Repos < project.Repo)
                return result;
            result.Add(new ChangeRepo(player.Id, -project.Repo));
            result.Add(new BuyProject(player.Id, project.Id));
            result.Add(new ApplySpecificProjectSettings(player, project));
            return result;
        }
        public List<IAction> CreateBuilding(int code, int x, int y, Player player)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            var result = new List<IAction>();
            result.Add(new CreateBulding(Game.UnitId, x, y, code, player));
            Game.UnitId++;
            return result;
        }

        private List<IAction> GenerateMovingResource(int diff, PlanetResource res, object fromToken, object toToken)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            var result = new List<IAction>();
            if (diff > 0)
            {
                for (int i = 0; i < Math.Abs(diff); i++)
                {
                    result.Add(new IncomeResource(fromToken, res.Type, res.Alternative, Game.Player));
                    result.Add(new SpendResource(toToken, res.Type, res.Alternative, Game.Player));
                }
            }
            else
            {
                for (int i = 0; i < Math.Abs(diff); i++)
                {
                    result.Add(new SpendResource(fromToken, res.Type, res.Alternative, Game.Player));
                    result.Add(new IncomeResource(toToken, res.Type, res.Alternative, Game.Player));
                }
            }
            return result;
        }

        public List<IAction> MoveResource(IHasResource fromToken, IHasResource toToken, 
            ResourceContainer fromLast)
        {
            if (Game.ActivePlayer != Game.Player)
                return new List<IAction>();
            var company = Game.GetCompany(Game.Player.Id);
            var result = new List<IAction>();
            int diff = 0;
            if (fromToken.Resources.Iron != fromLast.Iron)
            {
                diff = fromLast.Iron - fromToken.Resources.Iron;
                var res = company.GetAlignResource(ResourceType.Iron);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Alinium != fromLast.Alinium)
            {
                diff = fromLast.Alinium - fromToken.Resources.Alinium;
                var res = company.GetAlignResource(ResourceType.Aliminium);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Glass != fromLast.Glass)
            {
                diff = fromLast.Glass - fromToken.Resources.Glass;
                var res = company.GetAlignResource(ResourceType.Glass);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Plastic != fromLast.Plastic)
            {
                diff = fromLast.Plastic - fromToken.Resources.Plastic;
                var res = company.GetAlignResource(ResourceType.Plastic);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Oil != fromLast.Oil)
            {
                diff = fromLast.Oil - fromToken.Resources.Oil;
                var res = company.GetAlignResource(ResourceType.Oil);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Baskit != fromLast.Baskit)
            {
                diff = fromLast.Baskit - fromToken.Resources.Baskit;
                var res = company.GetAlignResource(ResourceType.Baksits);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Lime != fromLast.Lime)
            {
                diff = fromLast.Lime - fromToken.Resources.Lime;
                var res = company.GetAlignResource(ResourceType.Lime);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Cement != fromLast.Cement)
            {
                diff = fromLast.Cement - fromToken.Resources.Cement;
                var res = company.GetAlignResource(ResourceType.Cement);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            if (fromToken.Resources.Uran != fromLast.Uran)
            {
                diff = fromLast.Uran - fromToken.Resources.Uran;
                var res = company.GetAlignResource(ResourceType.Uran);
                result.AddRange(GenerateMovingResource(diff, res, fromToken, toToken));
            }
            return result;
        }

        public List<IAction> StartActions()
        {
            var result = new List<IAction>();
            if (Game.Player == Game.ActivePlayer)
            {
                result.Add(new ChangeGameState(Game.Player.Id, Game.State, GameState.Deploy));
            }
            return result;
        }
    }
}
