using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Boss : NetworkBehaviour
{
    private GameObject vehicle;
    private Rigidbody2D rb2d;
    public GameObject bulletPrefab;

    public int health;
    public int attack;
    public int bulletDamage;
    public int speed;
    public float distanceToEngage;
    public float minDistance;

    public float bulletForce;
    private bool canShoot;
    public float delayInSeconds;

    public Transform firePoint;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        rb2d = GetComponent<Rigidbody2D>();
        canShoot = true;
    }

    private void Update()
    {
        if(IsServer)
        {
            if (health <= 0)
            {
                NetworkObject.Despawn();
            }

            if (canShoot)
                Shoot();

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

    public void Shoot()
    {
        if ((vehicle.transform.position - transform.position).magnitude < distanceToEngage && (vehicle.transform.position - transform.position).magnitude > minDistance)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, vehicle.transform.position, Time.deltaTime * speed);
            rb2d.MovePosition(newPosition);

            GameObject bullet = Instantiate(this.bulletPrefab, firePoint.position, transform.rotation);
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

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
