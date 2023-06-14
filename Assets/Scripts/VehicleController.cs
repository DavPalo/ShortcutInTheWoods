using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public NetworkObject driver;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        someoneIsDriving.Value = false;
    }

    private void Update()
    {
        if(driver && driver.IsOwner)
        {
            Drive();
        }
    }

    public void Drive()
    {
        if (someoneIsDriving.Value)
        {
            Debug.Log("Dentro drive");
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

        Debug.Log("Pre Server");

        DriveServerRpc(currentForwardDirection, currentSpeed, movementVector, rotationSpeed);

        Debug.Log("Post Server");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DriveServerRpc(float currentForwardDirection, float currentSpeed, Vector2 movementVector, float rotationSpeed)
    {
        Debug.Log("Dentro Server");

        rb2d.velocity = (Vector2)transform.right * currentForwardDirection * currentSpeed * Time.fixedDeltaTime;

        Debug.Log(rb2d.velocity.magnitude);

        rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));

    }

    [ServerRpc(RequireOwnership = false)]
    public void changeSomeoneIsDrivingServerRpc(bool boolean, NetworkObjectReference networkDriver)
    {
        someoneIsDriving.Value = boolean;
        if (networkDriver.TryGet(out NetworkObject driver))
        {
            this.driver = driver;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeServerRpc(bool boolean)
    {
        someoneIsDriving.Value = boolean;
    }
}
