using UnityEngine;
using Mirror;

public class TestSpawner : NetworkBehaviour
{
    public static TestSpawner Instance;
    public GameObject prefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && player.isLocalPlayer)
        if (Input.GetKeyDown(KeyCode.Space) && NetworkServer.active)
        {
            SpawnEffects();
        }
    }

    public GameObject GetSpawnEffect()
    {
        return Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
    }

    public void SpawnEffects()
    {
        //GameObject spawned =  Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
        //NetworkServer.Spawn(spawned);
    }

    Vector3 GetRandomPosition ()
    {
        return new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);
    }
}
