using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FakeTrees : NetworkBehaviour
{
    public int health = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
        {
            SetActiveClientRpc(false);
        }
    }

    [ClientRpc]
    private void SetActiveClientRpc(bool ean) {
        gameObject.SetActive(ean);
    }
}
