using UnityEngine;
using Mirror;

public class TestSpawner : NetworkBehaviour
{
    public GameObject spawnPrefab;
    public NetworkBehaviour player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isLocalPlayer)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        GameObject enemy =  Instantiate(spawnPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(enemy);
    }
}
