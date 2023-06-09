using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Treasure : NetworkBehaviour
{
    public int health;
    public int value;
    public LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        value = Random.Range(30, 60);
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

    private void Update()
    {
        if(health <= 0)
        {
            levelManager.updateGoosServerRpc(value);
            if(IsServer)
                NetworkObject.Despawn();
        }
    }
}
