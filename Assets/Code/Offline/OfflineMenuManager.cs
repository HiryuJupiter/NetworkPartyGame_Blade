using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineMenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ConnectionsMenu;
    public GameObject JoinMenu;
    public GameObject ConnectionsMenu;
    GameObject activeMenu;

    private void Awake()
    {
        activeMenu = MainMenu;
    }

    public void OpenConenctionsMenu ()
    {
        CloseActive();
        OpenGameObject(ConnectionsMenu);
    }

    public void OpenMainMenu()
    {
        CloseActive();
        OpenGameObject(ConnectionsMenu);
    }


    //Private
    void OpenGameObject (GameObject go)
    {
        activeMenu = go;
        activeMenu.SetActive(true);
    }

    void CloseActive() => activeMenu?.SetActive(false);
}
