using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.Events;



public class DiscoveryRequest : NetworkMessage //Send to the client. Customize this how you want.
{
    public string gameName;
}

public class DiscoveryResponse : NetworkMessage //Received then converted and produced by the client
{
    // The client fills this up after we receive it.
    public IPEndPoint EndPoint { get; set; }
    public Uri uri;
    public long serverID;
    public string gameName;
}

[Serializable] public class ServerFoundEvent : UnityEvent<DiscoveryResponse> { }


//NetworkDiscovery is Mirror's way of automatically detecting new games without needing you type in their IP.
//It works by being to broadcasting and also to constantly listening for NetworkMessages.
//We create our own network dicovery class to process custom discovery requests and responses.
//NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse> is defining the two generic characters in 
public class BumpyParkNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        throw new NotImplementedException();
    }

    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
    {
        throw new NotImplementedException();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}