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

    //Map settings
    [SyncVar] public bool NightOrDay = true;
    [SyncVar] public bool IsMap1 = true;
    [SyncVar] public float TimeLimit = 10f;
    [SyncVar] public float SpawnInterval = 4f;

    public UnityEvent onMatchStarted = new UnityEvent();

    private bool hasJoinedLobby = false;
    public int score => GetComponent<ActualPlayer>().score;

    //Night or day
    public void SetNightOrDay(bool isTrue) => CmdSetNightOrDay(isTrue);
    [Command]
    public void CmdSetNightOrDay(bool isTrue) => AssignIsNightOrDay(isTrue);
    [ClientRpc]
    public void AssignIsNightOrDay(bool isTrue)
    {
        NightOrDay = isTrue;
        Camera.main.backgroundColor = isTrue ? Color.black : Color.grey;
    }

    //Is map 1
    public void SetIsMap1(bool isTrue) => CmdSetIsMap1(isTrue);
    [Command]
    public void CmdSetIsMap1(bool isTrue) => AssignIsMap1(isTrue);
    [ClientRpc]
    public void AssignIsMap1(bool isTrue)
    {
        IsMap1 = isTrue;
        GameManager.Instance.SetMap(IsMap1);
    }

    //Time limit
    public void SetTimeLimit(float amount) => CmdSetTimeLimit(amount);
    [Command]
    public void CmdSetTimeLimit(float amount) => AssignSetTimeLimit(amount);
    [ClientRpc]
    public void AssignSetTimeLimit(float amount)
    {
        TimeLimit = amount;
        StartCoroutine(GameManager.Instance.BeginGameCountdown(amount));
    }

    //Spawn interval
    public void AssignSpawnInterval(float amount) => CmdAssignSpawnInterval(amount);
    [Command]
    public void CmdAssignSpawnInterval(float amount) => AssignSetSpawnInterval(amount);
    [ClientRpc]
    public void AssignSetSpawnInterval(float amount)
    {
        SpawnInterval = amount;
        StartCoroutine(SpawnDotsRegularly(amount));
    }

    public void UpdateMapSettings()
    {
        //StartCoroutine(GameManager.Instance.BeginGameCountdown();
        //GameManager.Instance.SetMap(IsMap1);
        //GameManager.Instance.SetMap(IsMap1);
    }


    public void StartMatch()
    {
        if (isLocalPlayer)
        {
            CmdStartMatch();
            //GetComponent<ActualPlayer>().StartGame();
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
    public void CmdStartMatch()
    {
        RpcStartMatch();
    }
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
    Lobby lobby;
    [ClientRpc]
    public void RpcStartMatch()
    {
        PlayerNet player = BeybladeNetworkManager.Instance.LocalPlayer;

        lobby = FindObjectOfType<Lobby>();
        lobby.OnMatchStarted();
        //StartCoroutine(SpawnDotsRegularly());

        GameManager gm = FindObjectOfType<GameManager>();
        //StartCoroutine(gm.BeginGameCountdown());
        //gm.SetMap();

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

    IEnumerator SpawnDotsRegularly(float interval)
    {
        Debug.Log("interval: " + interval);
        if (BeybladeNetworkManager.Instance.IsHost && isLocalPlayer)
        {
            while (TestSpawner.Instance != null)
            {
                NetworkServer.Spawn(TestSpawner.Instance.GetSpawnEffect());
                yield return new WaitForSeconds(interval);
            }
        }
    }

    #endregion

    void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetUsername(BeybladeNetworkManager.PlayerName);
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
