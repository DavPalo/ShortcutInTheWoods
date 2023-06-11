using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    PlayerController player = null;
    bool active = false;
    public Canvas shop;
    public VehicleController Buggy;

    // Start is called before the first frame update
    void Start()
    {
        Buggy = GameObject.FindGameObjectWithTag("Vehicle").GetComponent<VehicleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player) {
            if (player.isDriving) {
                if (Input.GetKeyDown(KeyCode.Q)) {
                    active = !active;
                    shop.gameObject.SetActive(active);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player") {
            player = collision.collider.GetComponent<PlayerController>();
        }
    }
}
