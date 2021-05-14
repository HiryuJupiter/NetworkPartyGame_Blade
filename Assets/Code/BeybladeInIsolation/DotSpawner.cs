using System.Collections;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    public static DotSpawner Instance;

    [SerializeField] GameObject dot;
    [SerializeField] float spawnRange_x;
    [SerializeField] float spawnRange_y;

    private void Awake()
    {
        Instance = this;
        SpawnDot();
    }

    void SpawnDot()
    {
        Instantiate(dot, GetSpawnPos(), Quaternion.identity);
    }

    public static void Spawn() => Instance.SpawnDot();


    Vector2 GetSpawnPos ()
    {
        return new Vector2(Random.Range(-spawnRange_x, spawnRange_x), Random.Range(-spawnRange_y, spawnRange_y));
    }
}