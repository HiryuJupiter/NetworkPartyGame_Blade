using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyPlayer : NetworkManager
{
    public string username = "";
    public bool IsReady;
    public bool isLeader;

    BeybladeNetworkManager myNeworkManager;
}