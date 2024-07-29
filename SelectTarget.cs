using DesertPlanet.source;
using DesertPlanet.source.Field;
using Godot;
using System;
using System.Collections.Generic;
using DesertPlanet.source.Interfaces;

public partial class SelectTarget : Window
{
    // Called when the node enters the scene tree for the first time.

    public VBoxContainer HBox;

    public SelectorTools Selector { get; set; }

    private bool AddGroundToSelect { get; set; } = false;

    private bool SelectAndPerformAction = false;

    private ActionOnId PostAction { get; set; } = null;
    public override void _Ready()
    {
        HBox = GetNode<VBoxContainer>("VBoxContainer");
    }

	public void SetData(List<IOwnedToken> tokens)
	{
        PostAction = null;
        SelectAndPerformAction = false;

        for (int i = HBox.GetChildCount() - 1; i  >= 0; i--)
        {
            var button = HBox.GetChild(i);
            HBox.RemoveChild(button);
            button.QueueFree();
        }
        foreach(var token in tokens)
        {
            var button = new Button();
            button.Text = token.Name + " " + token.Id;
            button.Pressed += () => { SelectItem(token.Id); };
            HBox.AddChild(button);
        }
        if (AddGroundToSelect)
        {
            var button = new Button();
            button.Text = "Ground";
            button.Pressed += () => { SelectItem(-1); };
            HBox.AddChild(button);
        }
        Visible = true;
    }

    public void SetData(List<IOwnedToken> tokens, ActionOnId postAciton, bool isSelectGround)
    {
        AddGroundToSelect = false;
        AddGroundToSelect = isSelectGround;
        SetData(tokens);
        SelectAndPerformAction = true;
        PostAction = postAciton;
        Visible = true;
    }

    public void SelectItem(int id)
    {
        if (!SelectAndPerformAction)
        {
            Selector.UnitId = id;
            Selector.State = SelectorState.SelectAbility;
            Visible = false;
            SelectAndPerformAction = false;
        }
        else
        {
            if (Selector.State == SelectorState.SelectUnit)
            {
                Selector.UnitId = id;
                Selector.State = SelectorState.SelectAbility;
            }    
            SelectAndPerformAction = false;
            Visible = false;
            PostAction(id);
        }
    }
}
