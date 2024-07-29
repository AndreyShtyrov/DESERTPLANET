using DesertPlanet.source.Ability;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies;
using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;
using DesertPlanet.source.Interfaces;

public partial class DecideNotInvariantRecipe : Window
{
    [Export]
    public PackedScene ResourceButton { get; set; }

    public MakeRecipe Ability { get; set; }
    public GameMode GameMode { get; set; }
    public SelectorTools Selector { get; set; }

    private AbilityRecipe Recept { get; set; }
    private Company Company { get; set; }
    private ResourceContainer TempConatiner = null;
    private List<ResourceType> hasAdded = null;
    private Dictionary<ResourceType, ResourceMover> preLoadButtons = new Dictionary<ResourceType, ResourceMover>();

    public override void _Ready()
	{
        var button = ResourceButton.Instantiate<ResourceMover>();

        button.LoadData(ResourceType.Iron);
        preLoadButtons[ResourceType.Iron] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Iron); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Iron); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Plastic);
        preLoadButtons[ResourceType.Plastic] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Plastic); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Plastic); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Glass);
        preLoadButtons[ResourceType.Glass] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Glass); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Glass); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Aliminium);
        preLoadButtons[ResourceType.Aliminium] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Aliminium); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Aliminium); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Cement);
        preLoadButtons[ResourceType.Cement] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Cement); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Cement); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Uran);
        preLoadButtons[ResourceType.Uran] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Uran); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Uran); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Oil);
        preLoadButtons[ResourceType.Oil] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Oil); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Oil); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Lime);
        preLoadButtons[ResourceType.Lime] = button;
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Lime); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Lime); };
        button = ResourceButton.Instantiate<ResourceMover>();
        button.LoadData(ResourceType.Baksits);
        button.ButtonUpDown += () => { IncreaseResource(ResourceType.Baksits); };
        button.ButtonDownDown += () => { DecreaseResource(ResourceType.Baksits); };
        preLoadButtons[ResourceType.Baksits] = button;
    }
    public void SetData(AbilityRecipe recept, AbilityPresset ability)
    {
        if (GameMode == null)
            return;
        Recept = null;
        Company = null;
        TempConatiner = null;
        Recept = recept;
        Ability = ability as MakeRecipe;
        TempConatiner = new ResourceContainer();
        SetupButtonsSet();
        Visible = true;
    }

    private void SetupButtonsSet()
    {
        var HB = GetNode<HBoxContainer>("HBoxContainer");
        for (int i = HB.GetChildCount() - 1; i >= 0; i--)
        {
            HB.RemoveChild(HB.GetChild(i));
        }
        hasAdded = new List<ResourceType>();
        foreach (var resource in Recept.Resources)
        {
            if (!hasAdded.Contains(resource.Type))
                hasAdded.Add(resource.Type);
            if (!hasAdded.Contains(resource.Alternative) && resource.Alternative != ResourceType.None)
                hasAdded.Add(resource.Alternative);
        }
        var company = GameMode.GetCompany(GameMode.ActivePlayer.Id);
        Company = company;
        foreach (var resource in hasAdded)
        {
            HB.AddChild(preLoadButtons[resource]);
            preLoadButtons[resource].TextValue.Text = "0";
            preLoadButtons[resource].ButtonDown.Visible = true;
        }
    }
    private void IncreaseResource(ResourceType type)
    {
        var res = Company.GetAlignResource(type);
        TempConatiner.Add(res);
        UpdateVisual();
    }
    private void DecreaseResource(ResourceType type)
    {
        var res = Company.GetAlignResource(type);
        TempConatiner.Remove(res);
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach (var resource in hasAdded)
        {
            int count = 0;
            foreach (var res in TempConatiner)
                if (res.Type == resource)
                    count += 1;
            preLoadButtons[resource].TextValue.Text = count.ToString();
        }
    }

    public void OnConfirm()
    {
        if (!Ability.NeedSelecTarget)
        {
            var actions = GameMode.Logic.UseMakeReceptAbility(Ability, TempConatiner.ToList());
            GameMode.ActionManager.ApplyActions(actions);
            Selector.DeselectUnit();
            Visible = false;
        }
        else
        {
            Selector.State = SelectorState.SelectTarget;
            Selector.SelectedResources.AddRange(TempConatiner.ToList());
            GameMode.NeedDrawAbilityArea = true;
            Visible = false;
        }
    }
}
