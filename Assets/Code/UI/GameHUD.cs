using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameHUD : NetworkBehaviour
{
    public static GameHUD Instance;

    [SerializeField] Text debugText;
    [SerializeField] Text time;
    [SerializeField] Text playerWonText;
    [SerializeField] GameObject gameWonGroup;

    [SerializeField] Text testText1;
    [SerializeField] Text testText2;
    int testValue1;
    int testValue2;


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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            IncrementTestValue1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CmdIncrementTestValue2();
        }
    }

    void IncrementTestValue1 ()
    {
        testValue1++;
        testText1.text = testValue1.ToString();
    }

    [Command]
    void CmdIncrementTestValue2()
    {
        RpcIncrementTestValue2();
    }

    [ClientRpc]
    void RpcIncrementTestValue2()
    {
        if (TopdownShooterNetworkManager.Instance.IsHost)
            return;

        testValue2++;
        testText2.text = testValue2.ToString();
    }
}