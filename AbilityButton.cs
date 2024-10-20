using DesertPlanet.source;
using DesertPlanet.source.Ability;
using Godot;
using System;
using DesertPlanet.source.Interfaces;

public partial class AbilityButton : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public AbilityInfo AbilityInfo { get; set; }

	[Export]
	public int ActionId { get; set; }

	public event ActionOnId ApplyActionEvent;

	public LoadInfo RequestAbilityInfo;

	private Label AbilityNameText { get; set; }

	private Timer ShowHelpTimer { get; set; }
	public override void _Ready()
	{
		ShowHelpTimer = GetNode<Timer>("Timer");
		//AbilityNameText.Visible = false;
	}

	public event ShowAdditionAbilityUI OnAdditionAbilityUI;

    public void OnPress()
	{
		ApplyActionEvent?.Invoke(ActionId);
	}

	public void OnInitArea()
	{
		ShowHelpTimer.Start();
	}

	public void OnOutArea()
	{
		return;
		ShowHelpTimer.Stop();
		AbilityNameText.Visible = false;
	}

	public void Despose()
	{
		AbilityInfo = null;
	}

	public void OnEndTimer()
	{
		RequestAbilityInfo(this);
		return;
		if (AbilityInfo == null)
		{
			GD.Print(" Fail To Load Data into AbilityButton");
            return;
        }
        AbilityNameText.Text = AbilityInfo.Name;
		GD.Print("Show Ability Description");
		AbilityNameText.Visible = true;
	}

	public void OnPressButton()
	{
		if (ActionId == -1)
			return;
	}
}
