using System.Collections;
using UnityEngine;
using Mirror;

public class MoveShip : NetworkBehaviour
{
    [SerializeField]
    float speed;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            float movement = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(movement * speed, 0f);
        }
    }
}