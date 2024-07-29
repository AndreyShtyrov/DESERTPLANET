 using DesertPlanet.source.Field;
using DesertPlanet.source;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesertPlanet.source.Interfaces;

public partial class Redactor : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public int CurrentFieldType { get; set; } = 0;
    [Export]
    public PackedScene ResourceUIPacked { get; set; }
    public MapField Map { get; set; }

    private int PrevX { get; set; }

    private int PrevY { get; set; }

    private LineEdit SelectedPosition { get; set; }

    private OptionButton BordersType { get; set; }

    private bool needUpdateResources { get; set; }
    private LineEdit lineX { get; set; }

    private LineEdit lineY { get; set; }

    private LineEdit outputLine { get; set; }

    private FileDialog fileDialog { get; set; } = null;
    private SimpleField[,] resFields { get; set; }
    private SimpleField[,] prevResFields { get; set; }
    private TileMap tileMap { get; set; }
    private void InitResFieldsFromMap(MapField map)
    {
        resFields = new SimpleField[map.Horizontal, map.Vertical];
        for (int i = 0; i < resFields.GetLength(0); i++)
            for (int j = 0; j < resFields.GetLength(1); j++)
            {
                var resField = new SimpleField();
                foreach (var res in map[i, j].Resources)
                {
                    if (res.Type == ResourceType.Iron)
                        resField.Iron++;
                    if (res.Type == ResourceType.Oil) resField.Oil++;
                    if (res.Type == ResourceType.Uran) resField.Uran++;
                }
                resFields[i, j] = resField;
            }
        PrevX = 0;
        PrevY = 0;
        var bordersType = 0;
        SelectedPosition.Text = "Pos: " + PrevX + ", " + PrevY;
        BordersType.Selected = bordersType;
        needUpdateResources = true;
    }
    public override void _Ready()
    {
        needUpdateResources = false;
        prevResFields = null;
        Map = new MapField("NewMap", 10, 10);
        GetNode<LineEdit>("HB/Width").Text = Map.Horizontal.ToString();
        GetNode<LineEdit>("HB/Height").Text = Map.Vertical.ToString();
        lineX = GetNode<LineEdit>("HB/LX");
        lineY = GetNode<LineEdit>("HB/LY");
        outputLine = GetNode<LineEdit>("HB/OutPut");
        tileMap = GetNode<TileMap>("PlanetField");
        SelectedPosition = GetNode<LineEdit>("SetBorders/Postion");
        BordersType = GetNode<OptionButton>("SetBorders/OptionButton");
        InitResFieldsFromMap(Map);
        GD.Print("Init Redactor");
        LoadMap();
    }

    private void CleanMap()
    {
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
            {
                tileMap.SetCell(0, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(1, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(2, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(3, new Vector2I(i, j), -1, Vector2I.Zero);
            }
    }
    private void LoadMap()
    {
        for(int i = 0; i < Map.Horizontal; i++)
            for(int j =0; j < Map.Vertical; j++)
            {
                tileMap.SetCell(0, new Vector2I(i, j), 0, GetTileShift(Map[i, j]));
            }
        DrawBorders();
    }
    public override void _Process(double delta)
    {
        var pos = tileMap.GetLocalMousePosition();
        var tilePos = tileMap.LocalToMap(pos);

        lineX.Text = tilePos.X.ToString();
        lineY.Text = tilePos.Y.ToString();
        outputLine.Text = "M. P. " + pos.X.ToString() + " " + pos.Y.ToString();
        if (fileDialog == null)
        {
            if (Input.IsActionJustReleased("mb_left") && Map != null)
                UpdateSetBordersPanel(tilePos.X, tilePos.Y);
            UpdateResourceUI();
            if (Input.IsActionJustReleased("mb_left"))
            {
                if (!(tilePos.X < 0 || tilePos.X >= Map.Horizontal || tilePos.Y < 0 || tilePos.Y >= Map.Vertical))
                {
                    if (CurrentFieldType != 0)
                    {
                        if (CurrentFieldType < 12)
                        {
                            Map[tilePos.X, tilePos.Y] = GetFieldToken(tilePos.X, tilePos.Y);
                            tileMap.SetCell(0, tilePos, 0, GetTileShift(Map[tilePos.X, tilePos.Y]));
                        }
                        else
                        {
                            switch (CurrentFieldType)
                            {
                                case 12:
                                    Map[tilePos.X, tilePos.Y].Resources.Add(new PlanetResource(ResourceType.Iron)); break;
                                case 13:
                                    Map[tilePos.X, tilePos.Y].Resources.Add(new PlanetResource(ResourceType.Oil)); break;
                                case 14:
                                    Map[tilePos.X, tilePos.Y].Resources.Add(new PlanetResource(ResourceType.Uran)); break;
                            }
                            MatchResourceSets();
                        }
                    }
                }
            }
        }
        else
        {
            if (!fileDialog.Visible)
                fileDialog = null;
        }
    }

    private void ExpandBorders(int blockPosition, List<FieldToken> area)
    {
        var otherSidePosition = blockPosition + 3;
        if (otherSidePosition > 6)
            otherSidePosition = otherSidePosition - 6;
        area[blockPosition - 1].ExpandBlocks(otherSidePosition);
    }

    private void DeleteBorders(int blockPosition, List<FieldToken> area)
    {
        var otherSidePosition = blockPosition + 3;
        if (otherSidePosition > 6)
            otherSidePosition = otherSidePosition - 6;
        area[blockPosition - 1].DeleteBlocks(otherSidePosition);
    }

    private void DrawBorders()
    {
        tileMap.ClearLayer(3);
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
            {
                tileMap.SetCell(3, new Vector2I(i, j), 2, Map[i, j].BorderTileShift);
            }
    }

    public void OnSetBorders()
    {
        var neighbors = Map[PrevX, PrevY].Neighbors;
        var area = new List<FieldToken>();
        var newBorderType = BordersType.Selected;
        var prevBorderType = Map[PrevX, PrevY].BlockSeds;
        foreach (var pos in neighbors)
        {
            if (!Map.InBound(pos))
                continue;
            area.Add(Map[pos.X, pos.Y]);
        }
        area.Add(Map[PrevX, PrevY]);
        var prevBordersTypes = new List<int>();
        for (int i = 0; i < area.Count; i++)
            prevBordersTypes.Add(area[i].BlockSeds);
        try
        {
            var newBlocks = new List<int>();
            var oldBlocks = new List<int>();
            var prevBlockPositions = FieldToken.BorderPostions(prevBorderType);
            var newBlockPositions = FieldToken.BorderPostions(newBorderType);
            foreach (var pos in newBlockPositions)
                if (!prevBlockPositions.Contains(pos))
                    newBlocks.Add(pos);
            foreach (var pos in prevBlockPositions)
                if (!newBlockPositions.Contains(pos))
                    oldBlocks.Add(pos);
            foreach (var pos in newBlocks)
                ExpandBorders(pos, area);
            foreach(var pos in oldBlocks)
                DeleteBorders(pos, area);
                Map[PrevX, PrevY].BlockSeds = newBorderType;
        }
        catch {
            for (int i = 0; i < area.Count; i++)
                area[i].BlockSeds = prevBordersTypes[i];
        }
        DrawBorders();
    }

    public void OnCleanBorders()
    {
        var neighbors = Map[PrevX, PrevY].Neighbors;
        var center = new Vector2I(PrevX, PrevY);
        var area = new List<FieldToken>();
        foreach (var pos in neighbors)
        {
            if (!Map.InBound(pos))
                continue;
            area.Add(Map[pos.X, pos.Y]);
        }
        foreach (var field in area)
        {
            field.BlockSeds = 0;
        }
        DrawBorders();
    }
    public void UpdateSetBordersPanel(int X, int Y)
    {
        if (!Map.InBound(new Vector2I(X, Y)))
            return;
        PrevX = X;
        PrevY = Y;
        var bordersType = Map[PrevX, PrevY].BlockSeds;
        SelectedPosition.Text = "Pos: " + PrevX + ", " + PrevY;
        BordersType.Selected = bordersType;
    }

    public void MatchResourceSets()
    {
        for (int i = 0; i < resFields.GetLength(0); i++)
            for (int j = 0; j < resFields.GetLength(1); j++)
            {
                var resField = new SimpleField();
                foreach (var res in Map[i, j].Resources)
                {
                    if (res.Type == ResourceType.Iron)
                        resField.Iron++;
                    if (res.Type == ResourceType.Oil) resField.Oil++;
                    if (res.Type == ResourceType.Uran) resField.Uran++;
                }
                if ( resField != resFields[i, j])
                {
                    needUpdateResources = true;
                    resFields[i, j] = resField;
                }
            }
        needUpdateResources = true;
    }

    public void UpdateResourceUI()
    {
        
        if (!needUpdateResources)
            return;
        if (!needUpdateResources) return;
        if (prevResFields == null)
        {
            for(int i = 0; i < resFields.GetLength(0); i++)
                for(int j = 0; j < resFields.GetLength(1); j++)
                {
                    var resource = resFields[i, j];
                    if (resource.isZero) continue;
                    tileMap.SetCell(2, new Vector2I(i, j), 1, resource.GetTileShift());
                }
        }
        else
        {
            for (int i = 0; i < resFields.GetLength(0); i++)
                for (int j = 0; j < resFields.GetLength(1); j++)
                {
                    var resource = resFields[i, j];
                    if (resource == prevResFields[i, j]) continue;
                    tileMap.SetCell(2, new Vector2I(i, j), 1, resource.GetTileShift());
                }
        }
        if(prevResFields == null)
            prevResFields = new SimpleField[resFields.GetLength(0), resFields.GetLength(1)];
        for (int i = 0; i < resFields.GetLength(0); i++)
            for(int j = 0; j < resFields.GetLength(1); j++)
                prevResFields[i, j] = resFields[i, j].Copy();
        needUpdateResources = false;
    }

    private Vector2I GetTileShift(FieldToken token)
    {
        if (token == null)
            return new Vector2I(-1, -1);
        if (token.Name == "Sand")
            return FieldToken.SandTileShift;
        if (token.Name == "Stone")
            return FieldToken.StoneTileShift;
        if (token.Name == "Water")
            return FieldToken.WaterTileShift;
        if (token.Name == "StaticCosmoport")
            return FieldToken.SpaceDockTileShift;
        if (token.Name == "Empty")
            return FieldToken.EmptyTileShift;
        if (token.Name == "StoneIron")
            return FieldToken.StoneIronTileShift;
        if (token.Name == "StoneOil")
            return FieldToken.StoneOilTileShift;
        if (token.Name == "SandLime")
            return FieldToken.SandLimeTileShift;
        if (token.Name == "SandBoskit")
            return FieldToken.SandBokcitTileShift;
        if (token.Name == "Blocked")
            return FieldToken.BlockedTileShift;
        if (token.Name == "WaterOil")
            return FieldToken.WaterOilTileShift;
        GD.Print("Error Don't Found such token " + token.Name);
        return new Vector2I(-1, -1);
    }

    public FieldToken GetFieldToken(int X, int Y)
    {
        switch (CurrentFieldType)
        {
            case 1: return new Sand(X, Y);
            case 2: return new Stone(X, Y);
            case 3: return new Water(X, Y);
            case 4: return new StaticCosmoport(X, Y);
            case 5: return new Empty(X, Y);
            case 6: return new SandBoskit(X, Y);
            case 7: return new SandLime(X, Y);
            case 8: return new StoneIron(X, Y);
            case 9: return new StoneOil(X, Y);
            case 10: return new WaterOil(X, Y);
            case 11: return new Blocked(X, Y);
        }
        return new Empty(X, Y);
    }

    public void SelectSand() => CurrentFieldType = 1;

    public void SelectStone() => CurrentFieldType = 2;

    public void SelectWater() => CurrentFieldType = 3;

    public void SelectCosmo() => CurrentFieldType = 4;

    public void SelectEmpty() => CurrentFieldType = 5;

    public void SelectBoskit() => CurrentFieldType = 6;

    public void SelectLim() => CurrentFieldType = 7;

    public void SelectStoneIron() => CurrentFieldType = 8;

    public void SelectStoneOil() => CurrentFieldType = 9;

    public void SelectWaterOil() => CurrentFieldType = 10;

    public void SelectBlocked() => CurrentFieldType = 11;
    public void SelectIron() => CurrentFieldType = 12;

    public void SelectOil() => CurrentFieldType = 13;

    public void SelectUran() => CurrentFieldType = 14;

    public void Unselect() => CurrentFieldType = 0;

    public void OnLoadFile()
    {
        fileDialog = GetNode<FileDialog>("LoadFile");
        fileDialog.Show();
    }

    public void OnSaveFile()
    {
        fileDialog = GetNode<FileDialog>("SaveFile");
        fileDialog.Show();
    }

    public void SaveFile(string path)
    {
        Map.Save(path);
        fileDialog = null;
    }

    public void LoadFile(string path)
    {
        CleanMap();
        needUpdateResources = false;
        prevResFields = null;
        var map = MapField.Load(path);
        Map = map;
        InitResFieldsFromMap(Map);
        GD.Print("Load Map");
        LoadMap();
        fileDialog = null;
    }

    public void CreateMap()
    {
        try
        {
            CleanMap();
            var width = int.Parse(GetNode<LineEdit>("HB/Width").Text);
            var height = int.Parse(GetNode<LineEdit>("HB/Height").Text);
            needUpdateResources = false;
            prevResFields = null;
            var map = new MapField("NewMap", width, height);
            Map = map;
            InitResFieldsFromMap(Map);
            GD.Print("Load Map");
            LoadMap();
            fileDialog = null;
        }
        catch (Exception ex)
        {
            GD.Print(ex);
            return;
        }

    }
}
