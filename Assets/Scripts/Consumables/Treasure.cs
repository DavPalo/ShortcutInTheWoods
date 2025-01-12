using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class Treasure : NetworkBehaviour
{
    public int health;
    private int value;
    public int minValue;
    public int maxValue;
    private LevelManager levelManager;
    public GameObject message;
    public AudioClip sound;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        value = Random.Range(minValue, maxValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if(health <= 0)
        {
            Camera.main.GetComponent<AudioSource>().clip = sound;
            Camera.main.GetComponent<AudioSource>().Play();
            levelManager.ChangeTxtServerRpc("+" + value + "  gloos");
            levelManager.UpdateGloosServerRpc(value);

            if(IsServer)
                NetworkObject.Despawn();
        }
    }
}
