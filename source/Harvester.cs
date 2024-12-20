﻿using DesertPlanet.source.Ability;
using DesertPlanet.source.Action;
using DesertPlanet.source.Companies;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public class Harvester : IOwnedTokenWithAbilites, ICanBuilding, IHasStartTurnAction, IHasResource
    {
        public int Id { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2I Position { get {return new Vector2I(X, Y); } }

        public string Name { get; } = "Harvester";

        public ActionCounter Counter { get; }

        public Player Owner { get; }
        public ResourceContainer Resources { get; }

        public List<AbilityPresset> Abilities { get; }

        public Harvester(int id, int x, int y, Player owner, GameMode gameMode)
        {
            Id = id;
            X = x;
            Y = y;
            Resources = new ResourceContainer();
            Owner = owner;
            Abilities = new List<AbilityPresset>();
            Counter = new ActionCounter(1, 1);
            Abilities.Add(new MoveUnit(this, 0));
            Abilities.Add(new Dig(this, 31));
            var company = gameMode.GetCompany(owner.Id);
            Abilities.AddRange(company.GetCounstructAbility(this, 1));
            int countHarvesters = 0;
            foreach (var unit in gameMode.Harvesters.Values)
            {
                if (unit.Owner == owner)
                { countHarvesters++; }
            }
            TileShift = new Vector2I(countHarvesters, company.VerticalShiftForHarvesterTile);
        }
        public Vector2I TileShift { get; }

        public int EndTurnEnergyCotainer { get; set; }

        public bool HasStartTurnAction => true;

        public bool CanMoving => true;

        public bool CanBuild => true;

        public virtual List<IAction> StartTurnActions()
        {
            return new List<IAction>() { new IncreaseEnergy(Id, 2) };
        }

        public AbilityPresset GetAbilityById(int id)
        {
            foreach (var ability in Abilities) { 
                if (ability.Id == id) {  return ability; }
            }
            return null;
        }

        public AbilityPresset GetAbilityByRecipt(IRecipe recipe)
        {
            foreach(var ability in Abilities)
            {
                if (ability is ConstructBuilding construct)
                {
                    if (construct.Recipe == recipe)
                        return construct;
                }
            }
            throw new IndexOutOfRangeException("Cannot found Ability with this Recipe");
        }

        public bool HasAbility(int id)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Id == id) { return true; }
            }
            return false;
        }
    }
}
