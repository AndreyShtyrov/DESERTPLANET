using DesertPlanet.source;
using DesertPlanet.source.Field;
using Godot;
using Godot.Collections;
using System;
using DesertPlanet.source.Interfaces;

public partial class ResourceUi : HBoxContainer
{
	// Don't using
	private VBoxContainer resourceList { get; set; }
	
	private Dictionary<string, HBoxContainer> resourceDict { get; set; }

	public override void _Ready()
	{
		resourceDict = new Dictionary<string, HBoxContainer>();
        resourceList = GetNode<VBoxContainer>("ResourceList");
	}

	public void InitResourceList()
	{
        var hB = new HBoxContainer();
        var hBText = new LineEdit();
        hB.MouseFilter = MouseFilterEnum.Ignore;
        hBText.Editable = false;
        hBText.MouseFilter = MouseFilterEnum.Ignore;
        hBText.ExpandToTextLength = false;
        hBText.ContextMenuEnabled = false;
        hBText.VirtualKeyboardEnabled = false;
        hBText.ShortcutKeysEnabled = false;
        hBText.SelectingEnabled = false;
        hBText.MiddleMousePasteEnabled = false;
        var font = hBText.GetThemeFont("");
        font.Set("font_size", 6);
        hBText.CustomMinimumSize = new Vector2(20, 5);
        hBText.Text = "I " + ResourceAmount.Iron.ToString();
        var image = Image.LoadFromFile("res://source/Assets/IronUiSimple.png");
        var ttRect = new TextureRect();
        ttRect.Texture = ImageTexture.CreateFromImage(image);
        hB.AddChild(hBText);
        resourceDict.Add("Iron", hB);

        hB = new HBoxContainer();
        hBText = new LineEdit();
        hBText.Editable = false;
        hBText.ExpandToTextLength = false;
        hBText.ContextMenuEnabled = false;
        hBText.VirtualKeyboardEnabled = false;
        hBText.MouseFilter = MouseFilterEnum.Ignore;
        hBText.ShortcutKeysEnabled = false;
        hBText.SelectingEnabled = false;
        hBText.MiddleMousePasteEnabled = false;
        hBText.CustomMinimumSize = new Vector2(20, 5);
        hBText.Text = "O " + ResourceAmount.Oil.ToString();
        image = Image.LoadFromFile("res://source/Assets/OilUiSimple.png");
        ttRect = new TextureRect();
        ttRect.Texture = ImageTexture.CreateFromImage(image);
        hB.AddChild(hBText);
        resourceDict.Add("Oil", hB);

        hB = new HBoxContainer();
        hBText = new LineEdit();
        hBText.Editable = false;
        hBText.ExpandToTextLength = false;
        hBText.ContextMenuEnabled = false;
        hBText.VirtualKeyboardEnabled = false;
        hBText.MouseFilter = MouseFilterEnum.Ignore;
        hBText.ShortcutKeysEnabled = false;
        hBText.SelectingEnabled = false;
        hBText.MiddleMousePasteEnabled = false;
        hBText.CustomMinimumSize = new Vector2(20, 5);
        hBText.Text = "U " + ResourceAmount.Uran.ToString();
        image = Image.LoadFromFile("res://source/Assets/UranUiSimple.png");
        ttRect = new TextureRect();
        ttRect.Texture = ImageTexture.CreateFromImage(image);
        hB.AddChild(hBText);
        resourceDict.Add("Uran", hB);

        if (ResourceAmount.Iron > 0)
		{
            resourceList.AddChild(resourceDict["Iron"]);
		}
        if (ResourceAmount.Oil > 0)
        {
            resourceList.AddChild(resourceDict["Oil"]);
        }
		if(ResourceAmount.Uran  > 0)
		{
            resourceList.AddChild(resourceDict["Uran"]);
        }
    }
	public void SetAmountAndUpdate(SimpleField newAmount) {
		if (ResourceAmount == newAmount)
			return;
		if(newAmount.Iron != ResourceAmount.Iron && ResourceAmount.Iron > 0)
		{
			var textInput = resourceDict["Iron"].GetChild(0) as LineEdit;
			textInput.Text = newAmount.Iron.ToString();
		}
		if (newAmount.Uran != ResourceAmount.Uran && ResourceAmount.Uran > 0)
		{
            var textInput = resourceDict["Uran"].GetChild(0) as LineEdit;
            textInput.Text = newAmount.Uran.ToString();
        }
		if (newAmount.Oil != ResourceAmount.Oil && ResourceAmount.Oil > 0)
		{
            var textInput = resourceDict["Oil"].GetChild(0) as LineEdit;
            textInput.Text = newAmount.Oil.ToString();
        }
		if (newAmount.Iron == 0 && ResourceAmount.Iron > 0)
		{
			var item = resourceDict["Iron"];
			resourceList.RemoveChild(item);
		}
		if(newAmount.Uran == 0 && ResourceAmount.Uran > 0)
		{
			var item = resourceDict["Uran"];
			resourceList.RemoveChild(item);
		}
        if(newAmount.Oil == 0  && ResourceAmount.Oil > 0)
        {
            var item = resourceDict["Oil"];
            resourceList.RemoveChild(item);
        }
        if((ResourceAmount.Iron == 0 && newAmount.Iron > 0 ) || (ResourceAmount.Oil == 0 && newAmount.Oil > 0) ||
            (ResourceAmount.Uran == 0 && newAmount.Uran > 0))
        {
            for(int i = resourceList.GetChildCount() - 1; i >= 0; i--) {
                resourceList.RemoveChild(resourceList.GetChild(i));
            }
            if (newAmount.Iron > 0)
            {
                var item = resourceDict["Iron"];
                resourceList.AddChild(item);
                var textItem = item.GetChild(0) as LineEdit;
                textItem.Text = newAmount.Iron.ToString();
            }
                
            if (newAmount.Oil > 0)
            {
                var item = resourceDict["Oil"];
                resourceList.AddChild(item);
                var textItem = item.GetChild(0) as LineEdit;
                textItem.Text = newAmount.Oil.ToString();
            }
            if (newAmount.Uran > 0)
            {
                var item = resourceDict["Uran"];
                resourceList.AddChild(item);
                var textItem = item.GetChild(0) as LineEdit;
                textItem.Text = newAmount.Uran.ToString();
            }

        }
        ResourceAmount = newAmount;
	}
	public int MapPosX { get; set; }

	public int MapPosY { get; set; }

	public SimpleField ResourceAmount { get; set; }
}

