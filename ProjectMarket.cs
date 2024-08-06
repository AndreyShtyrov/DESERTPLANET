using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectMarket : Window
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public PackedScene ProjectCardTemplate { get; set; }
	public Dictionary<int, ProjectFile> ProjectCards { get; set; }

	public GameMode GameMode { get; set; }
	private GridContainer ProjectsGrid { get; set; }
	public override void _Ready()
	{
		ProjectsGrid = GetNode<GridContainer>("Projects");
		ProjectCards = new Dictionary<int, ProjectFile>();
	}

	public void Update()
	{
		var projects = GameMode.GetCompany(GameMode.Player.Id).Projects;
        for (int i = 0; i < ProjectCards.Count; i++)
        {
            if (projects[i].IsSold)
			{
				ProjectCards[projects[i].Id].BuyButton.Visible = false;
			}
        }
    }

	public void SetData()
	{
		foreach(var project in ProjectCards.Values)
		{
			project.BuyProject -= BuyProject;
		}
		ProjectCards.Clear();
		foreach (var project in GameMode.GetCompany(GameMode.Player.Id).Projects)
		{
			var card = ProjectCardTemplate.Instantiate<ProjectFile>();
			card.Data = project;
			ProjectCards[project.Id] = card;
			card.BuyProject += (int _) => { BuyProject(project.Id); };
			ProjectsGrid.AddChild(card);
		}
	}

	private void BuyProject(int i)
	{
		if (GameMode.State == GameState.AwaitSytem)
			return;
		if (GameMode.State == GameState.Deploy || GameMode.State == GameState.AwaitPlayers || GameMode.State == GameState.ChooseStartResource)
			return;
		var player = GameMode.Player;
		var actions = GameMode.Logic.BuyProject(player, ProjectCards[i].Data);
		if (actions.Count == 0)
			return;
		GameMode.ActionManager.ApplyActions(actions);
		Update();
	}

	public void OnExiteClick()
	{
		Visible = false;
	}
}
