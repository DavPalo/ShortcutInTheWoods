using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public GameObject shooter;
    private Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        //Collider2D shooterCollider = shooter.GetComponent<Collider2D>();
        //Physics2D.IgnoreCollision(collider, shooterCollider, true);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //NetworkObject networkBullet = gameObject.GetComponent<NetworkObject>();
        DestroyClientRpc();
    }

    [ClientRpc]
    public void DestroyClientRpc()
    {
        Destroy(gameObject);
    }
}
