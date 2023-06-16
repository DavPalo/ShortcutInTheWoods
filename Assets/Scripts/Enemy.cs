using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    private GameObject vehicle;
    private Rigidbody2D rb2d;
    public GameObject bullet;

    public int health;
    public int attack;
    public int bulletDamage;
    public float distanceToEngage;

    public float bulletForce;
    public bool canShoot;
    public float delayInSeconds;

    public float minimumDesiredDistance;
    public float maximumDesiredDistance;
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        rb2d = GetComponent<Rigidbody2D>();
        canShoot = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(IsServer)
        {
            if(health <= 0)
            {
                NetworkObject.Despawn();
            }

            if(canShoot)
                Shoot();
        }
    }

    private void FixedUpdate()
    {
        //Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    public void Move()
    {
        /*
        // ROTATION
        Vector3 point = vehicle.transform.position;
        Vector3 axis = new Vector3(0, 0, 1);
        transform.RotateAround(point, axis, Time.deltaTime * 10);
        */

        // MOVEMENT
        float distance = (vehicle.transform.position - transform.position).magnitude;
        Vector2 direction = vehicle.transform.position - transform.position;
        if (distance > maximumDesiredDistance)
        {
            rb2d.velocity = direction * speed * Time.fixedDeltaTime;
        }
        else if(distance < minimumDesiredDistance)
        {
            rb2d.velocity = -direction * speed * Time.fixedDeltaTime;
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Shoot()
    {
        if((vehicle.transform.position - transform.position).magnitude < distanceToEngage)
        {
            GameObject bullet = Instantiate(this.bullet, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().shooter = gameObject;
            bullet.GetComponent<SpriteRenderer>().color = Color.blue;
            bullet.GetComponent<Bullet>().damage = bulletDamage;
            bullet.GetComponent<NetworkObject>().Spawn(true);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = vehicle.transform.position - transform.position;

            rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            canShoot = false;
            StartCoroutine(ShootCoroutine());

        }
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
