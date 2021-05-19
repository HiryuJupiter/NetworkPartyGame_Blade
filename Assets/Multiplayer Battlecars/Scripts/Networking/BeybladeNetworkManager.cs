using UnityEngine;
using Mirror;
using System.Linq;
using System.Collections.Generic;

public class BeybladeNetworkManager : NetworkManager
{
    public static BeybladeNetworkManager Instance => singleton as BeybladeNetworkManager;

    public PlayerNet LocalPlayer
    {
        get
        {
            foreach (PlayerNet player in players.Values)
            {
                if (player.isLocalPlayer) return player;
            }
            return null;
        }
    }

    public string GameName { get; set; }
    public string PlayerName { get; set; }
    public int PlayerCount => players.Count;

    public bool IsHost { get; private set; } = false;

    public BattlecarsNetworkDiscovery discovery;

    private Dictionary<byte, PlayerNet> players = new Dictionary<byte, PlayerNet>();

    public override void OnStartHost()
    {
        IsHost = true;
        discovery.AdvertiseServer();
    }

    public PlayerNet GetPlayerForId(byte _playerId)
    {
        PlayerNet player;
        players.TryGetValue(_playerId, out player);
        return player;
    }

    // Runs when a client connects to the server. This function is responsible for creating the player object and placing it in the scene. It is also responsible for making sure the connection is aware of what their player object is.
    public override void OnServerAddPlayer(NetworkConnection _connection)
    {
        // Give us the next spawn position depending on the spawnMode
        Transform spawnPos = GetStartPosition();

        // Spawn a player and try to use the spawnPos
        GameObject playerObj =
            spawnPos != null
            ? Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation)
            : Instantiate(playerPrefab);

        // Assign the players ID and add them to the server based on the connection
        AssignPlayerId(playerObj);
        // Associates the player GameObject to the network connection on the server
        NetworkServer.AddPlayerForConnection(_connection, playerObj);
    }

    public void RemovePlayer(byte _id)
    {
        // If the player is present in the dictionary, remove them
        if (players.ContainsKey(_id))
        {
            players.Remove(_id);
        }
    }

    public void AddPlayer(PlayerNet _player)
    {
        if (!players.ContainsKey(_player.playerId))
        {
            players.Add(_player.playerId, _player);
        }
    }

    protected void AssignPlayerId(GameObject _playerObj)
    {
        byte id = 0;
        //List<string> playerUsernames = players.Values.Select(x => x.username).ToList();
        // Generate a list that is sorted by the keys value
        List<byte> playerIds = players.Keys.OrderBy(x => x).ToList();
        // Loop through all keys (playerID's) in the player dictionary
        foreach (byte key in playerIds)
        {
            // If the temp id matches this key, increment the id value
            if (id == key)
                id++;
        }

        // Get the playernet component from the gameobject and assign it's playerid
        PlayerNet player = _playerObj.GetComponent<PlayerNet>();
        player.playerId = id;
        players.Add(id, player);
    }
}