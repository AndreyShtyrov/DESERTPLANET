using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public enum SelectorState
    {
        SelectUnit = 0,
        SelectAbility = 1,
        SelectTarget = 2,
        SetupAbilityPars = 3,
        SpecifyPars = 4,
        AwaitDialog = 5,
    }
    public class SelectorTools
    {
        public SelectorState State { get; set; }

        private int _unitId = -1;
        public int UnitId {
            get
            {
                return _unitId;
            }
            set
            {
                _unitId = value;
            }
        }

        public Vector2I FirstTarget { get; set; } = new Vector2I(-1, -1);

        private Vector2I _position = Vector2I.Zero;
        public Vector2I Position { 
            get { return _position; } 
            set
            {
                _position = value;
            }
        }

        public int Energy { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Alinium { get; set; } = 0;

        public int Plastic { get; set; } = 0;

        public int Oil { get; set; } = 0;

        public int Cement { get; set; } = 0;

        public int Lime { get; set; } = 0;

        public int Uran { get; set; } = 0;

        public int Glass { get; set; } = 0;

        public int Baskit { get; set; } = 0;

        public List<PlanetResource> SelectedResources { get; set; }
        public List<PlanetResource> Resources { get; set; }
        public int AbilityId { get; set; }

        private GameMode mode;
        public SelectorTools(GameMode gameMode) {
            mode = gameMode;
            State = SelectorState.SelectUnit;
            _unitId = -1;
            AbilityId = -1;
            Resources = new List<PlanetResource>();
            SelectedResources = new List<PlanetResource>();
        }

        public void SetResource()
        {
            Resources = mode.Resources[Position.X, Position.Y].ToList();
            Iron = 0;
            Energy = 0;
            Plastic = 0;
            Oil = 0;
            Uran = 0;
            Glass = 0;
            Baskit = 0;
            Cement = 0;
            Lime = 0;
            Alinium = 0;
            foreach (var resource in Resources)
            {
                if (resource.OwnerId != mode.Player.Id)
                    continue;
                if (resource.Type == ResourceType.Iron)
                    Iron++;
                if (resource.Type == ResourceType.Energy)
                    Energy++;
                if (resource.Type == ResourceType.Plastic)
                    Plastic++;
                if (resource.Type == ResourceType.Oil)
                    Oil++;
                if (resource.Type == ResourceType.Baksits)
                    Baskit++;
                if (resource.Type == ResourceType.Uran)
                    Uran++;
                if (resource.Type == ResourceType.Cement)
                    Cement++;
                if (resource.Type == ResourceType.Lime)
                    Lime++;
                if (resource.Type == ResourceType.Glass)
                    Glass++;
                if (resource.Type == ResourceType.Aliminium)
                    Alinium++;
            }
        }

        private void ClearRes()
        {
            Resources.Clear();
            Energy = 0;
            Iron = 0;
            Plastic = 0;
            Oil = 0;
            Alinium = 0;
            Baskit = 0;
            Uran = 0;
            Cement = 0;
            Glass = 0;
            Lime = 0;
        }

        public void DeselectUnit()
        {
            _unitId = -1;
            AbilityId = -1;
            State = SelectorState.SelectUnit;
            FirstTarget = new Vector2I(-1, -1);
            ClearRes();
            SelectedResources.Clear();
        }

        public void SelectAbility()
        {
            AbilityId = -1;
            FirstTarget = new Vector2I(-1, -1);
            State = SelectorState.SelectAbility;
        }

        public string StringState
        {
            get
            {
                switch (State)
                {
                    case SelectorState.SelectAbility:
                        return " SA ";
                    case SelectorState.SelectTarget:
                        return " ST ";
                    case SelectorState.SetupAbilityPars:
                        return " SP ";
                    case SelectorState.SelectUnit:
                        return " SU ";
                    case SelectorState.SpecifyPars:
                        return " PP ";
                    case SelectorState.AwaitDialog:
                        return "AD";
                }
                return " Er ";
            }
        }
    }
}
