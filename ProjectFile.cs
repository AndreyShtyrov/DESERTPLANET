using DesertPlanet.source;
using DesertPlanet.source.Companies.Projects;
using Godot;
using System;

public partial class ProjectFile : Control
{
	// Called when the node enters the scene tree for the first time.

	public event ActionOnId BuyProject;

	public Button BuyButton { get; set; }
	public CompanyProject Data { get; set; }

	public override void _Ready()
	{
		GetNode<TextureRect>("VBoxContainer/HBoxContainer/ProjectImage").Texture = Data.Texture;
		GetNode<TextEdit>("VBoxContainer/HBoxContainer/Container/I1/TextEdit").Text = Data.Repo.ToString();
		GetNode<TextEdit>("VBoxContainer/Description").Text = Data.Description;
		BuyButton = GetNode<Button>("VBoxContainer/Button");

	}

}
