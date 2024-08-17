using DesertPlanet.source;
using DesertPlanet.source.Ability;
using Godot;
using System;
using DesertPlanet.source.Interfaces;

public partial class AbilityButton : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public AbilityPresset Ability { get; set; }

    private GameMode GameMode { get; set; }
    private SelectorTools Selector { get; set; }
	private ActionOnId PostAction { get; set; }

	private Label AbilityNameText { get; set; }

	private Timer ShowHelpTimer { get; set; }
	public override void _Ready()
	{
		Text = Ability.Name;
		AbilityNameText = GetNode<Label>("AbilityName");
		ShowHelpTimer = GetNode<Timer>("Timer");
		AbilityNameText.Visible = false;
	}

	public event ShowAdditionAbilityUI OnAdditionAbilityUI;
	// Need in PlanetScene Disconect this event
	public void SetData(AbilityPresset ability, SelectorTools selector, GameMode gamemode)
	{
        PostAction = null;
		Ability = ability;
		Selector = selector;
		GameMode = gamemode;
	}

    public void SetData(AbilityPresset ability, SelectorTools selector, GameMode gamemode, ActionOnId postAction)
    {
        PostAction = null;
        Ability = ability;
        Selector = selector;
        GameMode = gamemode;
		PostAction = postAction;
    }

	public void Despose()
	{
        PostAction = null;
        Ability = null;
        Selector = null;
        GameMode = null;
        PostAction = null;
    }

    public void OnPress()
	{
		if (GameMode.State == GameState.AwaitSytem)
			return;
		if (Selector.AbilityId == Ability.Id)
		{
			Selector.AbilityId = -1;
		}
		else
			Selector.AbilityId = Ability.Id;
		if (PostAction != null)
		{
			PostAction(Ability.Id);
			return;
		}
		Selector.State = SelectorState.SelectTarget;
		if (!Ability.NeedSelecTarget)
		{
			GameMode.Logic.UseAbility(Ability);
			Selector.DeselectUnit();
			return;
		}
		GameMode.NeedDrawAbilityArea = true;
		if (Ability.Name == "Move")
		{
			GameMode.NeedDrawAbilityArea = false;
            OnAdditionAbilityUI?.Invoke("Move");
        }
	}

	public void OnInitArea()
	{
		ShowHelpTimer.Start();
	}

	public void OnOutArea()
	{
		ShowHelpTimer.Stop();
		AbilityNameText.Visible = false;
	}

	public void OnEndTimer()
	{
		AbilityNameText.Text = Ability.Name;
		GD.Print("Show Ability Description");
		AbilityNameText.Visible = true;
	}
}
