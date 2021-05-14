using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReceiveDamage : NetworkBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SyncVar] int currentHealth = 10; //SyncVar means it must be synchronizd among instances
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] bool DestroyOnDeath;

    Vector2 initialPosition;



    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == enemyTag)
        {
            TakeDamage(1);
            Destroy(collider.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        if(isServer)
        {
            currentHealth -= amount;

            if(currentHealth <= 0)
            {
                if(DestroyOnDeath)
                {
                    Destroy(gameObject);
                }
                else
                {
                    currentHealth = maxHealth;
                    RpcRespawn();
                }
            }
        }
    }

    //[Command] send from clients to the server
    [ClientRpc] // ClientRpc is executed in the client, even if it gets called in server
    private void RpcRespawn()
    {
        transform.position = initialPosition;
    }
}
