 using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb2d;

    public NetworkVariable<float> horizontal = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float>  vertical = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private float moveLimiter = 0.7f;
    public float runSpeed;
    public Vector2 velocity;

    private VehicleController vehicle;
    private GameObject wheel;
    private GameObject[] weapons;
    private int weaponIndex;
    private GameObject shieldInteract;
    private GameObject shield;
    private GameObject binoculars;

    public bool isDriving = false;
    public bool isShooting = false;
    public bool isLooking = false;
    public bool isShopping = false;

    private GameObject interact;
    public float distanceToInteract;

    public Sprite[] sprites;
    public RuntimeAnimatorController[] controllers;

    public Animator animator;
    
    private LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rb2d = GetComponent<Rigidbody2D>();
        vehicle = GameObject.FindGameObjectWithTag("Vehicle").GetComponent<VehicleController>();
        NetworkObject.TrySetParent(vehicle.gameObject, false);
        GetComponent<NetworkObject>().AutoObjectParentSync = true;

        wheel = GameObject.Find("Wheel");
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        shieldInteract = GameObject.Find("Shield Interact");
        binoculars = GameObject.Find("Binoculars");
        shield = GameObject.Find("Shield");

        interact = this.gameObject.transform.GetChild(0).gameObject;
        interact.SetActive(false);
        animator = GetComponent<Animator>();

        if (NetworkObjectId == 1)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
            GetComponent<Animator>().runtimeAnimatorController = controllers[0];
        }
        else {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            GetComponent<Animator>().runtimeAnimatorController = controllers[1];
        }

    }

    void Update()
    {
        if (!IsOwner)
            return;

        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        if ((Input.GetKeyDown(KeyCode.E)) && isDriving && !isShopping)
        {
            rb2d.simulated = true;
            isDriving = false;
            ChangeServerRpc();
        }

        else if ((Input.GetKeyDown(KeyCode.E)) && isShooting)
        {
            rb2d.simulated = true;
            isShooting = false;
            weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting = false;
        }

        else if ((Input.GetKeyDown(KeyCode.E)) && isLooking)
        {
            rb2d.simulated = true;
            Camera.main.GetComponent<CameraFollow>().ZoomIn();
            isLooking = false;
        }

        // Gives a value between -1 and 1
        horizontal.Value = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical.Value = Input.GetAxisRaw("Vertical"); // -1 is down

        if (!isDriving && !isShooting && !isLooking)
        {
            if (horizontal.Value != 0 || vertical.Value != 0)
            {
                animator.SetFloat("Speed", 1);
                if(horizontal.Value < 0)
                    animator.SetBool("Left", true);
                else
                    animator.SetBool("Left", false);

            }
            else 
            {
                animator.SetFloat("Speed", 0);

            }


            /*
            if (horizontal.Value != 0 && vertical.Value != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal.Value *= moveLimiter;
                vertical.Value *= moveLimiter;

            }
            */

            velocity = new Vector2(horizontal.Value * runSpeed, vertical.Value * runSpeed);

            rb2d.velocity = velocity;
        }

        Interact();
    }

    private void Interact()
    {
        float distanceToWheel = (transform.position - wheel.transform.position).magnitude;

        float[] distanceToWeapons = new float[4];
        for (int i = 0; i < weapons.Length; i++)
        {
            distanceToWeapons[i] = (transform.position - weapons[i].transform.position).magnitude;
        }

        float minimumWeaponDistance = Mathf.Min(distanceToWeapons);
        weaponIndex = Array.IndexOf(distanceToWeapons, minimumWeaponDistance);

        float distanceToShield = (transform.position - shieldInteract.transform.position).magnitude;

        float distanceToBinoculars = (transform.position - binoculars.transform.position).magnitude;

        if (distanceToWheel < distanceToInteract && !isDriving && !vehicle.someoneIsDriving.Value)
        {
            interact.SetActive(true);
            interact.GetComponentInChildren<Text>().text = "E to Drive";

            if (Input.GetKeyDown(KeyCode.E) && !isDriving)
            {
                wheel.GetComponent<Wheel>().player = this;
                rb2d.simulated = false;
                isDriving = true;
                ChangeServerRpc();
            }
        } else if (isDriving)
        {
            interact.SetActive(true);
            interact.GetComponentInChildren<Text>().text = "Q to Repair";
        }
        else if(minimumWeaponDistance < distanceToInteract && !isShooting && !weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting)
        {
            interact.SetActive(true);
            interact.GetComponentInChildren<Text>().text = "E to Shoot";

            if (Input.GetKeyDown(KeyCode.E))
            {
                rb2d.simulated = false;
                isShooting = true;
                weapons[weaponIndex].GetComponent<WeaponController>().someoneIsShooting = true;
            }
        }
        else if (distanceToShield < distanceToInteract)
        {
            interact.SetActive(true);
            interact.GetComponentInChildren<Text>().text = "E to Shield";

            if (Input.GetKeyDown(KeyCode.E) && shield.GetComponent<Shield>().activable)
            {
                shield.GetComponent<Shield>().activateShieldServerRpc();
            }
        }
        else if (distanceToBinoculars < distanceToInteract)
        {
            interact.SetActive(true);
            interact.GetComponentInChildren<Text>().text = "E to Zoom Out";

            if (Input.GetKeyDown(KeyCode.E))
            {
                rb2d.simulated = false;
                isLooking = true;
                Camera.main.GetComponent<CameraFollow>().ZoomOut();
            }
        }
        else
        {
            interact.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeServerRpc() {
        vehicle.driverId.Value = NetworkObjectId;
        vehicle.someoneIsDriving.Value = !vehicle.someoneIsDriving.Value;

    }
}



