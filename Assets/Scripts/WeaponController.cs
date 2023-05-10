using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponController : NetworkBehaviour
{
    Camera mainCamera;
    public float rotationSpeed;
    public float minRotation;
    public float maxRotation;

    public bool someoneIsShooting;
    public Transform vehicle;

    private void Start()
    {
        mainCamera = Camera.main;
        someoneIsShooting = false;
        vehicle = transform.parent;
    }

    private void Update()
    {
        if(someoneIsShooting)
        {
            var mouseScreenPos = Input.mousePosition;
            var startingScreenPos = mainCamera.WorldToScreenPoint(transform.position);
            mouseScreenPos.x -= startingScreenPos.x;
            mouseScreenPos.y -= startingScreenPos.y;
        
            var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -30.0f + vehicle.transform.rotation.z, 120.0f + vehicle.transform.rotation.z);
        
            var rotationStep = rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationStep);
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }

}
