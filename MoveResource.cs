using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;
using DesertPlanet.source.Interfaces;

public partial class MoveResource : Window
{
	// Called when the node enters the scene tree for the first time.
	public SelectorTools Selector { get; set; }
    public GameMode GameMode { get; set; }
    public IHasResource First { get; set; }

	public IHasResource Last { get; set; }

	private ResourceContainer firstTemporaryContainer { get; set; }

	private ResourceContainer lastTemporaryContainer { get; set; }

	private List<TextEdit> firstResOutput = new List<TextEdit>();

	private List<TextEdit> lastResOutput = new List<TextEdit>();
	public override void _Ready()
	{
		for (int i = 1; i <=9; i++)
		{
			firstResOutput.Add(GetNode<TextEdit>("HB/I" + i + "/TextEdit"));
			lastResOutput.Add(GetNode<TextEdit>("HB/I" + i + "/TextEdit2"));
			var button = GetNode<TextureButton>("HB/I" + i + "/HBoxContainer/TextureButton");
            var j = i;
			button.Pressed += () => { DecreaseRes(j); };
			button = GetNode<TextureButton>("HB/I" + i + "/HBoxContainer/TextureButton2");
            button.Pressed += () => { IncreaseRes(j); };
        }
	}

    private void changeRes(int i, ResourceContainer first,  ResourceContainer last)
    {
        switch (i)
        {
            case 1:
                {
                    if (last.Iron > 0)
                    {
                        var res = last.PopupByType(ResourceType.Iron);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 2:
                {
                    if (last.Plastic > 0)
                    {
                        var res = last.PopupByType(ResourceType.Plastic);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 3:
                {
                    if (last.Glass > 0)
                    {
                        var res = last.PopupByType(ResourceType.Glass);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 4:
                {
                    if (last.Alinium > 0)
                    {
                        var res = last.PopupByType(ResourceType.Aliminium);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 5:
                {
                    if (last.Oil > 0)
                    {
                        var res = last.PopupByType(ResourceType.Oil);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 6:
                {
                    if (last.Uran > 0)
                    {
                        var res = last.PopupByType(ResourceType.Uran);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 7:
                {
                    if (last.Cement > 0)
                    {
                        var res = last.PopupByType(ResourceType.Cement);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 8:
                {
                    if (last.Lime > 0)
                    {
                        var res = last.PopupByType(ResourceType.Lime);
                        first.Add(res.Value);
                    }
                    break;
                }
            case 9:
                {
                    if (last.Baskit > 0)
                    {
                        var res = last.PopupByType(ResourceType.Baksits);
                        first.Add(res.Value);
                    }
                    break;
                }
        }
    }
	private void IncreaseRes(int i)
	{
		if (firstTemporaryContainer == null)
			firstTemporaryContainer = First.Resources.Copy();
		if (lastTemporaryContainer == null)
			lastTemporaryContainer = Last.Resources.Copy();
		var last = lastTemporaryContainer;
		var first = firstTemporaryContainer;
        changeRes(i, first, last);
        UpdateVisual();
	}

	private void DecreaseRes(int i)
	{
        if (firstTemporaryContainer == null)
            firstTemporaryContainer = First.Resources.Copy();
        if (lastTemporaryContainer == null)
            lastTemporaryContainer = Last.Resources.Copy();
        var last = lastTemporaryContainer;
        var first = firstTemporaryContainer;
        changeRes(i, last, first);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        var last = lastTemporaryContainer;
        var first = firstTemporaryContainer;
        firstResOutput[0].Text = first.Iron.ToString();
        firstResOutput[1].Text = first.Plastic.ToString();
        firstResOutput[2].Text = first.Glass.ToString();
        firstResOutput[3].Text = first.Alinium.ToString();
        firstResOutput[4].Text = first.Oil.ToString();
        firstResOutput[5].Text = first.Uran.ToString();
        firstResOutput[6].Text = first.Cement.ToString();
        firstResOutput[7].Text = first.Lime.ToString();
        firstResOutput[8].Text = first.Baskit.ToString();

        lastResOutput[0].Text = last.Iron.ToString();
        lastResOutput[1].Text = last.Plastic.ToString();
        lastResOutput[2].Text = last.Glass.ToString();
        lastResOutput[3].Text = last.Alinium.ToString();
        lastResOutput[4].Text = last.Oil.ToString();
        lastResOutput[5].Text = last.Uran.ToString();
        lastResOutput[6].Text = last.Cement.ToString();
        lastResOutput[7].Text = last.Lime.ToString();
        lastResOutput[8].Text = last.Baskit.ToString();
    }

	public void SetData(IHasResource first,  IHasResource last)
	{
		First = first;
		Last = last;
        firstTemporaryContainer = First.Resources.Copy();
        var to_del = new List<PlanetResource>();
        foreach (var res in firstTemporaryContainer)
            if (res.OwnerId != GameMode.Player.Id)
                to_del.Add(res);
        foreach (var res in to_del)
            firstTemporaryContainer.Remove(res);
        lastTemporaryContainer = Last.Resources.Copy();
        to_del = new List<PlanetResource>();
        foreach (var res in lastTemporaryContainer)
            if (res.OwnerId != GameMode.Player.Id)
                to_del.Add(res);
        foreach (var res in to_del)
            lastTemporaryContainer.Remove(res);
        Selector.State = SelectorState.AwaitDialog;
        UpdateVisual();
        Visible = true;
    }

    public void OnCancel()
    {
        Visible = false;
        lastTemporaryContainer = null;
        firstTemporaryContainer = null;
        First = null; Last = null;
        Selector.DeselectUnit();
    }

    public void OnConfirm()
    {
        var actions = GameMode.Logic.MoveResource(First, Last, firstTemporaryContainer);
        GameMode.ActionManager.ApplyActions(actions);
        Visible = false;
        lastTemporaryContainer = null;
        firstTemporaryContainer = null;
        First = null; Last = null;
        Selector.DeselectUnit();
    }
}
