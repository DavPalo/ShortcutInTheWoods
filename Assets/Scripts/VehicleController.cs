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

    public NetworkVariable<bool> someoneIsDriving;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        someoneIsDriving.Value = false;
    }

    private void Update()
    {
        if(IsOwner)
            Drive();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Drive()
    {
        if (someoneIsDriving.Value)
        {
            movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementVector.Normalize();

            if (movementVector.y > 0)
            {
                currentForwardDirection = 1;
            }
            else if (movementVector.y < 0)
            {
                currentForwardDirection = 0;
            }

            if (Mathf.Abs(movementVector.y) != 0)
                currentSpeed += acceleration * Time.deltaTime;
            else
                currentSpeed -= 2 * acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= 2 * acceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    public void Move()
    {
        rb2d.velocity = (Vector2)transform.right * currentForwardDirection * currentSpeed * Time.fixedDeltaTime;

        if (someoneIsDriving.Value)
            rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeSomeoneIsDrivingServerRpc(bool boolean)
    {
        someoneIsDriving.Value = boolean;
    }
}
