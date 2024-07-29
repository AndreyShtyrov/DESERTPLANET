using DesertPlanet.source.Interfaces;
using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;
using DesertPlanet.source.Ability;

public partial class TransportResource : Window
{
    // Called when the node enters the scene tree for the first time.
    public SelectorTools Selector { get; set; }
    public GameMode GameMode { get; set; }

    private Dictionary<int, int> ResourceCount = new Dictionary<int, int>();

    private AbilityPresset Ability;
    public IHasResource First { get; set; }

    public IHasResource Last { get; set; }

    public IHasAbilities TransportNode { get; set; }

    private ResourceContainer firstTemporaryContainer { get; set; }

    private ResourceContainer lastTemporaryContainer { get; set; }

    private List<TextEdit> firstResOutput = new List<TextEdit>();

    private List<TextEdit> lastResOutput = new List<TextEdit>();

    private TextEdit EnergyCount { get; set; }
    public override void _Ready()
    {
        EnergyCount = GetNode<TextEdit>("TextEdit");
        for (int i = 1; i <= 9; i++)
        {
            ResourceCount[i] = 0;
            firstResOutput.Add(GetNode<TextEdit>("HB/I" + i + "/TextEdit"));
            lastResOutput.Add(GetNode<TextEdit>("HB/I" + i + "/TextEdit2"));
            var button = GetNode<TextureButton>("HB/I" + i + "/HBoxContainer/TextureButton");
            var j = i;
            button.Pressed += () => { DecreaseRes(j); };
            button = GetNode<TextureButton>("HB/I" + i + "/HBoxContainer/TextureButton2");
            button.Pressed += () => { IncreaseRes(j); };
        }
    }

    private void changeRes(int i, ResourceContainer first, ResourceContainer last)
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
        ResourceCount[i] += 1;
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
        ResourceCount[i] += -1;
        changeRes(i, last, first);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        var n = 0;
        foreach (var item in ResourceCount.Values)
            n += Math.Abs(item);
        EnergyCount.Text = n.ToString();

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

    public void SetData(IHasResource first, IHasResource last, IHasAbilities transportNode, AbilityPresset ability)
    {
        for (int i = 1; i <= 9; i++)
            ResourceCount[i] = 0;
        First = first;
        Last = last;
        Ability = ability;
        TransportNode = transportNode;
        firstTemporaryContainer = First.Resources.Copy();
        lastTemporaryContainer = Last.Resources.Copy();
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
        var n = 0;
        foreach (var item in ResourceCount.Values)
            n += Math.Abs(item);
        for (int i = 0; i < n; i++)
            GameMode.Logic.UseAbility(Ability);
        GameMode.ActionManager.ApplyActions(actions);
        Visible = false;
        lastTemporaryContainer = null;
        firstTemporaryContainer = null;
        First = null; Last = null;
        GameMode.NeedRedraw = true;
        Selector.DeselectUnit();
    }
}
