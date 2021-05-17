using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineMenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ConnectionsMenu;
    public GameObject JoinMenu;
    public GameObject hostMenu;
    public InputField joinPlayerNameInputfield;
    public InputField hostPlayerNameInputField;
    private GameObject activeMenu;

    private TopdownShooterNetworkManager networkManager;

    private void Awake()
    {
        activeMenu = MainMenu;
    }

    private void Start()
    {
        networkManager = TopdownShooterNetworkManager.Instance;
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

    void OpenGameObject (GameObject go)
    {
        activeMenu = go;
        activeMenu.SetActive(true);
    }

    void CloseActive() => activeMenu?.SetActive(false);
    #endregion

    #region Server starting
    public void JoinGame ()
    {
        networkManager.StartClient();
    }

    public void HostGame ()
    {
        networkManager.PlayerName = hostPlayerNameInputField.text;
        networkManager.StartHost();
    }
    #endregion

    //public bool IsJoinPlayerNamed() => !string.IsNullOrEmpty(hostPlayerNameInputField.text);
}