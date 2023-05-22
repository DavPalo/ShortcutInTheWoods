using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D body;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;
    public float runSpeed;

    public VehicleController vehicle;
    public GameObject wheel;

    private bool isDriving = false;
    private bool isShooting = false;
    public Collider2D actualCollider = null;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        vehicle = GameObject.Find("Vehicle").GetComponent<VehicleController>();
        wheel = GameObject.Find("Wheel");
        transform.parent = vehicle.transform;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space) && isDriving)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(IgnoreCollision());
            isDriving = false;
            vehicle.someoneIsDriving = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isShooting)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(IgnoreCollision());
            isShooting = false;
            actualCollider.gameObject.GetComponent<WeaponController>().someoneIsShooting = false;
        }

        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        

        if (!isDriving)
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }

        Interact();
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeOwnerServerRpc()
    {
        vehicle.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeWeaponOwnerServerRpc()
    {
        Debug.Log("Actual collider -> " + actualCollider.gameObject.ToString());
        //actualCollider.gameObject.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    IEnumerator IgnoreCollision()
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), actualCollider, true);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), actualCollider, false);
        actualCollider = null;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wheel")
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            isDriving = true;
            vehicle.someoneIsDriving = true;
            actualCollider = collision.GetContact(0).collider;
            ChangeOwnerServerRpc();
        }

        if(collision.collider.tag == "Weapon")
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            isShooting = true;
            actualCollider = collision.GetContact(0).collider;
            actualCollider.gameObject.GetComponent<WeaponController>().someoneIsShooting = true;
            //ChangeWeaponOwnerServerRpc();
        }
    }*/

    private void Interact()
    {
        float distance = (transform.position - wheel.transform.position).magnitude;
        Debug.Log(distance);
        if (distance < 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                body.bodyType = RigidbodyType2D.Kinematic;
                body.constraints = RigidbodyConstraints2D.FreezePosition;
                isDriving = true;
                vehicle.someoneIsDriving = true;
                ChangeOwnerServerRpc();
            }
        }
    }

}
