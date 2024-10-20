using DesertPlanet.source.Action;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Ability
{
    public enum AbilityType
    {
        Move = 0,
        Construct = 1,
        Refine = 2,
        Dig = 3,
        Deconstruct = 4,
        Action = 5
    }

    public abstract class AbilityPresset
    {
        public string Name;
        public string Description;
        public AbilityType AbilityType;

        public AbilityInfo Info { get; set; }
        public bool NeedSelecTarget { get; }
        public IOwnedTokenWithAbilites Unit { get; set; }
        public int Id { get; set; }
        public string SourceImage { get; set; }
        public int AbilityPanelId { get; set; }
        public bool ShowInBulidingMenu { get; set; } = false;
        public virtual List<IAction> Use(GameMode mode, Vector2I target)
        {
            var result = new List<IAction>();
            var needEnergy = Unit.Counter.PredictEnergyChange(1);
            var sources = mode.GetEnergy(Unit);
            foreach( var source in sources )
            {
                if (source.Resources.Energy >= needEnergy)
                {
                    for (var i = 0; i < needEnergy; i++)
                        result.Add(new SpendResource(source, ResourceType.Energy, ResourceType.None, Unit.Owner));
                    break;
                }
                else
                {
                    for (var i = 0; i < source.Resources.Energy; i++)
                        result.Add(new SpendResource(source, ResourceType.Energy, ResourceType.None, Unit.Owner));
                    needEnergy -= source.Resources.Energy;
                }
            }
            result.Add(new IncreaseActionCount(Unit.Id, 1));
            result.Add(new ForceUpdateUI(true, false));
            return result;
        }

        public virtual bool IsReadyToUse(GameMode mode)
        {
            var needEnergy = Unit.Counter.PredictEnergyChange(1);
            var sources = mode.GetEnergy(Unit);
            var energy = 0;
            foreach (var source in sources)
            {
                energy += source.Resources.Energy;
            }
            if (energy >= needEnergy)
                return true;
            else return false;
        }

        public abstract List<Vector2I> Area(GameMode mode);

        public abstract void Return();

        public AbilityPresset(int id, bool needSelectTarget)
        {
            Id = id;
            NeedSelecTarget = needSelectTarget;
            Info = new AbilityInfo();
        }

        public AbilityPresset(int id)
        {
            Id = id;
            Info = new AbilityInfo();
        }
    }
}
