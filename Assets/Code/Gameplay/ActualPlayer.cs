using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ActualPlayer : NetworkBehaviour
{
    #region MonoBehavior
    private bool isEnabled = false;
    public void Enable() => isEnabled = true;

    //Movement
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 500f;

    //Text
    [SerializeField] TextMesh text;

    //Ref
    Rigidbody2D rb;

    //Status
    [SyncVar]
    string displayName = "Loading...";
    [SyncVar]
    int score;
    bool isMoving;
    bool doDamp;
    float curMoveDur;
    float dampAmount = .97f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        //Don't let other players control your player
        //if (!isLocalPlayer)
        //{
        //    Debug.Log("no");
        //    return;
        //}

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
            if (isLocalPlayer)
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
        //Debug.Log("hi");
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