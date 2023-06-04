using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy2 : NetworkBehaviour
{
    public GameObject vehicle;
    public Rigidbody2D rb2d;

    public float health;
    public int attack;

    public float speed;
    public float range;

    void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    public void FixedUpdate()
    {
        //get the distance between the player and enemy (this object)
        float dist = (vehicle.transform.position - transform.position).magnitude;
        Vector2 direction = vehicle.transform.position - transform.position;
        //check if it is within the range you set
        if (dist <= range)
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
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
