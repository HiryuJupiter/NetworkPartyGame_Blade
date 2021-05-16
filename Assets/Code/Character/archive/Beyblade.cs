using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Beyblade : NetworkBehaviour
{
    #region MonoBehavior
    const float TimeBeforeDampingStart = 0.5f;

    //[SerializeField] Transform spriteRoot;
    [SerializeField] TextMesh text;

    TopdownShooterNetworkManager networkManager;

    float moveSpeed = 25f;
    float rotationSpeed = 500f;
    Rigidbody2D rb;

    //Status
    [SyncVar] int score;
    bool isMoving;
    bool doDamp;
    float curMoveDur;
    float dampAmount = .97f;

    public Vector2 Velocity => rb.velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        networkManager = TopdownShooterNetworkManager.Instance;
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
            CmdIncrementScore();

    }
    #endregion

    #region Collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        HitsGameObject(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitsGameObject(collision.gameObject);
    }

    void HitsGameObject(GameObject go)
    {
        if (go.tag == "Dot")
        {
            Debug.Log("hits  Dot");
            CmdIncrementScore();

            if (DotSpawner.Instance != null)
                DotSpawner.Instance.EatDot();
            else
            {
                Debug.LogError("shouldn't happen");
            }

            //networkManager.SpawnDot();

            //if (DotSpawner.Instance != null  && NetworkServer.active)
            //    DotSpawner.Instance.SpawnDot();
        }
    }

    //[Command]
    //void DestroyDot (GameObject go)
    //{
    //    Destroy(go);
    //}
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
        rb.velocity = ArrowUpDir() * moveSpeed;
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
            if (curMoveDur > TimeBeforeDampingStart)
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
    void CmdIncrementScore()
    {
        RpcIncrementScore();
    }

    [ClientRpc]
    void RpcIncrementScore()
    {
        score++;
        text.text = score.ToString();
    }
    #endregion

    #region Minor
    Vector3 ArrowUpDir() => transform.up;
    #endregion
}