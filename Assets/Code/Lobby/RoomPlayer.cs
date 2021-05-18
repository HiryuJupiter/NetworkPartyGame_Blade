using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Text[] playerNameTexts;
    [SerializeField] private Text[] playerReadyTexts;
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private BeybladeNetworkManager myNetworkManager;
    private BeybladeNetworkManager MyNetworkManager
    {
        get
        {
            if (myNetworkManager != null) { return myNetworkManager; }
            return myNetworkManager = NetworkManager.singleton as BeybladeNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(OfflineMenuManager.PlayerName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        MyNetworkManager.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        MyNetworkManager.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in MyNetworkManager.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < MyNetworkManager.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = MyNetworkManager.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = MyNetworkManager.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        MyNetworkManager.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (MyNetworkManager.RoomPlayers[0].connectionToClient != connectionToClient) 
        {
            Debug.Log("Not the same connection");
            return; 
        }

        MyNetworkManager.StartGame();
    }
}
