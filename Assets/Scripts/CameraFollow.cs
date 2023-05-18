using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform vehicle;

    // Update is called once per frame
    void Update()
    {
        transform.position = vehicle.transform.position + new Vector3(0, 0, -1.5f);
    }
}
