using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyPlayerController : NetworkBehaviour
{
    [SerializeField]
    float speed;

    private void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            float movement = Input.GetAxis("Horizontal");
            GetComponent<Rigidbody2D>().velocity = new Vector2(movement * speed, 0f);
        }
    }
}
