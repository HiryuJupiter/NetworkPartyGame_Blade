using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownCharacterController : NetworkBehaviour
{
    #region MonoBehavior
    //Bullet
    [SerializeField] GameObject bulletPf;
    [SerializeField] Transform shootPoint;
    
    //Movement
    [SerializeField] float moveSpeed = 10f;

    //Health
    [SerializeField] int maxHealth = 10;
    [SyncVar] int currentHealth = 10; //SyncVar means it must be synchronizd among instances

    //Ref
    TopdownShooterNetworkManager networkManager;
    Rigidbody2D rb;

    //Status
    int playerIndex;
    int score;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        networkManager = TopdownShooterNetworkManager.Instance;
    }

    void Update()
    {
        //Don't let other players control your player
        if (!isLocalPlayer) 
            return;

        //if (isMoving)
        //{
        //    MovingUpdate();
        //}
        //else
        //{
        //    RotateArrow();
        //    ShootBallWhenPressed();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //    CmdIncrementScore();

        if (Input.GetKeyDown(KeyCode.P))
        {
            //CmdSpawnEnemy();
            //if (NetworkServer.active)
            //{
            //    CmdSpawnEnemy();
            //    //EnemySpawner.Instance.SpawnEnemy();
            //}
            //else
            //{
            //    Debug.Log("spawn enemy failed, no active server");
            //}
        }

        MovementUpdate();
        RotateTowardMouse();
        ShootUpdate();
    }
    #endregion

    #region Movement
    void MovementUpdate ()
    {
        Vector3 vel = Vector3.zero;
        vel.x = Input.GetAxis("Horizontal") * moveSpeed;
        vel.y = Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity = vel;
    }

    void RotateTowardMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 lookingDir = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, lookingDir);
    }
    #endregion

    #region Shoot

    void ShootUpdate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) //Shoot
        {
            //EnemySpawner.Instance.CmdShoot(bulletPf, shootPoint);
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        GameObject bullet = Instantiate(bulletPf, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * 5f;
        //bullet.GetComponent<Bullet>().Shoot (lookingDir , playerIndex);
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 3f);
    }
    #endregion

    #region Taking Damage
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.tag == "Bullet")
        //{
        //    TakeDamage(1);
        //    Destroy(collider.gameObject);
        //}
        if (collider.tag == "Enemy")
        {
            TakeDamage(1);
            Destroy(collider.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        if (isServer) //Is on server and was spawned
        {
            currentHealth -= amount;
            Destroy(gameObject);
            RpcOnDamagedFeedback();
        }
    }

    //[Command] send from clients to the server
    [ClientRpc] // ClientRpc is executed in the client, even if it gets called in server
    private void RpcOnDamagedFeedback()
    {
        Debug.Log("Player hit by bullet");
    }
    #endregion
}