using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Unity.Netcode;
using UnityEngine;

public class KeyBoss : NetworkBehaviour
{
    public int health = 1;
    private LevelManager levelManager;
    public GameObject message;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            levelManager.ChangeTxtServerRpc("Boss area Unlocked");

            levelManager.KeyGainedServerRpc();

            if (IsServer)
                NetworkObject.Despawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
