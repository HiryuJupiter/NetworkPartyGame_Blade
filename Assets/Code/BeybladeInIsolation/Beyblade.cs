using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Beyblade : MonoBehaviour
{
    #region MonoBehavior
    [SerializeField] Transform spriteRoot;
    [SerializeField] TextMesh text;

    float moveSpeed = 25f;
    float rotationSpeed = 2f;
    Rigidbody2D rb;

    //Status
    int score;
    bool isMoving;
    bool doDamp;
    float curMoveDur;
    float timeBeforeDampingStart = 0.1f;
    float dampAmount = .99f;

    public Vector2 Velocity => rb.velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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

    void HitsGameObject (GameObject go)
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

    void ShootBallWhenPressed()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) //Shoot
        {
            BeginMoving();
        }
    }

    #endregion

    #region Moving
    void BeginMoving ()
    {
        rb.velocity = ArrowUpDir() * moveSpeed;
        doDamp = false;
        isMoving = true;
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
            if (curMoveDur > timeBeforeDampingStart)
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
    void IncrementScore ()
    {
        score++;
        text.text = score.ToString();
    }
    #endregion

    #region Minor
    Vector3 ArrowUpDir() => spriteRoot.transform.up;
    #endregion
}