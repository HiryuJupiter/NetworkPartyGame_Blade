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

    //Body parts
    [SerializeField] TextMesh textScore;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] TextMesh playerNameText;

    //Ref
    Rigidbody2D rb;

    //Status
    [SyncVar]
    public int score;
    bool isMoving;
    bool doDamp;
    float curMoveDur;
    float dampAmount = .97f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //if (isLocalPlayer)
        //CmdSetPlayerProperties(BeybladeNetworkManager.PlayerName, BeybladeNetworkManager.PlayerColor);
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        if (!setProperties)
            StartGame();

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
        textScore.text = score.ToString();
    }
    #endregion

    #region Set player color and name
    bool setProperties;
    public void StartGame()
    {
        if (isLocalPlayer)
        {
            setProperties = true;
            CmdSetPlayerProperties(BeybladeNetworkManager.PlayerName, BeybladeNetworkManager.PlayerColor);
        }
    }

    [Command]
    void CmdSetPlayerProperties(string name, Color col)
    {
        //Debug.Log("CmdSetPlayerProperties " + name);
        RpcSetPlayerProperties(name, col);
    }

    [ClientRpc]
    void RpcSetPlayerProperties(string name, Color col)
    {
        //Debug.Log("   color " + col);
        bodySprite.color = col;
        playerNameText.text = name;
    }
    #endregion

}