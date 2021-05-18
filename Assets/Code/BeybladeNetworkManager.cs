//using Mirror;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class BeybladeNetworkManager : NetworkManager
//{
//    public static BeybladeNetworkManager Instance => singleton as BeybladeNetworkManager;
//    public static event Action OnClientConnected;
//    public static event Action OnClientDisconnected;
//    public static event Action<NetworkConnection> OnServerReadied;
//    public static event Action OnServerStopped;

//    const string Game = "Game";
//    const string Lobby = "Lobby";
//    const string Offline = "Offline";

//    [SerializeField] private int minPlayers = 2;

//    [Header("Maps")]
//    [SerializeField] private int numberOfRounds = 1;

//    [Header("Room")]
//    [SerializeField] private RoomPlayer roomPlayerPrefab = null;

//    [Header("Game")]
//    [SerializeField] private GamePlayer gamePlayerPrefab = null;
//    [SerializeField] private GameObject playerSpawnSystem = null;

//    public List<RoomPlayer> RoomPlayers { get; } = new List<RoomPlayer>();
//    public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();

//    bool CurrentlyInOfflineScene => SceneManager.GetActiveScene().name == Offline;
//    bool CurrentlyInGameScene => SceneManager.GetActiveScene().name == "Game";

//    public override void OnClientConnect(NetworkConnection conn)
//    {
//        base.OnClientConnect(conn);

//        OnClientConnected?.Invoke();
//    }

//    public override void OnClientDisconnect(NetworkConnection conn)
//    {
//        base.OnClientDisconnect(conn);

//        OnClientDisconnected?.Invoke();
//    }

//    public override void OnServerConnect(NetworkConnection conn)
//    {
//        if (numPlayers >= maxConnections)
//        {
//            conn.Disconnect();
//            return;
//        }

//        if (!CurrentlyInOfflineScene)
//        {
//            conn.Disconnect();
//            return;
//        }
//    }

//    public override void OnServerAddPlayer(NetworkConnection conn)
//    {
//        Debug.Log("---OnServerAddPlayer");
//        if (CurrentlyInOfflineScene)
//        {
//            Debug.Log("---is in menuScene");
//            bool isLeader = RoomPlayers.Count == 0;

//            RoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

//            roomPlayerInstance.IsLeader = isLeader;

//            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
//        }
//        else
//        {
//            Debug.Log("---NOT in menuScene");
//        }
//    }

//    public override void OnServerDisconnect(NetworkConnection conn)
//    {
//        if (conn.identity != null)
//        {
//            var player = conn.identity.GetComponent<RoomPlayer>();

//            RoomPlayers.Remove(player);

//            NotifyPlayersOfReadyState();
//        }

//        base.OnServerDisconnect(conn);
//    }

//    public override void OnStopServer()
//    {
//        OnServerStopped?.Invoke();

//        RoomPlayers.Clear();
//        GamePlayers.Clear();
//    }

//    public void NotifyPlayersOfReadyState()
//    {
//        foreach (var player in RoomPlayers)
//        {
//            player.HandleReadyToStart(IsReadyToStart());
//        }
//    }

//    private bool IsReadyToStart()
//    {
//        //if (numPlayers < minPlayers) { return false; }

//        foreach (var player in RoomPlayers)
//        {
//            if (!player.IsReady) { return false; }
//        }

//        return true;
//    }

//    public void StartGame()
//    {
//        if (CurrentlyInOfflineScene)
//        {
//            //Debug.Log("==StartGame a ==");
//            if (!IsReadyToStart()) { return; }

//            ServerChangeScene(Game);
//        }
//    }

//    public override void ServerChangeScene(string newSceneName)
//    {
//        // From menu to game
//        if (CurrentlyInOfflineScene && newSceneName == "Game")
//        {
//            Debug.Log("==ServerChangeScene. RoomPlayers count: " + RoomPlayers.Count);
//            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
//            {
//                NetworkConnection conn = RoomPlayers[i].connectionToClient;
//                GamePlayer gameplayerInstance = Instantiate(gamePlayerPrefab);
//                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

//                NetworkServer.Destroy(conn.identity.gameObject);

//                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
//            }
//        }

//        base.ServerChangeScene(newSceneName);
//    }

//    public override void OnServerSceneChanged(string sceneName)
//    {
//        if (sceneName == "Game")
//        {
//            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
//            NetworkServer.Spawn(playerSpawnSystemInstance);
//        }
//    }

//    public override void OnServerReady(NetworkConnection conn)
//    {
//        base.OnServerReady(conn);

//        OnServerReadied?.Invoke(conn);
//    }
//}
