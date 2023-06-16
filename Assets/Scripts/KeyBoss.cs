using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KeyBoss : NetworkBehaviour
{
    // Start is called before the first frame update
    public int health = 1;
    public LevelManager levelManager;

    public GameObject message;
    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            message.GetComponent<Timer>().txt = "Boss Area Unlocked";
            message.GetComponent<Timer>().txtChange = true;

            levelManager.KeyGainedServerRpc();

            if (IsServer)
                NetworkObject.Despawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
