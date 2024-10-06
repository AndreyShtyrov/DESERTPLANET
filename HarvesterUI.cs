using DesertPlanet.source;
using Godot;
using System;
using System.Linq;

public partial class HarvesterUI : Control
{
    public Harvester Harvester { get; set; }
    private GameMode GameMode { get; set; }

    private TextureRect[] texture;
    private VBoxContainer energyStack;

    private TextEdit IronText;
    private TextEdit PlasticText;
    private TextEdit AliminiumText;
    private TextEdit UranText;
    private TextEdit CementText;
    private TextEdit OilText;
    private TextEdit BaskitText;
    private TextEdit GlassText;
    private TextEdit LimeText;

    public override void _Ready()
    {
        texture = new TextureRect[30];
        for (int i = 0; i < 30 ; i++)
        {
            texture[i] = new TextureRect();
            texture[i].Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("uid://dmr7tdysvnclf"));
        }
        energyStack = GetNode<VBoxContainer>("MainGrid/EnergyStack");
    }
    public void InitResBar()
    {
        IronText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I6/TextEdit");
        PlasticText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I2/TextEdit");
        AliminiumText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I3/TextEdit");
        UranText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I4/TextEdit");
        CementText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I5/TextEdit");
        OilText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I7/TextEdit");
        BaskitText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I8/TextEdit");
        GlassText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I9/TextEdit");
        LimeText = GetNode<TextEdit>("MainGrid/VBoxContainer/RBar/I10/TextEdit");
    }

    public void UpdateResourceContainers()
    {
        if (Harvester == null)
        {
            Visible = false;
            return;
        }
        var Resources = GameMode.Resources[Harvester.X, Harvester.Y];

        IronText.Text = Resources.Iron.ToString();
        PlasticText.Text = Resources.Plastic.ToString();
        AliminiumText.Text = Resources.Alinium.ToString();
        GlassText.Text = Resources.Glass.ToString();
        UranText.Text = Resources.Uran.ToString();
        OilText.Text = Resources.Oil.ToString();
        BaskitText.Text = Resources.Baskit.ToString();
        CementText.Text = Resources.Cement.ToString();
        LimeText.Text = Resources.Lime.ToString();
        for (int i = energyStack.GetChildCount() - 1; i >= 0; i--)
            energyStack.RemoveChild(energyStack.GetChild(i));
        for (int i = 0; i < Resources.Energy; i++)
            if (i < 30)
                energyStack.AddChild(texture[i]);
            else
                energyStack.AddChild(new TextureRect() 
                { Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("uid://dmr7tdysvnclf")) });
    }

    public void SetData(Harvester harvester, GameMode gameMode)
    {
        if (harvester == null)
        {
            return;
        }
        Visible = true;
        GameMode = gameMode;
        Harvester = harvester;
        GetNode<TextEdit>("MainGrid/VBoxContainer/HarvesterName").Text = harvester.Name;
        UpdateResourceContainers();
    }
}
