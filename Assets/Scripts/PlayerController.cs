using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb2d;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;
    public float runSpeed;

    public VehicleController vehicle;
    public GameObject wheel;
    public GameObject[] weapons;
    public int weaponIndex;
    public GameObject shieldInteract;
    public GameObject shield;

    public bool isDriving = false;
    private bool isShooting = false;
    public GameObject interact;
    public float distanceToInteract;

    public bool shop;
    public LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rb2d = GetComponent<Rigidbody2D>();
        vehicle = GameObject.Find("Vehicle").GetComponent<VehicleController>();
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
            rb2d.simulated = true;
            isDriving = false;
            GetComponent<Collider2D>().enabled = true;
            vehicle.changeSomeoneIsDrivingServerRpc(false);
            RemoveVehicleOwnerServerRpc();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isShooting)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            isShooting = false;
            weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting = false;
        }

        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down


        if (!isDriving && !isShooting)
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            rb2d.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
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

        if (distanceToWheel < distanceToInteract && isDriving == false && !vehicle.someoneIsDriving.Value)
        {
            interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                wheel.GetComponent<Wheel>().player = this;
                rb2d.simulated = false;
                GetComponent<Collider2D>().enabled = false;
                isDriving = true;
                vehicle.changeSomeoneIsDrivingServerRpc(true);
                ChangeVehicleOwnerServerRpc(OwnerClientId);
            }
        }
        else if(minimumWeaponDistance < distanceToInteract && !isShooting && !weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting)
        {
            interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
                isShooting = true;
                weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting = true;

            }
        }
        else if (distanceToShield < distanceToInteract)
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
    public void ChangeVehicleOwnerServerRpc(ulong playerId)
    {
        vehicle.GetComponent<NetworkObject>().ChangeOwnership(playerId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveVehicleOwnerServerRpc()
    {
        vehicle.GetComponent<NetworkObject>().RemoveOwnership();
    }

}



