using DesertPlanet.source;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ProjectMarket : Window
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public PackedScene ProjectCardTemplate { get; set; }
	public List<ProjectFile> ProjectCards { get; set; }

	public GameMode GameMode { get; set; }
	private GridContainer ProjectsGrid { get; set; }
	public override void _Ready()
	{
		ProjectsGrid = GetNode<GridContainer>("Projects");
		ProjectCards = new List<ProjectFile>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Update()
	{
        ProjectCards.Clear();
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
		foreach(var project in ProjectCards)
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
		var player = GameMode.Player;
		var actions = GameMode.Logic.BuyProject(player, ProjectCards[i].Data);
		if (actions.Count == 0)
			return;
		GameMode.ActionManager.ApplyActions(actions);
		Update();
	}
}
