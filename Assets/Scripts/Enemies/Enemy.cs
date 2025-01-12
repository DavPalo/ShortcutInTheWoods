using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    private GameObject vehicle;
    private Rigidbody2D rb2d;
    public GameObject bulletPrefab;

    public int health;
    public int attack;
    public int bulletDamage;
    public float distanceToEngage;

    public float bulletForce;
    private bool canShoot;
    public float delayInSeconds;

    public float minimumDesiredDistance;
    public float maximumDesiredDistance;
    public float speed;

    public Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        rb2d = GetComponent<Rigidbody2D>();
        canShoot = true;
        animator = GetComponent<Animator>();
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

            if (vehicle.transform.position.x < transform.position.x)
            {
                animator.SetBool("Left", true);
            }
            else
                animator.SetBool("Left", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    private void Move()
    {

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

    private void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Shoot()
    {
        if((vehicle.transform.position - transform.position).magnitude < distanceToEngage)
        {
            if (IsServer) {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().shooter = gameObject;
                bullet.GetComponent<Bullet>().damage = bulletDamage;
                bullet.GetComponent<NetworkObject>().Spawn(true);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 direction = vehicle.transform.position - transform.position;
                rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
                canShoot = false;
                StartCoroutine(ShootCoroutine());

            }
        }
    }

    IEnumerator ShootCoroutine()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
