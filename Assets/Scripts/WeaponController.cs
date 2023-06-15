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

    public bool stop = false;

    public LevelManager levelManager;

    private void Start()
    {
        mainCamera = Camera.main;
        someoneIsShooting = false;
        canShoot = true;
        vehicle = transform.parent;
        baseRotation = transform.localRotation;
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

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
        if (stop)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, baseRotation, rotationStep);
        else
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
        bullet.GetComponent<Bullet>().damage = levelManager.damage.Value;
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
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        stop = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stop = false;
    }
}

