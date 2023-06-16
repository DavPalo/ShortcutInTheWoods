using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : NetworkBehaviour
{
    public GameObject shooter;
    private Collider2D coll;
    public int damage;

    private void Start()
    {
        if(shooter != null)
        {
            coll = GetComponent<Collider2D>();
            Collider2D shooterCollider = shooter.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(coll, shooterCollider, true);
        }
    }

    private void Update()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>(), true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       DestroyClientRpc();
    }

    [ClientRpc]
    public void DestroyClientRpc()
    {
        Destroy(gameObject);
    }
}
