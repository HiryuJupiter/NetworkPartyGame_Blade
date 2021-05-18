using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TopDownCharacterController : NetworkBehaviour
{
    #region MonoBehavior
    //Bullet
    [SerializeField] private GameObject bulletPf;
    [SerializeField] private Transform shootPoint;
    
    //Movement
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 500f;

    //Health
    [SerializeField] private int maxHealth = 10;
    [SyncVar] private int currentHealth = 10; //SyncVar means it must be synchronizd among instances

    //Text
    [SerializeField] TextMesh text;

    //Ref
    private BeybladeNetworkManager networkManager;
    private Rigidbody2D rb;

    //Status
    private int playerIndex;
    [SyncVar]
    private int score;
    bool isMoving;
    bool doDamp;
    float curMoveDur;
    float dampAmount = .97f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        networkManager = BeybladeNetworkManager.Instance;
    }

    private void Update()
    {
        //Don't let other players control your player
        if (!isLocalPlayer)
            return;

        if (isMoving)
        {
            MovingUpdate();
        }
        else
        {
            RotateArrow();
            ShootBallWhenPressed();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CmdSpawnEffect();
        }
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
    }

    [Command]
    public void CmdSpawnEffect()
    {
        NetworkServer.Spawn(TestSpawner.Instance.GetSpawnEffect());
    }
    #endregion

    #region Rotating and waiting to shoot
    //[Command]
    void RotateArrow()
    {
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }

    //[ClientRpc] //Only the client gets to shoot the ball
    void ShootBallWhenPressed()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) //Shoot
        {
            BeginMoving();
        }
    }

    #endregion

    #region Moving
    void BeginMoving()
    {
        rb.velocity = transform.up * moveSpeed;
        doDamp = false;
        isMoving = true;
        curMoveDur = 0f;
    }

    void MovingUpdate()
    {
        if (doDamp)
        {
            rb.velocity *= dampAmount;
            if (rb.velocity.sqrMagnitude < 1f)
            {
                isMoving = false;
            }
        }
        else
        {
            curMoveDur += Time.deltaTime;
            if (curMoveDur > 0.5f)
                doDamp = true;
        }

        MakeArrowFaceMovingDirection();
    }

    void MakeArrowFaceMovingDirection()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
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
}