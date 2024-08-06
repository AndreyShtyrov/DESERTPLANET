using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	
    private void SelectRedactor()
	{
		GetTree().ChangeSceneToFile("res://Redactor.tscn");
	}

	private void SelectNetwork()
	{
        var player = new Player(1);
        var data = new ProgramData();
        data.CurrentPlayer = player;
        GetTree().ChangeSceneToFile("res://NetworkRoom.tscn");
	}

	private void SelectPlay()
	{
        var player = new Player(1);
        var data = new ProgramData();
        data.Players.Add(0, player);
        data.Companies = new Dictionary<int, string> { { 1, "base" } };
        data.CurrentPlayer = player;
        data.PlayerOrder.Add(0);
        GetTree().ChangeSceneToFile("res://PlanetScene.tscn");
	}
}
