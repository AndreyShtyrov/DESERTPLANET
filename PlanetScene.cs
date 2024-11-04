 using DesertPlanet.source;
using DesertPlanet.source.Ability;
using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public partial class PlanetScene : Node2D
{   
    [Export]
    public PackedScene AbilityScene { get; set; }

    private static object _lock = new object();

    private static object _loadLock = new object();
    private LineEdit outputLine { get; set; }
    private SimpleField[,] resFields { get; set; }
    private SimpleField[,] prevResFields { get; set; }
    public Dictionary<int, Dictionary<int, AbilityButton>> PreloadAbilities { get; set; }
    private bool needUpdateResources { get; set; }
    private TileMap tileMap { get; set; }
    public MapField Map { get; set; } = null;
    private HBoxContainer AbilitiPanel { get; set; }
    private GridContainer BuldingPanel { get; set; }

    private OptionButton LandingSelected { get; set; }

    private event ActionHandler UpdateHarvestersUI;
    public SelectorTools Selector { get; set; }
    public GameMode GameMode { get; set; }

    public bool IsInit = false;

    public bool IsReady = false;

    private bool DebugFlagShowArea = false;
    private TextEdit EnergyText;
    private TextEdit IronText;
    private TextEdit PlasticText;
    private TextEdit AliminiumText;
    private TextEdit UranText;
    private TextEdit CementText;
    private TextEdit OilText;
    private TextEdit BaskitText;
    private TextEdit GlassText;
    private TextEdit LimeText;
    private TextEdit RepoAmount;

    private VBoxContainer AbilityContainer { get; set; }
    private PathLine Path { get; set; }
    private SelectTarget SelectWindow { get; set; }
    private DecideRecipe SelectReceptWindow { get; set; }
    private DecideNotInvariantRecipe SelectRecept2Window { get; set; }
    private MoveResource MoveResourceWindow { get; set; }
    private StartGameResourceMover StartGameResourceMoverWindow { get; set; }
    private TransportResource TransportResourceWindow { get; set; }

    private ProjectMarket ProjectMarketWindow { get; set; }
    public override void _Ready()
	{
        IsInit = false;
        needUpdateResources = false;
        prevResFields = null;
        outputLine = GetNode<LineEdit>("UpToolBar/LineEdit");
        tileMap = GetNode<TileMap>("PlanetField");
        SelectWindow = GetNode<SelectTarget>("SelectTarget");
        MoveResourceWindow = GetNode<MoveResource>("MoveResource");
        SelectReceptWindow = GetNode<DecideRecipe>("DecideRecipe");
        SelectRecept2Window = GetNode<DecideNotInvariantRecipe>("DecideNotInvariantRecipe");
        StartGameResourceMoverWindow = GetNode<StartGameResourceMover>("StartGameResourceMover");
        TransportResourceWindow = GetNode<TransportResource>("TransportResource");
        AbilityContainer = GetNode<VBoxContainer>("Actions");
        Path = GetNode<PathLine>("PathLine");
        ProjectMarketWindow = GetNode<ProjectMarket>("ProjectMarket");
        LandingSelected = GetNode<OptionButton>("MenuBar/Landing/SelectLanding");
        InitBuildButtons();
        InitMap(ProjectSettings.GlobalizePath("res://Maps/") + "/map1.json");
        UpdateHarvestersUI += GetNode<HarvesterUI>("GridContainer/HarvesterUI").UpdateResourceContainers;
        UpdateHarvestersUI += GetNode<HarvesterUI>("GridContainer/HarvesterUI2").UpdateResourceContainers;
        UpdateHarvestersUI += GetNode<HarvesterUI>("GridContainer/HarvesterUI3").UpdateResourceContainers;
        UpdateHarvestersUI += GetNode<HarvesterUI>("GridContainer/HarvesterUI4").UpdateResourceContainers;
        SetResBarOut();
        IsInit = true;
        IsReady = true;
        if (GameMode.Player == GameMode.ActivePlayer)
        {
            LandingSelected.Visible = true;
        }
        
        GetNode<Timer>("UpdateData").Start();
    }

    private void SetResBarOut()
    {
        EnergyText = GetNode<TextEdit>("RBar/I1/TextEdit");
        IronText = GetNode<TextEdit>("RBar/I6/TextEdit");
        PlasticText = GetNode<TextEdit>("RBar/I2/TextEdit");
        AliminiumText = GetNode<TextEdit>("RBar/I3/TextEdit");
        UranText = GetNode<TextEdit>("RBar/I4/TextEdit");
        CementText = GetNode<TextEdit>("RBar/I5/TextEdit");
        OilText = GetNode<TextEdit>("RBar/I7/TextEdit");
        BaskitText = GetNode<TextEdit>("RBar/I8/TextEdit");
        GlassText = GetNode<TextEdit>("RBar/I9/TextEdit");
        LimeText = GetNode<TextEdit>("RBar/I10/TextEdit");
        RepoAmount = GetNode<TextEdit>("RepoTolbar/I1/TextEdit");
    }
     
    public void DrawArea(List<Vector2I> area)
    {
        tileMap.ClearLayer(5);
        foreach(var field in area)
        {
            tileMap.SetCell(5, field, 7, new Vector2I(2, 0));
        }
        DrawBorders();
    }

    private void DrawBorders()
    {
        tileMap.ClearLayer(6);
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
            {
                tileMap.SetCell(6, new Vector2I(i, j), 5, Map[i, j].BorderTileShift);
            }
    }

    public void DrawDynamicObjects()
    {
        if (!GameMode.NeedRedraw)
            return;
        GameMode.RebuildElectrisity();
        GameMode.RebuildContainers();
        tileMap.ClearLayer(1);
        tileMap.ClearLayer(3);
        foreach (var harvester in GameMode.Harvesters.Values)
        {
            tileMap.SetCell(3, new Vector2I(harvester.X, harvester.Y), 2, harvester.TileShift);
        }
        foreach (var building in GameMode.Buildings.Values)
        {
            tileMap.SetCell(1, new Vector2I(building.X, building.Y), building.SourceLevel, building.TileShift);
            GD.Print("Draw Building " + building.X + " , " + building.Y + " ; " + building.SourceLevel + " ; " + building.TileShift.X + " , " + building.TileShift.Y);

        }
        GameMode.NeedRedraw = false;
        //GameMode.UpdatePath();
    }

    public void DrawResourceUI()
    {
        if (!needUpdateResources) return;
        if (prevResFields == null)
        {
            for (int i = 0; i < resFields.GetLength(0); i++)
                for (int j = 0; j < resFields.GetLength(1); j++)
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
        if (prevResFields == null)
            prevResFields = new SimpleField[resFields.GetLength(0), resFields.GetLength(1)];
        for (int i = 0; i < resFields.GetLength(0); i++)
            for (int j = 0; j < resFields.GetLength(1); j++)
                prevResFields[i, j] = resFields[i, j].Copy();
        needUpdateResources = false;
    }

    public override void _Process(double delta)
	{
        if (!IsReady)
            return;
        if (Map == null)
            return;
        if (IsInit)
        {
            IsInit = false;
            return;
        }
            
        var pos = tileMap.GetLocalMousePosition();
        var tilePos = tileMap.LocalToMap(pos);
        
        if (GameMode != null)
        {
            string toOutput = "MP: " + tilePos.X + " " + tilePos.Y + " " + GameMode.StringState();
            toOutput += "AcPl : " + GameMode.ActivePlayer.Id;
            var hex = GameMode.HexalTools.CubeToHex(tilePos.X - 6, tilePos.Y - 4);
            toOutput += " " + Selector.StringState + " H.P. " + hex.Q + " " + hex.R + " " + hex.S;
            if (Selector.UnitId > -1)
                toOutput += " Un S.: " + Selector.UnitId;
            outputLine.Text =  toOutput;
        }
        if (ProjectMarketWindow.Visible)
            return;
        if (GameMode.State == GameState.ChooseStartResource && !StartGameResourceMoverWindow.Visible)
        {
            tileMap.ClearLayer(5);
            LandingSelected.Visible = false;
            StartGameResourceMoverWindow.Show();
            List<Harvester> harvesters = new List<Harvester>();
            foreach (var harvester in GameMode.Harvesters.Values)
                if (harvester.Owner == GameMode.Player)
                    harvesters.Add(harvester);
            InitHarvesterStartGame();
            StartGameResourceMoverWindow.SetData(harvesters, GameMode.Companies[GameMode.Player.Id].StartResources, GameMode.Player);
        }
        if (DebugFlagShowArea)
            DrawArea(GameMode.Area(tilePos.X, tilePos.Y, 2));
        if (Selector.State != SelectorState.SelectAbility && Path.Visible)
            Path.Visible = false;
        if (GameMode.State != GameState.ChooseStartResource)
        {
            if (StartGameResourceMoverWindow.Visible)
            {
                StartGameResourceMoverWindow.Visible = false;
                tileMap.ClearLayer(4);
            } 
        }
        if (GameMode.NeedApplyTurnStartActions)
        {
            GameMode.ActionManager.ApplyActions(GameMode.GetStartTurnActionForPlayer(GameMode.Player));
        }
        if (GameMode.NeedUpdatePaths)
            GameMode.UpdatePath();
        if (GameMode.Map.InBound(tilePos))
        {
            if (Selector.UnitId == -1)
            {
                if (Selector.Position.X != tilePos.X || Selector.Position.Y != tilePos.Y)
                {
                    GD.Print("Prev pos " + Selector.Position.X + " " + Selector.Position.Y + " Next pos " + tilePos.X + " " + tilePos.Y);
                    Selector.Position = tilePos;
                }
            }
            else
            {
                var unitPos = GameMode.GetObjectById(Selector.UnitId).Position;
                if (Selector.Position.X != unitPos.X || Selector.Position.Y != unitPos.Y)
                {
                    Selector.Position = unitPos;
                }
            }

        }
        UpdateHarvestersUI?.Invoke();
        UpdateResourceBar();
        if (Selector.State == SelectorState.SelectUnit && GameMode.NeedDrawAbilityArea)
        {
            GameMode.NeedDrawAbilityArea = false;
        }
        if (Selector.State == SelectorState.SetupRecipt)
        {
            DecideRecipe(Selector.AbilityId);
        }
        if (GameMode.NeedDrawAbilityArea)
        {
            var ability = (GameMode.GetObjectById(Selector.UnitId) as IHasAbilities).GetAbilityById(Selector.AbilityId);
            DrawArea(ability.Area(GameMode));
            GameMode.NeedDrawAbilityArea = false;
            return;
        }
        
        if (GameMode.NeedRedraw)
            MatchResourceSets();
        DrawResourceUI();
        DrawDynamicObjects();
        ProceedInputData(tilePos.X, tilePos.Y, pos.X, pos.Y);
    }

    private void BindButton(VBoxContainer vbox)
    {
        for (int i = 0; i < vbox.GetChildCount(); i++)
        {
            var child = vbox.GetChild(i);
            if (child is AbilityButton button)
            {
                button.ApplyActionEvent += ApplyBuildingButton;
                button.RequestAbilityInfo = AddInfoToBuildButton;
            }
        }
    }
    public void InitBuildButtons()
    {
        var vbox = GetNode<VBoxContainer>("EnergyBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("SocialBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("MiningBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("RefineBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("TransferBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("PlatformBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("SpecialBPanel");
        BindButton(vbox);
        vbox = GetNode<VBoxContainer>("Actions");
        BindButton(vbox);
    }
    public void InitHarvesterStartGame()
    {
        var harvesters = new List<Harvester>();
        foreach(var harvester in GameMode.Harvesters.Values) {
            if (harvester.Owner == GameMode.Player)
                harvesters.Add(harvester);
        }
        tileMap.SetCell(4, harvesters[0].Position, 4, new Vector2I(0, 0));
        tileMap.SetCell(4, harvesters[1].Position, 4, new Vector2I(0, 1));
        if (harvesters.Count > 2)
            tileMap.SetCell(4, harvesters[2].Position, 4, new Vector2I(0, 2));
    }
    public void UpdateResourceBar()
    {
        if (Selector == null)
            return;
        EnergyText.Text = Selector.Energy.ToString();
        IronText.Text = Selector.Iron.ToString();
        PlasticText.Text = Selector.Plastic.ToString();
        AliminiumText.Text = Selector.Alinium.ToString();
        GlassText.Text = Selector.Glass.ToString();
        UranText.Text = Selector.Uran.ToString();
        OilText.Text = Selector.Oil.ToString();
        BaskitText.Text = Selector.Baskit.ToString();
        CementText.Text = Selector.Cement.ToString();
        LimeText.Text = Selector.Lime.ToString();
        RepoAmount.Text = GameMode.Player.Repos.ToString();
    }
    public void ProceedInputData(int X, int Y, float globalX, float globalY)
    {
        if (GameMode.State == GameState.AwaitSytem || GameMode.State == GameState.AwaitPlayers)
            return;
        if (Input.IsActionJustReleased("mb_right"))
        {
            Selector.State = SelectorState.SelectUnit;
            Selector.DeselectUnit();
            Path.Visible = false;
        }
        if (GameMode.State == GameState.ChooseStartResource) { 
            return;
        }
        if (GameMode.State == GameState.Deploy)
        {
            if (GameMode.LandingRegion == -1)
                return;
            if (Input.IsActionJustReleased("mb_left"))
            {
                if (GameMode.ActivePlayer != GameMode.Player)
                    return;
                if (!(X < 0 || X >= Map.Horizontal || Y < 0 || Y >= Map.Vertical))
                {
                    var landing = GameMode.LandingRegion;
                    if (landing != Map.LandingFields[X, Y].Value && landing + 4 != Map.LandingFields[X, Y].Value)
                        return;
                    if (GameMode.PreviosLandingSubSector != -1)
                    {
                        if (GameMode.PreviosLandingSubSector == 1 && landing == Map.LandingFields[X, Y].Value)
                            return;
                        if (GameMode.PreviosLandingSubSector == 2 && landing + 4 == Map.LandingFields[X, Y].Value)
                            return;
                    } 
                    var actions = GameMode.Logic.CreateHarvester(X, Y, GameMode.Player);
                    LandingSelected.Visible = false;
                    GameMode.ActionManager.ApplyActions(actions);
                    if (GameMode.Player.AmountStartHarvesters == 0)
                    {
                        actions = new List<IAction>();
                        UpdateHarvesterBinding();
                        actions.Add(new ChangeGameState(GameMode.Player.Id, GameMode.State, GameState.ChooseStartResource));
                        GameMode.ActionManager.ApplyActions(actions);
                        return;
                    }
                }
            }
        }
        if ((X < 0 || X >= Map.Horizontal || Y < 0 || Y >= Map.Vertical))
            return;
        if (GameMode.State == GameState.PlayTurn)
        {
            if (Input.IsActionJustReleased("mb_left"))
            {
                if (Selector.State == SelectorState.SelectAbility)
                {
                    var unit = GameMode.GetObjectById(Selector.UnitId);
                    if (unit is IHasAbilities hasAbilities)
                    {
                        if (!hasAbilities.CanMoving)
                            return;
                        GameMode.Logic.UseAbility(hasAbilities.Abilities[0], new Vector2I(X, Y));
                    }
                }
                if (Selector.State == SelectorState.SelectUnit)
                {
                    var units = GameMode.GetTokensByPos(X, Y);
                    Selector.UnitId = units[0].Id;
                    if (units[0].CanMoving)
                        Path.SetData(GameMode.GetObjectById(Selector.UnitId));
                    Selector.State = SelectorState.SelectAbility;
                }
                if (Selector.State == SelectorState.SelectTarget)
                {
                    if (GameMode.GetObjectById(Selector.UnitId) is IHasAbilities unit)
                    {
                        var ability = unit.GetAbilityById(Selector.AbilityId);
                        if (ability is DesertPlanet.source.Ability.TransportResource || ability is ShiftResource)
                        {
                            if (Selector.FirstTarget.X < 0 || Selector.FirstTarget.Y < 0)
                            {
                                Selector.FirstTarget = new Vector2I(X, Y);
                                var area = ability.Area(GameMode);
                                var newArea = new List<Vector2I>();
                                foreach (var field in area)
                                {
                                    if (field.X == X && field.Y == Y)
                                        continue;
                                    newArea.Add(field);
                                }
                                tileMap.ClearLayer(5);
                                DrawArea(newArea);
                                return;
                            }
                            else
                            {
                                if (ability is DesertPlanet.source.Ability.TransportResource)
                                    TransportResourceWindow.SetData(Map[Selector.FirstTarget.X, Selector.FirstTarget.Y],
                                        Map[X, Y], unit, ability);
                                else
                                    MoveResourceWindow.SetData(Map[Selector.FirstTarget.X, Selector.FirstTarget.Y], Map[X, Y]);
                                Selector.DeselectUnit();
                            }
                        }
                        if (ability is ConstructBuilding construct)
                        {
                            var actions = construct.Use(GameMode, new Vector2I(X, Y), Selector.SelectedResources);
                            if (actions.Count == 0)
                                return;
                            GameMode.ActionManager.ApplyActions(actions);
                            Selector.DeselectUnit();
                        }
                        if (ability is MakeRecipe makeRecept)
                        {
                            var actions = makeRecept.Use(GameMode, new Vector2I(X, Y), Selector.SelectedResources);
                            GameMode.ActionManager.ApplyActions(actions);
                            Selector.DeselectUnit();
                        }
                        else
                        {
                            GameMode.Logic.UseAbility(ability, new Vector2I(X, Y));
                            Selector.DeselectUnit();
                        }

                        tileMap.ClearLayer(5);
                        return;
                    }
                }
            }
        }
    }

    public void UpdateAvaliableActions()
    {
        if (Selector.UnitId < 0)
        {
            for (int i = 0; i < AbilityContainer.GetChildCount() - 1; i++)
            {
                var button = AbilityContainer.GetChild<AbilityButton>(i);
                button.Visible = false;
            }
            return;
        }
        var unit = GameMode.GetObjectById(Selector.UnitId);
        if (unit == null) return;
        if (unit is IHasAbilities hasAbilities)
        {
            for (int i = 0; i < AbilityContainer.GetChildCount() - 1; i++)
            {
                var button = AbilityContainer.GetChild<AbilityButton>(i);
                if (button.Visible)
                {
                    if (!hasAbilities.HasAbility(button.ActionId))
                        button.Visible = false;
                }
                else
                {
                    if (hasAbilities.HasAbility(button.ActionId))
                        button.Visible = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < AbilityContainer.GetChildCount() - 1; i++)
            {
                var button = AbilityContainer.GetChild<AbilityButton>(i);
                button.Visible = false;
            }
        }
    }
    public void UpdateHarvesterBinding()
    {
        List<Harvester> units = new List<Harvester>();
        var ids = GameMode.Harvesters.Keys.ToList();
        ids.Sort();
        foreach (var id in ids)
        {
            if (GameMode.Harvesters[id].Owner == GameMode.Player)
            {
                units.Add(GameMode.Harvesters[id]);
            }
        }
        var grid = GetNode<GridContainer>("GridContainer");
        for (int i = 0 ; i < units.Count; i++)
        {
            if (grid.GetChild(i) is HarvesterUI harvesterUI)
                harvesterUI.SetData(units[i], GameMode);
        }    
    }

    public void InitMap(string path)
    {
        GD.Print("Init Map");
        var data = ProgramData.Data;
        var players = new List<Player>();
        foreach(var idx in data.PlayerOrder)
        {
            players.Add(data.Players[idx]);
        }
        GameMode = new GameMode(players, data.Companies, data.CurrentPlayer, path);
        if (data.Status == ClientStatus.Multiplayer)
            GameMode.SendLastActions += BroadCastMapChange;
        Map = GameMode.Map;
        LoadMap();
        InitResFieldsFromMap(Map);
        MatchResourceSets();
        Selector = new SelectorTools(GameMode);
        GameMode.ActivePlayer = players[0];
        var actions = GameMode.Logic.StartActions();
        GameMode.ActionManager.ApplyActions(actions);
        PreloadAbilities = new Dictionary<int, Dictionary<int, AbilityButton>>();

        MoveResourceWindow.GameMode = GameMode;
        MoveResourceWindow.Selector = Selector;
        SelectWindow.Selector = Selector;
        SelectReceptWindow.Selector = Selector;
        SelectReceptWindow.GameMode = GameMode;
        SelectRecept2Window.Selector = Selector;
        SelectRecept2Window.GameMode = GameMode;
        StartGameResourceMoverWindow.GameMode = GameMode;
        Path.GameMode = GameMode;
        Path.TileMap = tileMap;
        TransportResourceWindow.GameMode = GameMode;
        TransportResourceWindow.Selector = Selector;
        ProjectMarketWindow.GameMode = GameMode;
        ProjectMarketWindow.SetData();
        for (int i = 0; i < Map.MaxPlayers; i++)
        {
            LandingSelected.AddItem((i + 1).ToString());
        }
        LandingSelected.Select(GameMode.LandingRegion);
        Selector.UnitSelectedEvent += UpdateAvaliableActions;
    }

    private void ShowAdditionUI(string Name)
    {
        if (Name == "Move")
            Path.SetData(GameMode.GetObjectById(Selector.UnitId));
    }
    private void CleanMap()
    {
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
            {
                tileMap.SetCell(0, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(1, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(2, new Vector2I(i, j), -1, Vector2I.Zero);
                tileMap.SetCell(4, new Vector2I(i, j), -1, Vector2I.Zero);
            }
    }

    private void LoadMap()
    {
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
                tileMap.SetCell(0, new Vector2I(i, j), 0, Map[i, j].TileShift);
        for (int i = 0; i < Map.Horizontal; i++)
            for (int j = 0; j < Map.Vertical; j++)
                tileMap.SetCell(5, new Vector2I(i, j), 7, Map.LandingFields[i, j].GetTile);
        DrawBorders();
    }

    public void BroadCastMapChange()
    {
        
        var lastActionIdx = GameMode.ActionManager.ActionIdx;
        var json = GameMode.ActionManager.GetStringForLastActions(GameMode.LastSendedAction);
        if (GameMode.LastSendedAction >= lastActionIdx)
            return;
        GameMode.LastSendedAction = lastActionIdx;
        Rpc("ApplyChangesThroughRpc", json);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ApplyChangesThroughRpc(string json)
    {
        GameMode.ActionManager.ApplyActionsFromJson(json);
        if (GameMode.State == GameState.Deploy && !LandingSelected.Visible)
        {
            ExcludeOccupiedRegionsIds();
            LandingSelected.Visible = true;
            tileMap.ClearLayer(5);
            for (int i = 0; i < Map.Horizontal; i++)
                for (int j = 0; j < Map.Vertical; j++)
                {
                    if (GameMode.OccupiedLandings.Contains(Map.LandingFields[i, j].Value))
                        continue;
                    tileMap.SetCell(5, new Vector2I(i, j), 7, Map.LandingFields[i, j].GetTile);
                }

        }
    }

    public void ApplyBuildingButton(int id)
    {
        GameMode.Logic.PressBuildingButton(Selector, id);
    }
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
        needUpdateResources = true;
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
                if (resField != resFields[i, j])
                {
                    needUpdateResources = true;
                    resFields[i, j] = resField;
                }
            }
        needUpdateResources = true;
    }

    public void OnEndTurn()
    {
        GameMode.EndTurn();
    }

    public void OnMoveResource()
    {
        if (MoveResourceWindow.Selector is null)
            return;
        if (Selector.State == SelectorState.SelectAbility)
        {
            var first = GameMode.GetObjectById(Selector.UnitId);
            if (!(first is IHasResource))
                return;
            Selector.State = SelectorState.AwaitDialog;
            var units = new List<IOwnedToken>() { first };
            foreach (var unit in GameMode.GetTokensByPos(first.X, first.Y))
                if (unit is IHasResource && unit.Id != first.Id)
                    units.Add(unit);
            SelectWindow.SetData(units, PreformOnMoveResource, true);
        }
    }

    public void OnShowProjectMarket()
    {
        ProjectMarketWindow.Show();
    }

    public void OnShowArea() => DebugFlagShowArea = !DebugFlagShowArea;

    public void OnSaveWindows() => GetNode<FileDialog>("MenuBar/SaveWindow").Visible = true;

    public void OnSaveGame(string path)
    {
        GameMode.Save(path);
    }

    public void OnLoadGame(string path)
    {
        IsInit = true;
        lock (_loadLock)
        {
            IsReady = false;
            GameMode newGame = null;
            try
            {
                newGame = GameMode.Load(path);
            }
            catch
            {
                GD.Print("Cannot load game");
                return;
            }
            CleanMap();
            GameMode = newGame;
            Map = GameMode.Map;
            foreach(var unit in PreloadAbilities.Values)
                foreach (var ability in unit.Values)
                    ability.Despose();
            
            PreloadAbilities.Clear();
            LoadMap();
            InitResFieldsFromMap(Map);
            Selector = new SelectorTools(GameMode);
            PreloadAbilities = new Dictionary<int, Dictionary<int, AbilityButton>>();

            MoveResourceWindow.GameMode = GameMode;
            MoveResourceWindow.Selector = Selector;
            SelectWindow.Selector = Selector;
            SelectReceptWindow.Selector = Selector;
            SelectReceptWindow.GameMode = GameMode;
            SelectRecept2Window.Selector = Selector;
            SelectRecept2Window.GameMode = GameMode;
            StartGameResourceMoverWindow.GameMode = GameMode;
            Path.GameMode = GameMode;
            Path.TileMap = tileMap;
            TransportResourceWindow.GameMode = GameMode;
            TransportResourceWindow.Selector = Selector;
            ProjectMarketWindow.GameMode = GameMode;
            prevResFields = null;
            ProjectMarketWindow.SetData();
            SetResBarOut();
            GameMode.ActivePlayer = GameMode.Player;
            GameMode.NeedRedraw = true;
            needUpdateResources = true;
            DrawResourceUI();
            DrawDynamicObjects();
            IsReady = true;
        }
    }

    public void OnLoadWindows() => GetNode<FileDialog>("MenuBar/LoadWindow").Visible = true;

    private void PreformOnMoveResource(int id)
    {
        var unit = GameMode.GetObjectById(Selector.UnitId);
        var unit1 = unit as IHasResource;
        IHasResource unit2 = null;
        if (id == -1)
            unit2 = GameMode.Map[unit.X, unit.Y];
        else
            unit2 = GameMode.GetObjectById(id) as IHasResource;
        if (unit1 == null || unit2 == null)
            return;
        MoveResourceWindow.SetData(unit1, unit2);
    }

    private void DecideRecipe(int id)
    {
        var unit = GameMode.GetObjectById(Selector.UnitId) as IHasAbilities;
        if (unit == null)
        {
            Selector.DeselectUnit();
            return;
        }
        var ability = unit.GetAbilityById(id) as ConstructBuilding;
        if (ability == null)
        {
            Selector.DeselectUnit();
            return;
        }
        SelectReceptWindow.SetData(ability.Recipe, ability);
    }

    public void AddInfoToBuildButton(object sender)
    {
        if (sender is AbilityButton button)
        {
            var company = GameMode.GetCompany(GameMode.Player);
            button.AbilityInfo = company.Recepts[button.ActionId].Info;
        }
    }
    private void DecideNotInvariantRecipe(int id)
    {
        var unit = GameMode.GetObjectById(Selector.UnitId) as IHasAbilities;
        if (unit == null)
        {
            Selector.DeselectUnit();
            return;
        }
        var ability = unit.GetAbilityById(id) as MakeRecipe;
        if (ability == null)
        {
            Selector.DeselectUnit();
            return;
        }
        SelectRecept2Window.SetData(ability.Recipe, ability);
    }

    private void ExcludeOccupiedRegionsIds()
    {
        for (int i = LandingSelected.ItemCount - 1; i >= 0; i--)
            LandingSelected.RemoveItem(i);
        for (int i = 0; i < Map.MaxPlayers; i++)
            if (!GameMode.OccupiedLandings.Contains(i + 1))
                LandingSelected.AddItem((i + 1).ToString());
        LandingSelected.Selected = -1;
    }

    public void ChooseLandingRegion(int i)
    {
        GameMode.LandingRegion = LandingSelected.GetItemText(i).ToInt();
    }
}
