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
    [SerializeField] Text join_colorSettingText;
    [SerializeField] Text host_colorSettingText;

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
        BeybladeNetworkManager.PlayerName = joinPlayerNameInputfield.text;
    }

    public void InputFieldUpdate_HostGameUsername()
    {
        buttonHostGame.interactable = !string.IsNullOrEmpty(hostPlayerNameInputField.text);
        BeybladeNetworkManager.PlayerName = hostPlayerNameInputField.text;
    }

    public void UpdateColor(float index)
    {
        switch (Mathf.RoundToInt(index))
        {
            case 0:
                BeybladeNetworkManager.PlayerColor = Color.white;
                host_colorSettingText.text = "White";
                join_colorSettingText.text = "White";
                break;
            case 1:
                BeybladeNetworkManager.PlayerColor = Color.yellow;
                host_colorSettingText.text = "Yellow";
                join_colorSettingText.text = "Yellow";
                break;
            case 2:
                BeybladeNetworkManager.PlayerColor = Color.cyan;
                host_colorSettingText.text = "Cyan";
                join_colorSettingText.text = "Cyan";
                break;
        }
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