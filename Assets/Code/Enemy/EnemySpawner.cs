using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawner : NetworkBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField]
    GameObject enemyPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P) && NetworkServer.active)
        //if (Input.GetKeyDown(KeyCode.P) && isServer)
        if (Input.GetKeyDown(KeyCode.P) && isLocalPlayer)
            {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(enemy);
    }

    [SerializeField]
    float enemySpeed = 1.0f;


    //public override void OnStartServer()
    //{
    //    InvokeRepeating("SpawnEnemy", this.spawnInterval, this.spawnInterval);
    //}


    //public void SpawnEnemy()
    //{
    //    Vector2 spawnPosition = new Vector2(Random.Range(-4.0f, 4.0f), this.transform.position.y);
    //    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
    //    enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -this.enemySpeed);
    //    //if (NetworkServer.active)

    //    NetworkServer.Spawn(enemy);

    //    Destroy(enemy, 10);
    //}
}
