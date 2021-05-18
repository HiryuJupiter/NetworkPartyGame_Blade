using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class BumpyNetworkRoomPlayer : NetworkBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] Text[] nameTexts;
    [SerializeField] Text[] readyTexts;
    [SerializeField] Button btnStartGame;

    [SyncVar(hook = nameof(HandleDisplayNameChanged)) ] public string DisplayName = "Loading..."; //All clients call this function when this gets updated.
    [SyncVar(hook = nameof(HandleReadyStatusChanged))] public bool IsReady = false;

    private bool isHost;
    public bool IsHost
    {
        set
        {
            isHost = value;
            btnStartGame.gameObject.SetActive(value);
        }
    }

    BeybladeNetworkManager room;
    BeybladeNetworkManager NetworkManager
    {
        get
        {
            if (room != null)
                return room;
            return room = BeybladeNetworkManager.singleton as BeybladeNetworkManager;
        }
    }

    public override void OnStartAuthority()  //When the game starts
    {
        //CmdSetDisplayName(PlayerNameInput.DisplayName);

        ui.SetActive(true);
    }

    public override void OnStartClient()
    {
        room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    void UpdateDisplay ()
    {
        if (!isLocalPlayer)
        {
            foreach (var player in room.RoomPlayers)
            {
                if(player.isLocalPlayer)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
        }

        for (int i = 0; i < nameTexts.Length; i++)
        {
            nameTexts[i].text = "Waiting";
            readyTexts[i].text = "";
        }

        for (int i = 0; i < room.RoomPlayers.Count; i++)
        {
            nameTexts[i].text = room.RoomPlayers[i].DisplayName;
            readyTexts[i].text = room.RoomPlayers[i].IsReady ? "Ready" :"Not Ready";
        }
    }

    public void HandleReadyToStart (bool readyToStart)
    {
        if (!isHost) return;
        btnStartGame.interactable = readyToStart;
    }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp ()
    {
        IsReady = !IsReady;
        room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame ()
    {
        //
        //room.StartGame();
    }
}