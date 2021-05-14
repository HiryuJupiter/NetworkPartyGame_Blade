using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Field and mono
    public static GameManager Instance;

    const float GameDuration = 10;

    GameHUD hud;

    float gameTimeFloat = GameDuration;
    int gameTimeInt;
    bool gameStarted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hud = GameHUD.Instance;
    }

    private void Update()
    {
        if (gameStarted)
            TickGameTime();
    }
    #endregion

    public void GameStart ()
    {
        gameStarted = true;
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