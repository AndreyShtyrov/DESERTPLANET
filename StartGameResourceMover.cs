using DesertPlanet.source;
using DesertPlanet.source.Action;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;

public partial class StartGameResourceMover : Window
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public PackedScene ResourceButtonScene { get; set; }

	public GameMode GameMode { get; set; }

    public ResourceContainer FirstHarvester { get; set; }

    public ResourceContainer SecondHarvester { get; set; }

    public ResourceContainer ThirdHarvester { get; set; }

    private Dictionary<ResourceType, ResourceMover> FirstVisual = new Dictionary<ResourceType, ResourceMover>();

    private ResourceContainer Stack { get; set; }

    private Dictionary<ResourceType, ResourceMover> StackVisual = new Dictionary<ResourceType, ResourceMover>();

	private Dictionary<ResourceType, ResourceMover> SecondVisual = new Dictionary<ResourceType, ResourceMover>();

    private Dictionary<ResourceType, ResourceMover> ThirdVisual = new Dictionary<ResourceType, ResourceMover>();

    private List<Harvester> Harvesters { get; set; }

	public HBoxContainer HBLine1 { get; set; }

	public HBoxContainer HBLine2 { get; set; }

	public HBoxContainer HBLine3 { get; set; }

    public HBoxContainer HBLine4 { get; set; }

	private List<ResourceType> hasResourceTypes = new List<ResourceType>();
    public override void _Ready()
	{
		HBLine1 = GetNode<HBoxContainer>("VB/Line1");
        HBLine2 = GetNode<HBoxContainer>("VB/Line2");
        HBLine3 = GetNode<HBoxContainer>("VB/Line3");
        HBLine4 = GetNode<HBoxContainer>("VB/Line4");
    }

	public void SetData(List<Harvester> harvesters, List<PlanetResource> startResource, Player player)
	{
		Stack = new ResourceContainer();
        FirstHarvester = new ResourceContainer();
        SecondHarvester = new ResourceContainer();
        ThirdHarvester = new ResourceContainer();

		StackVisual = new Dictionary<ResourceType, ResourceMover>();
		FirstVisual = new Dictionary<ResourceType, ResourceMover>();
		SecondVisual = new Dictionary<ResourceType, ResourceMover>();
		ThirdVisual = new Dictionary<ResourceType, ResourceMover>();
        Harvesters = harvesters;
		hasResourceTypes.Clear();
		hasResourceTypes = new List<ResourceType>();
		foreach(var resource in startResource)
		{
			if (!hasResourceTypes.Contains(resource.Type))
				hasResourceTypes.Add(resource.Type);
			Stack.Add(resource);
		}
		int source = 0;
		source = 0;
		foreach(var resource in  hasResourceTypes)
		{
			var visual = ResourceButtonScene.Instantiate<ResourceMover>();
			visual.LoadData(resource);
			HBLine1.AddChild(visual);
            visual.TextValue.Text = Stack.CountResourceWithType(resource).ToString();
            visual.ButtonDown.Visible = false;
            visual.ButtonUp.Visible = false;
			StackVisual[resource] = visual;
        }
        foreach (var resource in hasResourceTypes)
		{
			var visual = ResourceButtonScene.Instantiate<ResourceMover>();
			visual.LoadData(resource);
			visual.ButtonDownDown += () => { UpdateResource(1, source, resource); };
			visual.ButtonUpDown += () => { UpdateResource(source, 1, resource); };
			HBLine2.AddChild(visual);
			visual.ButtonUp.Visible = true;
            visual.ButtonDown.Visible = true;
            visual.TextValue.Text = "0";
			FirstVisual[resource] = visual;
        }
        foreach (var resource in hasResourceTypes)
        {
            var visual = ResourceButtonScene.Instantiate<ResourceMover>();
            visual.LoadData(resource);
            visual.ButtonDownDown += () => { UpdateResource(2, source, resource); };
            visual.ButtonUpDown += () => { UpdateResource(source, 2, resource); };
			HBLine3.AddChild(visual);
            visual.ButtonUp.Visible = true;
            visual.ButtonDown.Visible = true;
            visual.TextValue.Text = "0";
			SecondVisual[resource] = visual;
        }
		if (harvesters.Count != 3)
			return;
        foreach (var resource in hasResourceTypes)
        {
            var visual = ResourceButtonScene.Instantiate<ResourceMover>();
            visual.LoadData(resource);
            visual.ButtonDownDown += () => { UpdateResource(3, source, resource); };
            visual.ButtonUpDown += () => { UpdateResource(source, 3, resource); };
			HBLine4.AddChild(visual);
            visual.ButtonUp.Visible = true;
            visual.ButtonDown.Visible = true;
            visual.TextValue.Text = "0";
			ThirdVisual[resource] = visual;
        }
    }

	private void UpdateResource(int toSource, int fromSource, ResourceType type)
	{
		Nullable<PlanetResource> resource = null;
		switch(fromSource)
		{
			case 0:
				{
					resource = Stack.PopupByType(type); break;
				}
			case 1:
				{
					resource = FirstHarvester.PopupByType(type); break;
				}
			case 2:
				{
					resource = SecondHarvester.PopupByType(type); break;
				}
			case 3:
				{
					resource = ThirdHarvester.PopupByType(type); break;
				}
		}
		if (!resource.HasValue)
			return;
		switch(toSource)
		{
			case 0:
				{
					if (resource == null)
						break;
					Stack.Add(resource.Value);
					break;
				}
			case 1:
				{
					if (resource == null)
						break;
					FirstHarvester.Add(resource.Value);
					break;
				}
			case 2:
				{
					if (resource == null)
						break;
					SecondHarvester.Add(resource.Value);
					break;
				}
			case 3:
                {
                    if (resource == null)
                        break;
					ThirdHarvester.Add(resource.Value);
					break;
                }
		}
		UpdateVisual();
	}

	private void UpdateVisual()
	{
        foreach (var resource in hasResourceTypes)
			StackVisual[resource].TextValue.Text = Stack.CountResourceWithType(resource).ToString();
        foreach (var resource in hasResourceTypes)
			FirstVisual[resource].TextValue.Text = FirstHarvester.CountResourceWithType(resource).ToString();
		foreach (var resource in hasResourceTypes)
			SecondVisual[resource].TextValue.Text = SecondHarvester.CountResourceWithType(resource).ToString();
		if (Harvesters.Count == 3)
			foreach (var resource in hasResourceTypes)
				ThirdVisual[resource].TextValue.Text = ThirdHarvester.CountResourceWithType(resource).ToString();
    }

	public void OnConfirm()
	{
		var actions = new List<IAction>();
		if (Stack.Count > 0)
			return;
        var harvester = Harvesters[0];
        foreach (var resource in FirstHarvester)
			actions.Add(new IncomeResource(harvester, resource.Type, resource.Alternative, harvester.Owner));
		
		harvester = Harvesters[1];
        foreach (var resource in SecondHarvester)            
            actions.Add(new IncomeResource(harvester, resource.Type, resource.Alternative, harvester.Owner));
		if (Harvesters.Count == 3)
		{
            harvester = Harvesters[2];
            foreach (var resource in ThirdHarvester)
                actions.Add(new IncomeResource(harvester, resource.Type, resource.Alternative, harvester.Owner));
        }
		actions.AddRange(GameMode.Logic.EndPlayerDeploy());
		GameMode.ActionManager.ApplyActions(actions);
    }
}
