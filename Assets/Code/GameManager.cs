using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : MonoBehaviour
{
    #region Field and mono
    public static GameManager Instance;

    const float GameDuration = 10;
    [SerializeField] GameObject map1;
    [SerializeField] GameObject map2;

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
    #endregion

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SetMap(bool isMap1)
    {
        if (isMap1)
            map1.SetActive(true);
        else
            map2.SetActive(true);
    }

    public IEnumerator BeginGameCountdown (float time)
    {
        hud = GameHUD.Instance;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            hud.SetTime((int)time);
            yield return null;
        }

        GameHUD.Instance.SetWinner(BeybladeNetworkManager.Instance.FindWinningPlayer());
    }
}