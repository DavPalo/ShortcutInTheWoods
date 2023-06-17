using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy2 : NetworkBehaviour
{
    private GameObject vehicle;
    private Rigidbody2D rb2d;

    public float health;
    public int attack;

    public float speed;
    public float distanceToEngage;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            if(IsServer)
                NetworkObject.Despawn();
        }
    }

    private void FixedUpdate()
    {
        //get the distance between the player and enemy (this object)
        float distance = (vehicle.transform.position - transform.position).magnitude;
        Vector2 direction = vehicle.transform.position - transform.position;
        //check if it is within the range you set
        if (distance <= distanceToEngage)
        {
            //move to target(player) 
            rb2d.velocity = direction * speed * Time.fixedDeltaTime;
        }
        //else, if it is not in rage, it will not follow player
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
        if(collision.gameObject.tag == "Vehicle")
        {
            NetworkObject.Despawn();
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
    }
}
