using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability.Constructs
{
    public class ConstructCompanyOffice : ConstructBuilding
    {
        public ConstructCompanyOffice(BuildingRecipe recept, IOwnedTokenWithAbilites token, int id) : base(recept, token, id)
        {
        }

        public override List<IAction> Use(GameMode mode, Vector2I target)
        {
            var result = base.Use(mode, target);
            if (result.Count > 0)
            {
                foreach(var player in mode.PlayerList)
                {
                    result.Add(new ChangeAccessToSpecialBuildings(player.IsHydroponic, true, player));
                }
            }
            return result;
        }
    }
}
