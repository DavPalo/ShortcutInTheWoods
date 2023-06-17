using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TreasureDmg : NetworkBehaviour
{
    public int health = 10;
    public int minValue;
    public int maxValue;
    private int value;
    private LevelManager levelManager;
    public GameObject message;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        value = Random.Range(minValue, maxValue);
    }

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
            levelManager.ChangeTxtServerRpc("+" + value + "dmg");
            levelManager.IncreaseDmgServerRpc(value);

            if (IsServer)
                NetworkObject.Despawn();
        }
    }
}
