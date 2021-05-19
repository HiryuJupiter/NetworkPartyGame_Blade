using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineMenuManager : MonoBehaviour
{
    public static string PlayerName = "Playername";

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject ConnectionsMenu;
    [SerializeField] GameObject JoinMenu;
    [SerializeField] GameObject hostMenu;
    [SerializeField] InputField joinPlayerNameInputfield;
    [SerializeField] InputField hostPlayerNameInputField;
    [SerializeField] Button buttonJoinGame;
    [SerializeField] Button buttonHostGame;

    private GameObject activeMenu;

    private BeybladeNetworkManager networkManager;

    private void Awake()
    {
        activeMenu = MainMenu;
    }

    private void Start()
    {
        networkManager = BeybladeNetworkManager.Instance;
    }

    #region Menu opening and closing
    public void OpenConenctionsMenu()
    {
        CloseActive();
        OpenGameObject(ConnectionsMenu);
    }

    public void OpenJoinPanel()
    {
        CloseActive();
        OpenGameObject(JoinMenu);
    }

    public void OpenHostPanel()
    {
        CloseActive();
        OpenGameObject(hostMenu);
    }

    void OpenGameObject(GameObject go)
    {
        activeMenu = go;
        activeMenu.SetActive(true);
    }

    void CloseActive() => activeMenu?.SetActive(false);
    #endregion

    #region Input field update
    public void InputFieldUpdate_JoinGameUsername()
    {
        buttonJoinGame.interactable = !string.IsNullOrEmpty(joinPlayerNameInputfield.text);
    }

    public void InputFieldUpdate_HostGameUsername()
    {
        buttonHostGame.interactable = !string.IsNullOrEmpty(hostPlayerNameInputField.text);
    }
    #endregion

    #region Server starting
    public void JoinGame()
    {
        PlayerName = joinPlayerNameInputfield.text;
        networkManager.StartClient();
        CloseActive();
    }

    public void HostGame()
    {
        PlayerName = hostPlayerNameInputField.text;
        networkManager.StartHost();
        CloseActive();
        Debug.Log("start host");
    }
    #endregion

    //public bool IsJoinPlayerNamed() => !string.IsNullOrEmpty(hostPlayerNameInputField.text);
}