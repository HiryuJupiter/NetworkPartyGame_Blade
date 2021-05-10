using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootBullet : NetworkBehaviour
{
    [SerializeField] GameObject pf;

    void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot ()
    {
        GameObject obj = Instantiate(pf, transform.position + Vector3.up, Quaternion.identity);
        //obj.GetComponent<Rigidbody2D>().velocity = Vector2.up * 5f;
        NetworkServer.Spawn(obj);
        Destroy(obj, 3f);
    }
}
