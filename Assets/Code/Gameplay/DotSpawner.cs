using System.Collections;
using UnityEngine;
using Mirror;

public class DotSpawner : NetworkBehaviour
{
    public static DotSpawner Instance;

    [SerializeField] GameObject dotPf;
    [SerializeField] float spawnRange_x;
    [SerializeField] float spawnRange_y;

    private void Awake()
    {
        Instance = this;
    }

    //[Command]
    void SpawnDot()
    {
        GameObject dot = Instantiate(dotPf, GetSpawnPos(), Quaternion.identity);
        NetworkServer.Spawn(dot);
    }

    public static void Spawn() => Instance.SpawnDot();


    Vector2 GetSpawnPos ()
    {
        return new Vector2(Random.Range(-spawnRange_x, spawnRange_x), Random.Range(-spawnRange_y, spawnRange_y));
    }
}