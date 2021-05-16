using System.Collections;
using UnityEngine;
using Mirror;

public class DotSpawner : NetworkBehaviour
{
    public static DotSpawner Instance;

    [SerializeField] GameObject dotPf;
    [SerializeField] float spawnRange_x;
    [SerializeField] float spawnRange_y;

    GameObject activeDot;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Debug.Log("Dot spawned created");
        }
    }

    private void Start()
    {
    }

    public void EatDot ()
    {
        if (activeDot == null)
        {
            Destroy(activeDot);
            SpawnDot();
        }
    }

    public void SpawnDot()
    {
        if (!NetworkServer.active) return;

        Debug.Log("Spawn dot");
        if (activeDot == null)
        {
            activeDot = Instantiate(dotPf, GetSpawnPos(), Quaternion.identity);
            NetworkServer.Spawn(activeDot);
        }
    }

    //[Command]
    //void CmdSpawnDot()
    //{
    //    //SpawnDot();
    //}

    //[ClientRpc]
    //void SpawnDot ()
    //{
    //    GameObject dot = Instantiate(dotPf, GetSpawnPos(), Quaternion.identity);
    //}

    //public void DestroySpawner()
    //{
    //    if (Instance == this)
    //    {
    //        Instance = null;
    //        Debug.Log("destroyed");
    //    }
    //}

    Vector2 GetSpawnPos()
    {
        return new Vector2(Random.Range(-spawnRange_x, spawnRange_x), Random.Range(-spawnRange_y, spawnRange_y));
    }
}