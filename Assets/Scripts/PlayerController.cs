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

    public NetworkVariable<bool> isDriving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool isShooting = false;
    private Collider2D actualCollider = null;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        vehicle = GameObject.Find("Vehicle").GetComponent<VehicleController>();
        this.transform.parent = vehicle.transform;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space) && isDriving.Value)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.None;
            StartCoroutine(IgnoreCollision());
            isDriving.Value = false;
            vehicle.enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isShooting)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.None;
            StartCoroutine(IgnoreCollision());
            isShooting = false;
        }

        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (!isDriving.Value)
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
        else
        {
            InputServerRpc();
        }
    }

    IEnumerator IgnoreCollision()
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), actualCollider, true);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), actualCollider, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wheel")
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            isDriving.Value = true;
            actualCollider = collision.collider;
            vehicle.enabled = true;
        }

        if(collision.collider.tag == "Weapon")
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            isShooting = true;
            actualCollider = collision.collider;
        }
    }

    [ServerRpc]
    void InputServerRpc()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        vehicle.body.velocity = new Vector2(horizontal * vehicle.speed, vertical * vehicle.speed);
    }
}
