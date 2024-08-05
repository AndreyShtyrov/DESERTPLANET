using DesertPlanet.source;
using Godot;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class NetworkRoom : Node
{
    public event Action<int, Player> PlayerConnected;

    public event Action<int> PlayerDisconnected;

    public event Action ServerDisconnected;

    const int Port = 7000;
    const string DefaultServerIp = "127.0.0.1";
    const int MaxConnections = 12;

    public ProgramData Data { get; set; }

    public Dictionary<int, Player> Players = new Dictionary<int, Player>();

    public int PlayersLoaded { get; set; } = 1;

    public LineEdit IpAdress { get; set; }

    public VBoxContainer PlayersHBox;

    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
        Multiplayer.ConnectedToServer += OnConnectedOk;
        Multiplayer.ConnectionFailed += OnConnectedFail;
        Multiplayer.ServerDisconnected += OnserverDisconnected;

        PlayerConnected += AddPlayerToLobby;
        IpAdress = GetNode<LineEdit>("HBoxContainer/LineEdit2");
        PlayersHBox = GetNode<VBoxContainer>("PlayersList");
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
        Multiplayer.MultiplayerPeer = peer;
    }

    public void Host()
    {
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(Port, MaxConnections);
        if (error == Error.Failed)
        {
            GD.Print("Cannot Create Sever due " + error);
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        var player = new Player(1);
        PlayerConnected.Invoke(1, player);
    }

    public void OnPlayerConnected(long id)
    {
        RegisterPlayer(id);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RegisterPlayer(long id)
    {
        var _id =(int)id;
        //var _id = Multiplayer.GetRemoteSenderId();
        Players[_id] = new Player(_id);
        PlayerConnected.Invoke(_id, Players[_id]);
    }

    public void OnConnectedOk()
    {
        var peerId = Multiplayer.GetUniqueId();
        Players[peerId] = new Player(peerId);
        PlayerConnected.Invoke(peerId, Players[peerId]);
    }
    
    public void OnPlayerDisconnected(long id)
    {
        int _id = (int)id;
        Players.Remove(_id);
        PlayerDisconnected.Invoke(_id);
    }

    public void OnConnectedFail()
    {
        Multiplayer.MultiplayerPeer.Dispose();
        Multiplayer.MultiplayerPeer = null;
    }

    public void Connect()
    {
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
    }

    public void AddPlayerToLobby(int id, Player player)
    {
        Players.Add(id, player);
        if (!PlayersHBox.Visible)
            PlayersHBox.Visible = true;
        var text = new LineEdit();
        text.Text = player.Id.ToString();
        PlayersHBox.AddChild(text);
    }
}
