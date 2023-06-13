using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    public Transform vehicle;
    public float cameraDistance;

    // Update is called once per frame
    void Update()
    {
        vehicle = GameObject.Find("Vehicle").transform;
        if(vehicle != null)
            transform.position = vehicle.transform.position + new Vector3(0, 0, cameraDistance);
    }
}
