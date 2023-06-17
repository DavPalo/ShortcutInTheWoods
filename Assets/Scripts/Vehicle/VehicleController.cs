using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VehicleController : NetworkBehaviour
{
    private Rigidbody2D rb2d;
    public float maxSpeed;
    public float rotationSpeed;
    public Vector2 movementVector;

    public float acceleration;
    public float currentSpeed;
    public float currentForwardDirection;

    public NetworkVariable<bool> someoneIsDriving = new NetworkVariable<bool>(false);
    public NetworkObject driver;


    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (driver)
        {
            Drive();
        }
 
    }

    public void Drive()
    {
        if (someoneIsDriving.Value && driver.IsOwner)
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
            currentSpeed -= 2* acceleration * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        DriveServerRpc(currentForwardDirection, currentSpeed, movementVector, rotationSpeed);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DriveServerRpc(float currentForwardDirection, float currentSpeed, Vector2 movementVector, float rotationSpeed)
    {

        rb2d.velocity = (Vector2)transform.right * currentForwardDirection * currentSpeed * Time.fixedDeltaTime;
        rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));

        //For moving Host while Client is Driving
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().IsOwnedByServer)
            {
                player.GetComponent<Rigidbody2D>().velocity = (Vector2)transform.right * currentForwardDirection *
                        currentSpeed * Time.fixedDeltaTime + player.GetComponent<PlayerController>().velocity;
                player.GetComponent<Rigidbody2D>().MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));
            }
        }
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
