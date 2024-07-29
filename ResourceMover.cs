using DesertPlanet.source;
using Godot;
using System;
using DesertPlanet.source.Interfaces;

public partial class ResourceMover : HBoxContainer
{
	public event ResMoverButtonDown ButtonUpDown;

    public event ResMoverButtonDown ButtonDownDown;

	public TextEdit TextValue { get; set; }

	public TextureButton ButtonUp { get; set; }

	public TextureButton ButtonDown { get; set; }

    public ResourceType Resource { get; set; }

    public override void _Ready()
    {
		TextValue = GetNode<TextEdit>("VBoxContainer/Value");
		ButtonUp = GetNode<TextureButton>("VBoxContainer/BUp");
		ButtonDown = GetNode<TextureButton>("VBoxContainer/BDown");
    }
    public void LoadData(ResourceType resource)
	{
		Resource = resource;
        var image = GetNode<TextureRect>("ResImage");
        if (Resource == ResourceType.Iron)
            image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/iron.png"));
		if (Resource == ResourceType.Energy)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Energy.png"));
		if (Resource == ResourceType.Oil)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/oil.png"));
		if (Resource == ResourceType.Aliminium)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Aliminium.png"));
		if (Resource == ResourceType.Baksits)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Baskit.png"));
		if (Resource == ResourceType.Lime)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Lime.png"));
		if (Resource == ResourceType.Plastic)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Plastic.png"));
		if (Resource == ResourceType.Uran)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Uran.png"));
		if (Resource == ResourceType.Glass)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Glass.png"));
		if (Resource == ResourceType.Cement)
			image.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ResourceIcons/Cement.png"));
    }

	public void OnUpDown()
	{
		ButtonUpDown?.Invoke();
	}

	public void OnDownDown()
	{
		ButtonDownDown?.Invoke();
	}
}
