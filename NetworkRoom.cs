using DesertPlanet.source;
using Godot;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DesertPlanet.source.Companies;

public partial class NetworkRoom : Node
{
    public event Action UpdateLobby;

    public event Action ServerDisconnected;

    const int Port = 7000;
    const string DefaultServerIp = "127.0.0.1";
    const int MaxConnections = 12;

    private Player CurrentPlayer = null;

    public LineEdit DOutput;

    public Dictionary<long, Player> Players = new Dictionary<long, Player>();

    public Dictionary<int, string> Companies = new Dictionary<int, string>();
    public int PlayersLoaded { get; set; } = 2;
    public LineEdit IpAdress { get; set; }

    public VBoxContainer PlayersHBox;

    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
        Multiplayer.ConnectedToServer += OnConnectedOk;
        Multiplayer.ConnectionFailed += OnConnectedFail;
        Multiplayer.ServerDisconnected += OnserverDisconnected;

        UpdateLobby += UpDataLobby;
        ServerDisconnected += CloseLobby;

        IpAdress = GetNode<LineEdit>("HBoxContainer/LineEdit2");
        PlayersHBox = GetNode<VBoxContainer>("PlayersList");
        DOutput = GetNode<LineEdit>("HBoxContainer/OutPut");
    }

    public void Join(string address)
    {
        if (address.Length == 0) return;
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(address, Port);
        if (error == Error.Failed)
        {
            GD.Print("Failed to connect due " + error);
            return;
        }
        PlayersHBox.Visible = true;
        Multiplayer.MultiplayerPeer = peer;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void StartGame()
    {
        var data = ProgramData.Data;
        if (data is null)
            data = new ProgramData();
        data.Players = Players;
        data.Companies = Companies;
        data.CurrentPlayer = CurrentPlayer;
        data.Status = ClientStatus.Multiplayer;
        GetTree().ChangeSceneToFile("res://PlanetScene.tscn");
    }

    public void Host()
    {
        Companies.Clear();
        Players.Clear();
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(Port, MaxConnections);
        if (error == Error.Failed)
        {
            GD.Print("Cannot Create Sever due " + error);
            return;
        }
        Players[1] = new Player(1);
        CurrentPlayer = Players[1];
        Companies[1] = "base";
        Multiplayer.MultiplayerPeer = peer;
        UpdateLobby.Invoke();
    }

    public void OnPlayerConnected(long id)
    {
        DOutput.Text += "OPC ";
        if (Players.ContainsKey(id))
            if (Companies.ContainsKey((int)id))
            {
                RegisterPlayer(id, (int)id, Companies[(int)id]);
                return;
            }
        RegisterPlayer(id, (int)id, "base");
    }

    public void OnStart()
    {
        Rpc("StartGame");
        var data = ProgramData.Data;
        if (data is null)
            data = new ProgramData();
        data.Players = Players;
        data.Companies = Companies;
        data.CurrentPlayer = CurrentPlayer;
        data.Status = ClientStatus.Multiplayer;
        GetTree().ChangeSceneToFile("res://PlanetScene.tscn");
    }
    public void OnTest()
    {
        Rpc("SendTestMessage");
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SendTestMessage()
    {
        DOutput.Text += "Test M";
    }

    public void OnChangeCompany(long i)
    {
        Companies[CurrentPlayer.Id] = Company.Avalialve[(int)i];
        foreach (var peerId in Players.Keys)
            Rpc("UpdateCompanyChoose", CurrentPlayer.Id, Companies[CurrentPlayer.Id]);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RegisterPlayer(long id, int newPlayerId, string company)
    {
        DOutput.Text += "RP ";
        var newPlayer = new Player(newPlayerId);
        DOutput.Text += newPlayer.Id + " " + id;
        Players[id] = newPlayer;
        Companies[newPlayer.Id] = company;
        UpdateLobby.Invoke();
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void UpdateCompanyChoose(int id,  string companyName)
    {
        Companies[id] = companyName;
        UpdateLobby.Invoke();
    }
    public void OnConnectedOk()
    {
        DOutput.Text += "OCO ";
        var peerId = Multiplayer.GetUniqueId();
        Players[peerId] = new Player(peerId);
        CurrentPlayer = Players[peerId]; 
        UpdateLobby.Invoke();
    }
    
    public void OnPlayerDisconnected(long id)
    {
        DOutput.Text += "OCD ";
        Players.Remove(id);
        UpdateLobby.Invoke();
    }

    public void OnConnectedFail()
    {
        DOutput.Text += "OCF ";
        Multiplayer.MultiplayerPeer.Dispose();
        Multiplayer.MultiplayerPeer = null;
    }

    public void Connect()
    {
        DOutput.Text = "";
        Join(IpAdress.Text);
    }

    public void RemoveMultiplayerPeer()
    {
        Multiplayer.MultiplayerPeer.Dispose();
        Multiplayer.MultiplayerPeer = null;
    }

    public void OnserverDisconnected()
    {
        Multiplayer.MultiplayerPeer?.Dispose();
        Multiplayer.MultiplayerPeer = null;
        Players.Clear();
        ServerDisconnected.Invoke();
    }

    public void CloseLobby()
    {
        for (int i = PlayersHBox.GetChildCount() - 1; i >= 0; i--)
            PlayersHBox.RemoveChild(PlayersHBox.GetChild(i));
    }

    public void UpDataLobby()
    {
        DOutput.Text += " LC-" + Players.Values.Count;
        if (!PlayersHBox.Visible)
            PlayersHBox.Visible = true;
        for (int i = PlayersHBox.GetChildCount() - 1; i >= 0; i--)
            PlayersHBox.RemoveChild(PlayersHBox.GetChild(i));
        foreach (var player in Players.Values)
        {
            bool canChooseCompany = false;
            if (CurrentPlayer == player)
                canChooseCompany = true;
            var hb = new HBoxContainer();
            var text = new LineEdit();
            text.Text = "Player_" + player.Id.ToString();
            text.ExpandToTextLength = true;
            var oB = new OptionButton();
            foreach(var company in Company.Avalialve)
            {
                oB.AddItem(company);
                if (Companies.ContainsKey(player.Id))
                    oB.Selected = Company.Avalialve.IndexOf(Companies[player.Id]);
                else
                    oB.Selected = 0;
                if (!canChooseCompany)
                    oB.Disabled = true;
                else
                    oB.ItemSelected += OnChangeCompany;
            }
            hb.AddChild(text);
            hb.AddChild(oB);
            PlayersHBox.AddChild(hb);
        }
    }
}
