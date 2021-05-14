using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BumpyParkNetworkManager : NetworkManager
{
    public static BumpyParkNetworkManager Instance => (BumpyParkNetworkManager)singleton;

    public bool IsHost { get; private set; } = false;

    //Network manager has an empty virtual method OnStartHost. We override it to implemnent
    //our custom features.
    public override void OnStartHost()
    {
        IsHost = true;
        StartCoroutine(ImParadoidaf());
    }

    IEnumerator ImParadoidaf()
    {
        yield return null;
        yield return null;
        GameHUD.Instance.SetDebugText("Why :,<");
        DotSpawner.Spawn();
        GameManager.Instance.GameStart();
    }

}