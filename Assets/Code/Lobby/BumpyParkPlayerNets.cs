using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BumpyParkPlayerNets : NetworkBehaviour
{
    [SyncVar] public byte playerId;
    [SyncVar] public string username = "";
    [SyncVar] public bool ready = false;

    public UnityEvent onMatchStarted = new UnityEvent();

    private LobbyManager lobby;
    private bool hasJoinedLobby = false;

    //#region Start match
    //public void StartMatch()
    //{
    //    if (isLocalPlayer)
    //        CmdStartMatch();
    //}
    //[Command]
    //public void CmdStartMatch() => RpcStartMatch();
    //[ClientRpc]
    //public void RpcStartMatch()
    //{
    //    BumpyParkPlayerNets player = TopdownShooterNetworkManager.Instance.LocalPlayer;
    //    FindObjectOfType<LobbyUIManager>().OnMatchStarted();
    //}
    //#endregion

    //#region Set username
    //public void SetUsername(string _name)
    //{
    //    if (isLocalPlayer)
    //    {
    //        // Only localplayers can call Commands as localplayers are the only
    //        // ones who have the authority to talk to the server
    //        CmdSetUsername(_name);
    //    }
    //}
    //[Command]
    //public void CmdSetUsername(string _name) => username = _name;

    //#endregion

    //#region Set ready
    //public void SetReady(bool _ready)
    //{
    //    if (isLocalPlayer)
    //    {
    //        // Only localplayers can call Commands as localplayers are the only
    //        // ones who have the authority to talk to the server
    //        CmdSetReady(_ready);
    //    }
    //}
    //[Command]
    //public void CmdSetReady(bool _ready) => ready = _ready;
    //#endregion

    //#region Assign player to slot
    //public void AssignPlayerToSlot(bool _left, int _slotId, byte _playerId)
    //{
    //    if (isLocalPlayer)
    //    {
    //        CmdAssignPlayerToLobbySlot(_left, _slotId, _playerId);
    //    }
    //}

    //[Command]
    //public void CmdAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId) => RpcAssignPlayerToLobbySlot(_left, _slotId, _playerId);

    //[ClientRpc]
    //public void RpcAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId)
    //{
    //    // If this is running on the host client, we don't need to set the player
    //    // to the slot, so just ignore this call
    //    if (TopdownShooterNetworkManager.Instance.IsHost)
    //        return;

    //    // Find the Lobby in the scene and set the player to the correct slot
    //    StartCoroutine(AssignPlayerToLobbySlotDelayed(TopdownShooterNetworkManager.Instance.GetPlayerForId(_playerId), _left, _slotId));
    //}
    //#endregion


    //#region Coroutines
    //private IEnumerator AssignPlayerToLobbySlotDelayed(BumpyParkPlayerNet _player, bool _left, int _slotId)
    //{
    //    // Keep trying to get the lobby until it's not null
    //    LobbyUIManager lobby = FindObjectOfType<LobbyUIManager>();
    //    while (lobby == null)
    //    {
    //        yield return null;

    //        lobby = FindObjectOfType<LobbyUIManager>();
    //    }

    //    // Lobby successfully got, so assign the player
    //    lobby.AssignPlayerToSlot(_player, _left, _slotId);
    //}
    //#endregion

    //// Start is called before the first frame update
    //void Start()
    //{
    //    SetUsername(TopdownShooterNetworkManager.Instance.PlayerName);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Determine if we are on the host client
    //    if (TopdownShooterNetworkManager.Instance.IsHost)
    //    {
    //        // Attempt to get the lobby if we haven't already joined a lobby
    //        if (lobby == null && !hasJoinedLobby)
    //            lobby = FindObjectOfType<LobbyUIManager>();

    //        // Attempt to join the lobby if we haven't already and the lobby is set
    //        if (lobby != null && !hasJoinedLobby)
    //        {
    //            hasJoinedLobby = true;
    //            lobby.OnPlayerConnected(this);
    //        }
    //    }
    //}

    //public override void OnStartClient()
    //{
    //    TopdownShooterNetworkManager.Instance.AddPlayer(this);
    //}

    //// Runs only when the object is connected is the local player
    //public override void OnStartLocalPlayer()
    //{
    //    // Load the scene with the lobby
    //    SceneManager.LoadSceneAsync("InGameMenus", LoadSceneMode.Additive);
    //}

    //// Runs when the client is disconnected from the server
    //public override void OnStopClient()
    //{
    //    // Remove the playerID from the server
    //    TopdownShooterNetworkManager.Instance.RemovePlayer(playerId);
    //}
}