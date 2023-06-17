using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    public float cameraDistance = -12f;
    public float zoom = 8f;
    private Transform vehicle;

    private void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle").transform;
    }

    private void Update()
    {
        transform.position = vehicle.transform.position + new Vector3(0, 0, cameraDistance);
    }

    public void ZoomOut() {
        cameraDistance = cameraDistance - zoom;
    }

    public void ZoomIn()
    {
        cameraDistance = cameraDistance + zoom;
    }
}
