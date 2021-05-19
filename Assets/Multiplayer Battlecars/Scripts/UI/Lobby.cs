using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    List<LobbyPlayerSlot> teamSlots = new List<LobbyPlayerSlot>();
    [SerializeField] private GameObject teamHolder;
    [SerializeField] Button readyUpButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Camera uiCam;

    PlayerNet localPlayer;

    public void AssignPlayerToSlot(PlayerNet _player, int _slotId)
    {
        teamSlots[_slotId].AssignPlayer(_player);
    }

    public void OnPlayerConnected(PlayerNet _player)
    {
        bool assigned = false;

        // If the player is the localplayer, assign it
        if (_player.isLocalPlayer && localPlayer == null)
        {
            localPlayer = _player;
            localPlayer.onMatchStarted.AddListener(OnMatchStarted);
        }

        teamSlots.ForEach(slot =>
        {
            if (assigned)
                return;
            else if (!slot.IsTaken)
            {                
                slot.AssignPlayer(_player);
                assigned = true;
            }
        });

        for (int i = 0; i < teamSlots.Count; i++)
        {
            LobbyPlayerSlot slot = teamSlots[i];
            if (slot.IsTaken)
                localPlayer.AssignPlayerToSlot(i, slot.Player.playerId);
        }
    }

    void Start()
    {
        // Fill the two lists with their slots
        teamSlots.AddRange(teamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
       
        readyUpButton.onClick.AddListener(() =>
        {
            PlayerNet player = BeybladeNetworkManager.Instance.LocalPlayer;
            player.SetReady(!player.ready);
        });

        startGameButton.onClick.AddListener(() => localPlayer.StartMatch());
    }

    public void OnMatchStarted()
    {
        uiCam.enabled = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        startGameButton.interactable = AllPlayersReady();
    }

    private bool AllPlayersReady()
    {
        int playerCount = 0;

        foreach (LobbyPlayerSlot slot in teamSlots)
        {
            if (slot.Player == null)
                continue;

            playerCount++;

            if (!slot.Player.ready)
                return false;
        }

        return BeybladeNetworkManager.Instance.IsHost;
        //return playerCount >= 2 && BattlecarsNetworkManager.Instance.IsHost;
    }
}
