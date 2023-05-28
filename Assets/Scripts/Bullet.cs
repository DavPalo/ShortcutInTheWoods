using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public GameObject shooter;
    private Collider2D coll;

    private void Start()
    {
        if (shooter != LevelManager.vehicle)
        {
            coll = GetComponent<Collider2D>();
            Collider2D shooterCollider = shooter.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(coll, shooterCollider, true);
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