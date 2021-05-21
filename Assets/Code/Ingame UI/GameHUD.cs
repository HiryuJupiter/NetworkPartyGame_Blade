using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameHUD : NetworkBehaviour
{
    public static GameHUD Instance;

    [SerializeField] Text time;
    [SerializeField] Text playerWonText;
    [SerializeField] GameObject gameWonGroup;
    [SerializeField] Text nightAndDay;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTime (int seconds)
    {
        time.text = seconds.ToString();
    }

    public void SetWinner (int playerNumber)
    {
        gameWonGroup.SetActive(true);
        playerWonText.text = "Player " + playerNumber + " has won!";
    }

    public void SetNightAndDay ()
    {
        //nightAndDay.text = Lobby.Instance.NightOrDay ? "Night" : "Day";
        //Camera.main.backgroundColor = Lobby.Instance.NightOrDay ? Color.black : Color.grey;
    }
}