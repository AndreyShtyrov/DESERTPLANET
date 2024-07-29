using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability
{
    public class MakeRecipe : AbilityPresset
    {

        public MakeRecipe(AbilityRecipe recept, IOwnedTokenWithAbilites owner, int id): base(id, false)
        {
            Recipe = recept;
            Unit = owner;
        }

        public AbilityRecipe Recipe { get; set; }
        public override List<Vector2I> Area(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadyToUse(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsReadyToUse(GameMode mode, List<PlanetResource> resources)
        {
            if (!base.IsReadyToUse(mode))
                return false;
            var container = mode.Map[Unit.X, Unit.Y].Resources.Copy();
            foreach(var token in mode.GetTokensByPos(Unit.X, Unit.Y))
                if (token is IHasResource hasResource)
                    container.AddRange(hasResource.Resources);
            if (container.HasSubSeqByTypes(resources))
                return true;
            return false;
        }

        public override void Return()
        {
            throw new NotImplementedException();
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            throw new NotImplementedException();
        }

        public virtual List<IAction> Use(GameMode mode, List<PlanetResource> resources)
        {
            var company = mode.GetCompany(Unit.Owner.Id);
            var container = ResourceContainer.CreateFromList(resources);
            var result = new List<IAction>();
            while (container.HasSubSeqByTypes(Recipe.Resources) && container.Count > 0)
            {
                foreach(var res in resources)
                {
                    var r = company.GetAlignResource(res.Type);
                    container.Remove(r);
                }
                result.AddRange(Use(mode, new Vector2I(Unit.X, Unit.Y)));
            }
            if (!(this is BurnOil))
                result.AddRange(base.Use(mode, new Vector2I(Unit.X, Unit.Y)));
            return result;
        }

        public virtual List<IAction> Use(GameMode mode, Vector2I target ,List<PlanetResource> resources)
        { throw new NotImplementedException(); }
    }
}
