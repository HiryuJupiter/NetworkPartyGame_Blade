using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class BeybladeNetworkManager : NetworkManager
{
    [Space(20), Tooltip("Scenes")]
    string LobbySceneName = "Lobby";
    string GameplayScene = "Online";

    public string PlayerName;
    public string GameName;

    [Space(20), Tooltip("Prefabs")]
    [SerializeField] BumpyNetworkRoomPlayer roomPlayerPrefab = null;
    [SerializeField] Beyblade gamePlayerPrefab = null;
    [SerializeField] GameObject roundSystem = null;
    [SerializeField] LobbyPlayer lobbyPlayerObject;

    [Space(20), Tooltip("Network discovery")]

    public BumpyParkNetworkDiscovery discovery;


    DotSpawner dotSpawner;

    public List<LobbyPlayer> RoomPlayers { get; } = new List<LobbyPlayer>();
    //Player name
    //Player colour

    public static BeybladeNetworkManager Instance => (BeybladeNetworkManager)singleton;

    public bool IsHost { get; private set; } = false;

    //Network manager has an empty virtual method OnStartHost. We override it to implemnent
    //our custom features.
    public override void OnStartHost()
    {
        base.OnStartHost();
        IsHost = true;
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

    #region Lobby
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == LobbySceneName)
        {
            Debug.Log("--- OnServerAddPlayer. ID = " + (RoomPlayers.Count + 1));
            LobbyPlayer lobbyPlayer = Instantiate(lobbyPlayerObject);
            lobbyPlayer.isLeader = RoomPlayers.Count == 0;
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            LobbyPlayer player = conn.identity.GetComponent<LobbyPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }
    #endregion

    /* MENU TO GAMEPLAY
     
    public void StartGame()
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                if (!IsReadyToStart()) { return; }

                mapHandler = new MapHandler(mapSet, numberOfRounds);

                ServerChangeScene(mapHandler.NextMap);
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            // From menu to game
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
            {
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Scene_Map"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);

                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
            }
        }
     */
}