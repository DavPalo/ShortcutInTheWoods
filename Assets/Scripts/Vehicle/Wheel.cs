using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public PlayerController player = null;
    bool active = false;
    public Canvas shop;

    // Update is called once per frame
    void Update()
    {
        if (player) {
            if (player.isDriving) {
                if (Input.GetKeyDown(KeyCode.Q)) {
                    player.GetComponent<PlayerController>().isShopping = !player.GetComponent<PlayerController>().isShopping;
                    active = !active;
                    shop.gameObject.SetActive(active);
                }
            }
        }
    }
}
