using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Beyblade : NetworkBehaviour
{
    #region MonoBehavior
    const float TimeBeforeDampingStart = 0.5f;

    [SerializeField] Transform spriteRoot;
    [SerializeField] TextMesh text;

    float moveSpeed = 25f;
    float rotationSpeed = 2f;
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
    }

    //[Client]
    void Update()
    {
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
            IncrementScore();
            Destroy(go);
            DotSpawner.Spawn();
        }
    }
    #endregion

    #region Rotating and waiting to shoot
    void RotateArrow()
    {
        spriteRoot.Rotate(new Vector3(0f, 0f, rotationSpeed));
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
        spriteRoot.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
    #endregion

    #region Score
    [Command]
    void IncrementScore()
    {
        score++;
        text.text = score.ToString();
    }
    #endregion

    #region Minor
    Vector3 ArrowUpDir() => spriteRoot.transform.up;
    #endregion
}