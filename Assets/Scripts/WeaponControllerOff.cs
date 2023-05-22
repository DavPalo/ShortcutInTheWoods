using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponControllerOff : NetworkBehaviour
{
    Camera mainCamera;
    public float rotationSpeed;
    public float baseRotation;

    public bool someoneIsShooting;
    public Transform vehicle;

    public GameObject bulletPrefab;
    public float bulletForce;
    public Transform firePoint;
    public NetworkVariable<bool> canShoot;
    public float shootDelay;

    private void Start()
    {
        mainCamera = Camera.main;
        someoneIsShooting = false;
        vehicle = transform.parent;
        canShoot.Value = true;
    }

    private void Update()
    {
        if (someoneIsShooting)
        {
            Aim();

            if (Input.GetButton("Fire1") && canShoot.Value)
            {
                ShootServerRpc();
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
        //angle = Mathf.Clamp(angle, baseRotation - 60f, baseRotation + 60f);

        float rotationStep = rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationStep);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        AimClientRpc(angle, rotationStep);
    }

    [ClientRpc]
    public void AimClientRpc(float angle, float rotationStep)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationStep);
    }


    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bullet.GetComponent<Bullet>().shooter = gameObject;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        canShoot.Value = false;
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot.Value = true;
    }

}

