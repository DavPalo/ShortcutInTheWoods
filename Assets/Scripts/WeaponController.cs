using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponController : NetworkBehaviour
{
    Camera mainCamera;
    public float rotationSpeed;
    public Quaternion baseRotation;

    public bool someoneIsShooting;
    public Transform vehicle;

    public GameObject bulletPrefab;
    public float bulletForce;
    public Transform firePoint;
    public bool canShoot;
    public float shootDelay;
    public int bulletDamage;

    private void Start()
    {
        mainCamera = Camera.main;
        someoneIsShooting = false;
        canShoot = true;
        vehicle = transform.parent;
        baseRotation = transform.localRotation;
    }

    private void Update()
    {
        if (someoneIsShooting)
        {
            Aim();

            if (Input.GetButton("Fire1") && canShoot)
            {
                Shoot();
            }
        }
    }

    public void Move(float currentForwardDirection, float currentSpeed, Vector2 movementVector, float vehicleRotationSpeed)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = (Vector2)transform.right * currentForwardDirection * currentSpeed * Time.fixedDeltaTime;
        rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * vehicleRotationSpeed * Time.fixedDeltaTime));
    }

    public void Aim()
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;

        float angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;

        float rotationStep = rotationSpeed * Time.deltaTime;

        AimServerRpc(angle, rotationStep);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AimServerRpc(float angle, float rotationStep)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationStep);
    }

    public void Shoot()
    {
        canShoot = false;
        StartCoroutine(ShootCoroutine());
        ShootServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bullet.GetComponent<Bullet>().shooter = gameObject;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeSomeoneIsShootingServerRpc(bool boolean)
    {
        someoneIsShooting = boolean;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, baseRotation, rotationSpeed * Time.deltaTime);
    }
}

