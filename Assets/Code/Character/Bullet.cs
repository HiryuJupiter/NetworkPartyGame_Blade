using System.Collections;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SyncVar] int playerIndex = -1;

    public int PlayerIndex 
    {
        get => playerIndex;
        set { playerIndex = value; }
    }

    [Command]
    public void Shoot (Vector3 dir, int playerIndex)
    {
        this.playerIndex = playerIndex;
        GetComponent<Rigidbody2D>().velocity = dir * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
