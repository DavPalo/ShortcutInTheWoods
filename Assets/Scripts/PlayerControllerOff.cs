using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerControllerOff : NetworkBehaviour
{
    private Rigidbody2D body;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;
    public float runSpeed;

    public VehicleControllerOff vehicle;
    public GameObject wheel;
    public GameObject[] weapons;
    public int weaponIndex;
    public GameObject shieldInteract;
    public GameObject shield;

    private bool isDriving = false;
    private bool isShooting = false;
    public GameObject interact;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        vehicle = GameObject.Find("Vehicle").GetComponent<VehicleControllerOff>();
        wheel = GameObject.Find("Wheel");
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        shieldInteract = GameObject.Find("Shield Interact");
        shield = GameObject.Find("Shield");
        transform.parent = vehicle.transform;
        interact = this.gameObject.transform.GetChild(0).gameObject;
        interact.SetActive(false);
    }

    void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && isDriving)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            isDriving = false;
            vehicle.someoneIsDriving = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isShooting)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            isShooting = false;
            weapons[weaponIndex].GetComponent<WeaponControllerOff>().someoneIsShooting = false;
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

    private void Interact()
    {
        float distanceToWheel = (transform.position - wheel.transform.position).magnitude;

        float[] distanceToWeapons = new float[4];
        for(int i = 0; i < weapons.Length; i++)
        {
            distanceToWeapons[i] = (transform.position - weapons[i].transform.position).magnitude;
        }

        float minimumWeaponDistance = Mathf.Min(distanceToWeapons);
        weaponIndex = Array.IndexOf(distanceToWeapons, minimumWeaponDistance);

        float distanceToShield = (transform.position - shieldInteract.transform.position).magnitude;

        if (distanceToWheel < 0.2 && isDriving == false)
        {
            interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                body.bodyType = RigidbodyType2D.Kinematic;
                body.constraints = RigidbodyConstraints2D.FreezePosition;
                isDriving = true;
                vehicle.someoneIsDriving = true;
                ChangeVehicleOwnerServerRpc();
            }
        }
        else if(minimumWeaponDistance < 0.2 && isShooting == false)
        {
            interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                body.bodyType = RigidbodyType2D.Kinematic;
                body.constraints = RigidbodyConstraints2D.FreezePosition;
                isShooting = true;
                weapons[weaponIndex].GetComponent<WeaponControllerOff>().someoneIsShooting = true;
                ChangeWeaponOwnerServerRpc();
            }
        }
        else if (distanceToShield < 0.2)
        {
            interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && shield.GetComponent<Shield>().activable)
            {
                shield.GetComponent<Shield>().activateShieldServerRpc();
            }
        }
        else
        {
            interact.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeVehicleOwnerServerRpc()
    {
        vehicle.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeWeaponOwnerServerRpc()
    {
        weapons[weaponIndex].GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

}

