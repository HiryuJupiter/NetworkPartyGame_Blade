using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BeybladeGraphics))]
[RequireComponent(typeof(BeybladePhysics))]
public class Beyblade : MonoBehaviour
{

    BeybladePhysics physics;
    BeybladeGraphics graphics;


    private void Awake()
    {
        physics = GetComponent<BeybladePhysics>();
        graphics = GetComponent<BeybladeGraphics>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void Shoot ()
    {

    }
}
