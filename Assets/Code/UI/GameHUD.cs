using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance;

    [SerializeField] Text debugText;
    [SerializeField] Text time;
    [SerializeField] Text playerWonText;
    [SerializeField] GameObject gameWonGroup;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTime (int seconds)
    {
        time.text = seconds.ToString();
    }

    public void SetDebugText(string st)
    {
        debugText.text = st;
    }

    public void SetWinner (int playerNumber)
    {
        gameWonGroup.SetActive(true);
        playerWonText.text = "Player " + playerNumber + " has won!";
    }
}