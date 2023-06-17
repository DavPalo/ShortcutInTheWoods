using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Treasure : NetworkBehaviour
{
    public int health;
    public int value;
    public int minValue;
    public int maxValue;
    public LevelManager levelManager;

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

            message.GetComponent<Timer>().txt = "+ "+ value + " goos";
            message.GetComponent<Timer>().txtChange = true;
            levelManager.updateGoosServerRpc(value);
            if(IsServer)
                NetworkObject.Despawn();
        }
    }
}
