﻿using DesertPlanet.source.Action;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies;
using DesertPlanet.source.Electosity;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using static Godot.Projection;

namespace DesertPlanet.source
{
    public enum GameState
    {
        Deploy = 1,
        AwaitPlayers = 2,
        PlayTurn = 3,
        ChooseStartResource = 4,
        AwaitSytem = 0
    }
    public class GameMode
    {
        public Player ActivePlayer { get; set; } = null;
        public Player Player { get; }
        public List<int> OccupiedLandings { get; } = new List<int>();
        public int PreviosLandingSubSector { get; set; } = -1;
        public int LandingRegion { get; set; } = -1;
        public Player NextPlayer { get
            {
                var index = PlayerList.IndexOf(ActivePlayer);
                if (index == PlayerList.Count - 1)
                    return PlayerList[0];
                else
                    return PlayerList[index + 1];
            } }
        public Dictionary<int, Building> Buildings { get; }
        public List<Player> PlayerList { get; }
        public ElectrosityGraph Electrosity { get; }
        public GameState State { get; set; }
        public Dictionary<int, Harvester> Harvesters { get; }
        public MapField Map { get; }
        private MapField StartMap { get; }
        public ResourceContainer[,] Resources { get; }
        public int UnitId { get; set; }
        public ActionManager ActionManager { get; }
        public Dictionary<int, Company> Companies { get; }
        public GameLogic Logic { get; }
        private Task RebuildElectrisityTask = null;
        private Task RebuildContainersTask = null;
        private PathFounding PathFounding;
        public bool NeedRedraw { get; set; } = false;
        public bool NeedRebuildContainers { get; set; } = false;
        public bool NeedRebuildElectrisity { get; set; } = false;
        public bool NeedDrawAbilityArea { get; set; } = false;
        public bool NeedLoadAbilityGUI { get; set; } = false;
        public bool NeedApplyTurnStartActions { get; set; } = false;
        public bool NeedUpdatePaths { get; set; } = false;
        public CubeHexalTools HexalTools { get; set; }
        public List<int> UpdateAbilitiesGuiTargets { get; }
        public GameMode(List<Player> players, Dictionary<int, string> companies, Player currentPlayer, string path) {
            Player = currentPlayer;
            State = GameState.AwaitSytem;
            Map = MapField.Load(path);
            Harvesters = new Dictionary<int, Harvester>();
            Buildings = new Dictionary<int, Building>();
            PlayerList = players;
            ActionManager = new ActionManager(this);
            UnitId = 0;
            NeedRedraw = false;
            Electrosity = new ElectrosityGraph(this);
            UpdateAbilitiesGuiTargets = new List<int>();
            Resources = new ResourceContainer[Map.Horizontal, Map.Vertical];
            for (int i = 0; i < Map.Horizontal; i++)
                for (int j = 0; j < Map.Vertical; j++)
                {
                    Resources[i, j] = new ResourceContainer();
                }
            Logic = new GameLogic(this);
            Companies = new Dictionary<int, Company>();
            foreach (var player in players)
                Companies.Add(player.Id, Company.CreateFromName(companies[player.Id], player, this));
            HexalTools = new CubeHexalTools();
            StartMap = Map.Copy();
            PathFounding = new PathFounding(this);
        }
        public GameMode(List<Player> players, Dictionary<int, string> companies, Player currentPlayer, MapField map)
        {
            Player = currentPlayer;
            State = GameState.AwaitSytem;
            Map = map;
            Harvesters = new Dictionary<int, Harvester>();
            Buildings = new Dictionary<int, Building>();
            PlayerList = players;
            ActionManager = new ActionManager(this);
            UnitId = 0;
            NeedRedraw = false;
            Electrosity = new ElectrosityGraph(this);
            UpdateAbilitiesGuiTargets = new List<int>();
            Resources = new ResourceContainer[Map.Horizontal, Map.Vertical];
            for (int i = 0; i < Map.Horizontal; i++)
                for (int j = 0; j < Map.Vertical; j++)
                {
                    Resources[i, j] = new ResourceContainer();
                }
            Logic = new GameLogic(this);
            Companies = new Dictionary<int, Company>();
            foreach (var player in players)   
               Companies.Add(player.Id, Company.CreateFromName("base", player, this));
            HexalTools = new CubeHexalTools();
            PathFounding = new PathFounding(this);
            StartMap = Map.Copy();
        }
        public int LastSendedAction = -1;
        public event System.Action SendLastActions;
        public void RebuildElectrisity()
        {
            RebuildElectrisityTask = new Task(() => { Electrosity.Rebuild(); });
            RebuildElectrisityTask.Start();
        }
        public void RebuildContainers()
        {
            if (RebuildContainersTask != null)
                RebuildContainersTask.Wait();
            RebuildContainersTask = new Task(() => { UpdateContainers(); });
            RebuildContainersTask.Start();
        }
        private void UpdateContainers()
        {
            if (RebuildElectrisityTask == null)
                RebuildElectrisity();
            RebuildElectrisityTask.Wait();
            List<IHasResource>[,] units = new List<IHasResource>[Map.Horizontal, Map.Vertical];
            foreach(var unit in Harvesters.Values)
            {
                if (units[unit.X, unit.Y] == null)
                    units[unit.X, unit.Y] = new List<IHasResource> { unit };
                else
                    units[unit.X, unit.Y].Add(unit);
            }
            foreach(var unit in Buildings.Values)
            {
                if (unit is IHasResource hasResource )
                {
                    if (units[unit.X, unit.Y] == null)
                        units[unit.X, unit.Y] = new List<IHasResource> { hasResource };
                    else
                        units[unit.X, unit.Y].Add(hasResource);
                }
            }
            Player.Resources.Clean();
            for (int i = 0; i < Map.Horizontal; i++)
                for (int j = 0;  j < Map.Vertical; j++) 
                {
                    
                    try
                    {
                        Resources[i, j].Clean();
                        foreach (var res in Map[i, j].Resources)
                            if (res.OwnerId == Player.Id)
                            {
                                Player.Resources.Add(res);
                                Resources[i, j].Add(res);
                            }
                    }
                    catch
                    {
                        GD.Print(i, j);
                    }
                    
                    if (units[i, j] == null)
                        continue;
                    foreach (var resource in units[i, j])
                    {
                        foreach (var res in resource.Resources)
                            if (res.OwnerId == Player.Id)
                            {
                                Player.Resources.Add(res);
                                Resources[i, j].Add(res);
                            }
                    }
                }
        }
        public List<IHasResource> GetEnergy(IOwnedToken token)
        {
            if (RebuildElectrisityTask == null)
            {
                return new List<IHasResource>();
            }
            RebuildElectrisityTask.Wait();
            return Electrosity.GetEnergy(token);
        }
        public List<IHasAbilities> GetTokensByPos(int X, int Y)
        {
            var result = new List<IHasAbilities>();
            foreach (var harvester in Harvesters.Values)
            {
                if (harvester.X == X && harvester.Y == Y && Player.Id == harvester.Owner.Id)
                    result.Add(harvester);
            }
            foreach (var building in Buildings.Values)
                if (building.X == X && building.Y == Y && Player.Id == building.Owner.Id)
                    result.Add(building);
            return result;
        }
        public IOwnedToken GetObjectById(int id)
        {
            foreach(var harvester in Harvesters)
            {
                if (Harvesters.ContainsKey(id))
                    return Harvesters[id];
            }
            foreach(var building in Buildings)
                if (Buildings.ContainsKey(id))
                    return Buildings[id];
            return null;
        }
        public Company GetCompany(int id)
        {
            return Companies[id];
        }
        public Company GetCompany(Player player)
        {
            return GetCompany(player.Id);
        }
        public Player GetPlayer(int id)
        {
            foreach (var player in PlayerList)
                if (player.Id == id)
                    return player;
            throw(new Exception("!! Error Player with id "+ id + " hasn't found"));
        }
        public void EndTurn()
        {
            if (ActivePlayer != Player)
                return;
            var actions = new List<IAction>();
            if (PlayerList.Count != 1)
            {
                var index = PlayerList.IndexOf(ActivePlayer);
                if (index == PlayerList.Count - 1)
                {
                    actions.Add(new ChangeGameState(ActivePlayer.Id, GameState.PlayTurn, GameState.AwaitPlayers));
                    actions.Add(new ChangeActivePlayer(ActivePlayer, PlayerList[0]));
                    actions.Add(new ChangeGameState(PlayerList[0].Id, GameState.AwaitPlayers, GameState.PlayTurn));
                }
                    
                else
                {
                    actions.Add(new ChangeGameState(ActivePlayer.Id, GameState.PlayTurn, GameState.AwaitPlayers));
                    actions.Add(new ChangeActivePlayer(ActivePlayer, PlayerList[index + 1]));
                    actions.Add(new ChangeGameState(PlayerList[index + 1].Id, GameState.AwaitPlayers, GameState.PlayTurn));
                }
            }
            else
            {
                actions.AddRange(GetStartTurnActionForPlayer(Player));
            }
            ActionManager.ApplyActions(actions);
        }
        public PathNode[,] GetPaths(int id)
        {
            return PathFounding.GetPaths(id);
        }
        public void CleanPaths() => PathFounding.Clear();
        public void UpdatePath() => PathFounding.UpdatePath();
        public List<Vector2I> Area(int x0, int y0, int radius, bool isFill = true)
        {
            var result = new List<Vector2I>();
            for (var x = x0 - radius - 2; x <= x0 + radius + 2; x++)
            {
                if ( x < 0 || x >= Map.Horizontal)
                    continue;
                for (var y = y0 - radius - 2; y <= y0 + radius + 2; y++)
                {
                    if ( y < 0 || y >= Map.Vertical)
                        continue;
                    if (isFill)
                    {
                        if (HexalTools.CubeToEvenQ(x - x0, y - y0, x0).Length() <= radius)
                            result.Add(new Vector2I(x, y));
                    }
                    else
                    {
                        if (HexalTools.CubeToEvenQ(x - x0, y - y0, x0).Length() == radius)
                            result.Add(new Vector2I(x, y));
                    }

                }    
            }
            return result;
        }
        public List<IAction> StartGame()
        {
            return new List<IAction>() { new ChangeGameState(Player.Id, State, GameState.Deploy)};
        }
        public List<IAction> GetStartTurnActionForPlayer(Player player)
        {
            var result = new List<IAction>();
            foreach (var harvester in Harvesters.Values)
                if (harvester.Owner.Id == player.Id)
                {
                    result.AddRange(harvester.StartTurnActions());
                    result.Add(new RefreshActionCounter(harvester));
                }
            foreach (var building in Buildings.Values)
                if (building.Owner.Id == player.Id)
                {
                    if (building is IOwnedTokenWithAbilites hasAbilities)
                        result.Add(new RefreshActionCounter(hasAbilities));
                    if (building is IHasStartTurnAction startBuilding)
                        result.AddRange(startBuilding.StartTurnActions());
                }

            result.Add(new CloseStartActions(player));
            return result;
        }
        public void Save(string file)
        {
            var saveGame = new SaveGame();
            saveGame.MapFile = StartMap.Name;
            saveGame.Actions = new List<IAction>();
            saveGame.Player = Player.Id;
            saveGame.Players = new List<int>();
            foreach (var player in PlayerList)
                saveGame.Players.Add(player.Id);
            saveGame.Companies = new Dictionary<int, string>();
            foreach (var player in PlayerList)
                saveGame.Companies.Add(player.Id, GetCompany(player).Name);
            foreach (var action in ActionManager.PreviousStates)
            {
                saveGame.Actions.Add(action);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(saveGame,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            using (StreamWriter fw = new StreamWriter(file))
                fw.WriteLine(json);
        }
        public static GameMode Load(string file)
        {
            string jsonLine = "";
            var programData = ProgramData.Data;
            using (StreamReader fs = new StreamReader(file))
                jsonLine = fs.ReadToEnd();
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveGame>(jsonLine, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            var map = MapField.Load(data.MapFile);
            Player currentPlayer = null;
            var players = new List<Player>();
            foreach (var player in data.Players)
                if (player == data.Player)
                {
                    var p = new Player(player);
                    players.Add(p);
                    currentPlayer = p;
                }
                else
                {
                    players.Add(new Player(player));
                }
            var result = new GameMode(players, data.Companies, currentPlayer, map);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveGame>(jsonLine, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            result.ActionManager.ApplyActions(data.Actions);
            return result;
        }
        public void TriggerUpdateActions()
        {
            SendLastActions?.Invoke();
        }
        public string StringState()
        {
            var result = "";
            switch(State)
            {
                case GameState.AwaitSytem:
                    {
                        result += "St: AS ";
                        break;
                    }
                case GameState.AwaitPlayers:
                    {
                        result += "St: AP ";
                        break;
                    }
                case GameState.Deploy:
                    {
                        result += "St: De ";
                        break;
                    }
                case GameState.PlayTurn:
                    {
                        result += "St: PT";
                        break;
                    }
            }
            result += " AM: " + ActionManager.ActionIdx;
            return result;
        }
    }

    public class SaveGame
    {
        public string MapFile { get; set; }
        public string Name { get; set; }

        public Dictionary<int, string> Companies { get; set; }
        public List<int> Players { get; set; }
        public int Player { get; set; }
        public List<IAction> Actions { get; set; }

    }

    public delegate void ResMoverButtonDown();

    public delegate void ActionOnId(int id);

    public delegate void ActionHandler();

    public delegate void ShowAdditionAbilityUI(string name);
}
