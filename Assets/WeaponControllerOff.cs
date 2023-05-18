using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerOff : MonoBehaviour
{
    Camera mainCamera;
    public float rotationSpeed;
    public float baseRotation;

    public bool someoneIsShooting;
    public Transform vehicle;

    public GameObject bullet;
    public float bulletForce;
    public Transform firePoint;
    public bool canShoot;
    public float delayInSeconds;

    private void Start()
    {
        mainCamera = Camera.main;
        someoneIsShooting = false;
        vehicle = transform.parent;
        canShoot = true;
    }

    private void Update()
    {
        if (someoneIsShooting)
        {
            var mouseScreenPos = Input.mousePosition;
            var startingScreenPos = mainCamera.WorldToScreenPoint(transform.position);
            mouseScreenPos.x -= startingScreenPos.x;
            mouseScreenPos.y -= startingScreenPos.y;

            var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
            //angle = Mathf.Clamp(angle, baseRotation - 60f, baseRotation + 60f);

            var rotationStep = rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationStep);
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Input.GetButton("Fire1") && canShoot)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(this.bullet, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        canShoot = false;
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }

}

