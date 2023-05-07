using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VehicleController : NetworkBehaviour
{ 
    public Rigidbody2D rb2d;
    public float maxSpeed;
    public float rotationSpeed;
    public Vector2 movementVector;

    public float acceleration;
    public float currentSpeed;
    public float currentForwardDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        this.enabled = false;
    }

    private void Update()
    {
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVector.Normalize();

        if(movementVector.y > 0)
        {
            currentForwardDirection = 1;
        }
        else if(movementVector.y < 0)
        {
            currentForwardDirection = 0;
        }

        if (Mathf.Abs(movementVector.y) != 0)
            currentSpeed += acceleration * Time.deltaTime;
        else
            currentSpeed -= 2 * acceleration * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    private void FixedUpdate()
    {
        rb2d.velocity = (Vector2)transform.right * currentForwardDirection * currentSpeed * Time.fixedDeltaTime;
        rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x* rotationSpeed * Time.fixedDeltaTime));
    }
}
