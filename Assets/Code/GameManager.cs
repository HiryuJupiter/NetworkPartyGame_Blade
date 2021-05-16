using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : MonoBehaviour
{
    #region Field and mono
    public static GameManager Instance;

    const float GameDuration = 10;

    //TopdownShooterNetworkManager networkManager;
    GameHUD hud;

    float gameTimeFloat = GameDuration;
    int gameTimeInt;
    bool gameStarted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("-- Destroyed duplicate GameManager ---");
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    private void Start()
    {
        //hud = GameHUD.Instance;
        //networkManager = TopdownShooterNetworkManager.Instance;

        if (NetworkServer.active)
            StartCoroutine(DelayedGameStart());
    }
    #endregion

    IEnumerator DelayedGameStart()
    {
        yield return new WaitForSeconds(1f);
        //gameStarted = true;
        //if (DotSpawner.Instance == null)
        //    yield return null;

        //networkManager.SpawnDot();
    }

    #region GameTime
    void TickGameTime()
    {
        gameTimeFloat -= Time.deltaTime;
        if (gameTimeInt != (int)gameTimeFloat)
        {
            gameTimeInt = (int)gameTimeFloat;
            hud.SetTime(gameTimeInt);

            if (gameTimeInt <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        if (gameStarted)
        {
            gameStarted = false;
            hud.SetWinner(1);
            StartCoroutine(DelayUntilQuit());
        }
    }

    IEnumerator DelayUntilQuit()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}