using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOff : MonoBehaviour
{
    public GameObject vehicle;

    public float health;
    public float attack;

    public GameObject bullet;
    public float bulletForce;
    public bool canShoot;
    public float delayInSeconds;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = GameObject.Find("Vehicle");
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        if(canShoot)
            Shoot();
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

    public void Shoot()
    {
        GameObject bullet = Instantiate(this.bullet, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = vehicle.transform.position - transform.position;

        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
        canShoot = false;
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
