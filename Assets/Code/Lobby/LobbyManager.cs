using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [SerializeField] Text[] PlayerTexts;
    [SerializeField] Button readyUpButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Text text_gameSeconds;
    [SerializeField] Text text_mapNumber;
    private BumpyParkPlayerNets localPlayer;

    public static int ModeMapNumber { get; private set; } = 1;
    public static int ModeDuration { get; private set; } = 10;
    public static bool ModeHasObstacle { get; private set; } = false;

    private void Awake() => Instance = this;

    public void AssignPlayer ()
    {

    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    #region Public set game mode
    public void SetMap (float map)
    {
        ModeMapNumber = Mathf.RoundToInt(map);
        text_mapNumber.text = ModeMapNumber.ToString();
    }

    public void SetGameSeconds (float seconds)
    {
        ModeDuration = Mathf.RoundToInt(seconds);
        text_gameSeconds.text = ModeDuration.ToString();
    }

    public void SetObstacle (bool hasObstacle)
    {
        ModeHasObstacle = hasObstacle;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "Map " + ModeMapNumber);
        GUI.Label(new Rect(20, 40, 200, 20), "Seconds " + ModeDuration);
        GUI.Label(new Rect(20, 60, 200, 20), "HasObstacle " + ModeHasObstacle);
    }
    #endregion

    //public override void OnServerConnect (Network)
    //public void AssignPlayerToSlot(BumpyParkPlayerNets _player, bool _left, int _slotId)
    //{
    //    // Get the correct slot list depending on the left param
    //    List<LobbyPlayerSlot> slots = _left ? leftTeamSlots : rightTeamSlots;
    //    // Assign the player to the relevant slot in this list
    //    slots[_slotId].AssignPlayer(_player);
    //}

    //public void OnPlayerConnected(BumpyParkPlayerNets _player)
    //{
    //    bool assigned = false;

    //    // If the player is the localplayer, assign it
    //    if (_player.isLocalPlayer && localPlayer == null)
    //    {
    //        localPlayer = _player;
    //        localPlayer.onMatchStarted.AddListener(OnMatchStarted);
    //    }

    //    List<LobbyPlayerSlot> slots = assigningToLeft ? leftTeamSlots : rightTeamSlots;

    //    // Loop through each item in the list and run a lambda with the item at that index
    //    slots.ForEach(slot =>
    //    {
    //        // If we have assigned the value already, return from the lambda
    //        if (assigned)
    //        {
    //            return;
    //        }
    //        else if (!slot.IsTaken)
    //        {
    //            // If we haven't already assigned the player to a slot and this slot
    //            // hasn't been taken, assign the player to this slot and flag 
    //            // as slot been assigned
    //            slot.AssignPlayer(_player);
    //            slot.SetSide(assigningToLeft);
    //            assigned = true;
    //        }
    //    });

    //    for (int i = 0; i < leftTeamSlots.Count; i++)
    //    {
    //        LobbyPlayerSlot slot = leftTeamSlots[i];
    //        if (slot.IsTaken)
    //            localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
    //    }

    //    for (int i = 0; i < rightTeamSlots.Count; i++)
    //    {
    //        LobbyPlayerSlot slot = rightTeamSlots[i];
    //        if (slot.IsTaken)
    //            localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
    //    }

    //    // Flip the flag so that the next one will end up in the other list
    //    assigningToLeft = !assigningToLeft;
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // Fill the two lists with their slots
    //    leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
    //    rightTeamSlots.AddRange(rightTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());

    //    readyUpButton.onClick.AddListener(() =>
    //    {
    //        BattlecarsPlayerNet player = BattlecarsNetworkManager.Instance.LocalPlayer;
    //        player.SetReady(!player.ready);
    //    });

    //    startGameButton.onClick.AddListener(() => localPlayer.StartMatch());
    //}

    //public void OnMatchStarted()
    //{
    //    uiCam.enabled = false;
    //    gameObject.SetActive(false);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    startGameButton.interactable = AllPlayersReady();
    //}

    //private bool AllPlayersReady()
    //{
    //    int playerCount = 0;

    //    foreach (LobbyPlayerSlot slot in leftTeamSlots)
    //    {
    //        if (slot.Player == null)
    //            continue;

    //        playerCount++;

    //        if (!slot.Player.ready)
    //            return false;
    //    }

    //    foreach (LobbyPlayerSlot slot in rightTeamSlots)
    //    {
    //        if (slot.Player == null)
    //            continue;

    //        playerCount++;

    //        if (!slot.Player.ready)
    //            return false;
    //    }

    //    return playerCount >= 2 && BattlecarsNetworkManager.Instance.IsHost;
    //}
}
