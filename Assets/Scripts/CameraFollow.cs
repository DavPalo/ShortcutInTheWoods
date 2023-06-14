using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    public float cameraDistance;
    private Transform vehicle;

    private void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = vehicle.transform.position + new Vector3(0, 0, cameraDistance);
    }
}
