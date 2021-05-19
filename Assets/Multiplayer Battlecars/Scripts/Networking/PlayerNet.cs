using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Mirror;

using System.Collections;
public class PlayerNet : NetworkBehaviour
{
    [SyncVar] public byte playerId;
    [SyncVar] public string username = "";
    [SyncVar] public bool ready = false;

    public UnityEvent onMatchStarted = new UnityEvent();

    private Lobby lobby;
    private bool hasJoinedLobby = false;

    public int score => GetComponent<ActualPlayer>().score;

    public void StartMatch()
    {
        if (isLocalPlayer)
        {
            CmdStartMatch();
        }
    }

    public void SetReady(bool _ready)
    {
        if (isLocalPlayer)
        {
            // Only localplayers can call Commands as localplayers are the only
            // ones who have the authority to talk to the server
            CmdSetReady(_ready);
        }
    }

    public void AssignPlayerToSlot(int _slotId, byte _playerId)
    {
        if (isLocalPlayer)
        {
            CmdAssignPlayerToLobbySlot(_slotId, _playerId);
        }
    }

    #region Commands
    [Command]
    public void CmdSetUsername(string _name) => username = _name;
    [Command]
    public void CmdSetReady(bool _ready) => ready = _ready;
    [Command]
    public void CmdAssignPlayerToLobbySlot(int _slotId, byte _playerId) => RpcAssignPlayerToLobbySlot(_slotId, _playerId);
    [Command]
    public void CmdStartMatch() => RpcStartMatch();
    #endregion

    #region RPCs
    [ClientRpc]
    public void RpcAssignPlayerToLobbySlot(int _slotId, byte _playerId)
    {
        // If this is running on the host client, we don't need to set the player
        // to the slot, so just ignore this call
        if (BeybladeNetworkManager.Instance.IsHost)
            return;

        // Find the Lobby in the scene and set the player to the correct slot
        StartCoroutine(AssignPlayerToLobbySlotDelayed(BeybladeNetworkManager.Instance.GetPlayerForId(_playerId), _slotId));
    }

    [ClientRpc]
    public void RpcStartMatch()
    {
        PlayerNet player = BeybladeNetworkManager.Instance.LocalPlayer;

        FindObjectOfType<Lobby>().OnMatchStarted();
        StartCoroutine(SpawnDotsRegularly());
        StartCoroutine(FindObjectOfType<GameManager>().BeginGameCountdown());
        FindObjectOfType<GameHUD>().SetNightAndDay();
        //FindObjectOfType<Lobby>().OnMatchStarted();
        player.GetComponent<ActualPlayer>().Enable();
    }
    #endregion

    #region Coroutines
    private IEnumerator AssignPlayerToLobbySlotDelayed(PlayerNet _player, int _slotId)
    {
        // Keep trying to get the lobby until it's not null
        Lobby lobby = FindObjectOfType<Lobby>();
        while (lobby == null)
        {
            yield return null;

            lobby = FindObjectOfType<Lobby>();
        }

        // Lobby successfully got, so assign the player
        lobby.AssignPlayerToSlot(_player, _slotId);
    }

    IEnumerator SpawnDotsRegularly()
    {
        //if (BeybladeNetworkManager.Instance.IsHost && isLocalPlayer)
        if (BeybladeNetworkManager.Instance.IsHost)
        {
            while (TestSpawner.Instance != null)
            {
                NetworkServer.Spawn(TestSpawner.Instance.GetSpawnEffect());
                yield return new WaitForSeconds(Lobby.SpawnSpeed);
            }
        }
    }

    #endregion

    void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetUsername(BeybladeNetworkManager.Instance.PlayerName);
        }
    }

    void Update()
    {
        if (BeybladeNetworkManager.Instance.IsHost)
        {
            // Attempt to get the lobby if we haven't already joined a lobby
            if (lobby == null && !hasJoinedLobby)
                lobby = FindObjectOfType<Lobby>();

            // Attempt to join the lobby if we haven't already and the lobby is set
            if (lobby != null && !hasJoinedLobby)
            {
                hasJoinedLobby = true;
                lobby.OnPlayerConnected(this);
            }
        }
    }

    public override void OnStartClient()
    {
        BeybladeNetworkManager.Instance.AddPlayer(this);
    }

    public override void OnStartLocalPlayer()
    {
        SceneManager.LoadSceneAsync("LobbyRoom", LoadSceneMode.Additive);
    }

    public override void OnStopClient()
    {
        BeybladeNetworkManager.Instance.RemovePlayer(playerId);
    }
}
