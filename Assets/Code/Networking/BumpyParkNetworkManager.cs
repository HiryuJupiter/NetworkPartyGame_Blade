using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BumpyParkNetworkManager : NetworkManager
{
    public static BumpyParkNetworkManager Instance => (BumpyParkNetworkManager)singleton;

    public bool IsHost { get; private set; } = false;

    public override void OnStartHost()
    {
        IsHost = true;
    }
}