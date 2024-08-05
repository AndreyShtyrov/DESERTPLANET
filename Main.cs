using Godot;
using System;

public partial class Main : Node2D
{

	private void SelectRedactor()
	{
		GetTree().ChangeSceneToFile("res://Redactor.tscn");
	}

	private void SelectNetwork()
	{
		GetTree().ChangeSceneToFile("res://NetworkRoom.tscn");
	}

	private void SelectPlay()
	{
		GetTree().ChangeSceneToFile("res://PlanetScene.tscn");
	}
}
