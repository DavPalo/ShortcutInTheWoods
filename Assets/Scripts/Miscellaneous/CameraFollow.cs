using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    public float cameraDistance = -12f;
    public float zoom = 8f;
    private Transform vehicle;
    bool zoomActivated = false;

    private void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle").transform;
    }

    private void Update()
    {
        if (vehicle)
            transform.position = vehicle.transform.position + new Vector3(0, 0, cameraDistance);
    }

    public void ZoomOut() {
        if (zoomActivated){
            cameraDistance = cameraDistance + zoom;
        }
        else {
            cameraDistance = cameraDistance - zoom;
        }
        zoomActivated = !zoomActivated;
    }  
}
