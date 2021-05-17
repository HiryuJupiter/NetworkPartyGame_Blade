using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TopdownShooterNetworkManager : NetworkManager
{
    const int MainMenuSceneIndex = 1;

    public string PlayerName;
    public string GameName;

    //The player representation in the lobby
    [SerializeField] BumpyNetworkRoomPlayer roomPlayerPrefab = null;
    [SerializeField] Beyblade gamePlayerPrefab = null;
    [SerializeField] GameObject roundSystem = null;
    [SerializeField] GameObject dotSpawnerPf;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    DotSpawner dotSpawner;

    int TimeLimit;
    int ScoreLimit;
    bool nightMode;
    int mapIndex;

    public List<BumpyNetworkRoomPlayer> RoomPlayers { get; } = new List<BumpyNetworkRoomPlayer>();
    //Player name
    //Player colour

    public static TopdownShooterNetworkManager Instance => (TopdownShooterNetworkManager)singleton;

    public bool IsHost { get; private set; } = false;

    //Network manager has an empty virtual method OnStartHost. We override it to implemnent
    //our custom features.
    public override void OnStartHost()
    {
        base.OnStartHost();
        IsHost = true;
    }

    public void SpawnDot ()
    {
        if (dotSpawner == null)
            dotSpawner = DotSpawner.Instance;

        if (dotSpawner != null)
            dotSpawner.SpawnDot();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    bool IsReadyToStart()
    {
        foreach (var players in RoomPlayers)
        {
            //if (!player.IsReady) return false;
        }
        return true;
    }
}