using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class VehicleController : NetworkBehaviour
{
    public Rigidbody2D body;
    private float moveLimiter = 0.7f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

}
