using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownCharacterController : NetworkBehaviour
{
    #region MonoBehavior
    //Bullet
    [SerializeField] private GameObject bulletPf;
    [SerializeField] private Transform shootPoint;
    
    //Movement
    [SerializeField] private float moveSpeed = 10f;

    //Health
    [SerializeField] private int maxHealth = 10;
    [SyncVar] private int currentHealth = 10; //SyncVar means it must be synchronizd among instances

    //Text
    [SerializeField] TextMesh text;

    //Ref
    private TopdownShooterNetworkManager networkManager;
    private Rigidbody2D rb;

    //Status
    private int playerIndex;
    private int score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        networkManager = TopdownShooterNetworkManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Dot")
        {
            if(isLocalPlayer)
            {
                CmdIncrementScore(collider.gameObject);
            }
        }
        if (collider.tag == "Enemy")
        {
            TakeDamage(1);
            Destroy(collider.gameObject);
        }
    }


    [Command]
    public void CmdSpawnEffect()
    {
        //Usage:
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    CmdSpawnEffect();
        //}
        NetworkServer.Spawn(TestSpawner.Instance.GetSpawnEffect());
    }

    private void Update()
    {
        //Don't let other players control your player
        if (!isLocalPlayer) 
            return;

        if (Input.GetKeyDown(KeyCode.B))
        {
            CmdSpawnEffect();
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
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        GameObject bullet = Instantiate(bulletPf, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * 5f;
        //bullet.GetComponent<Bullet>().Shoot(lookingDir, playerIndex);
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 3f);
    }
    #endregion

    #region Score
    [Command]
    void CmdIncrementScore(GameObject toDestroy)
    {
        RpcIncrementScore();
        NetworkServer.Destroy(toDestroy);
    }

    [ClientRpc]
    void RpcIncrementScore()
    {
        score++;
        text.text = score.ToString();
    }
    #endregion

    #region Taking Damage
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