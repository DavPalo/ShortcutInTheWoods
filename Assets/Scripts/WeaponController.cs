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
    public float baseRotation;

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
        baseRotation = transform.localRotation.eulerAngles.z;
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

        float vehicleAngle = vehicle.transform.rotation.eulerAngles.z;

        float max = vehicleAngle + baseRotation + 60f, min = vehicleAngle + baseRotation - 60f;

        Debug.Log("Pre ----min: " + min + "\nmax: " + max);

        if (max >= 180f)
            max = (max % 180) - 180f;
        
        if (min <= -180f)
            min = (min % 180) + 180f;

        if (max < min)
        {
            float temp = min;
            min = max;
            max = temp;

        }

        Debug.Log("min: " + min + "\nmax: " + max);
        angle = Mathf.Clamp(angle, min, max);


        Debug.Log("Angle " + angle);

        float rotationStep = rotationSpeed * Time.deltaTime;

        AimServerRpc(angle, rotationStep);
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360.0)
            angle += 360.0f;
        if (angle > 360.0)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
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

    static public float ModularClamp(float val, float min, float max, float rangemin = -180f, float rangemax = 180f)
    {
        var modulus = Mathf.Abs(rangemax - rangemin);
        if ((val %= modulus) < 0f) val += modulus;
        return Mathf.Clamp(val + Mathf.Min(rangemin, rangemax), min, max);
    }
}

