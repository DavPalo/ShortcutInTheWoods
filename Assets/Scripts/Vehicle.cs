using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vehicle : NetworkBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        shield = GameObject.Find("Shield");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(10);
        }

        if (collision.gameObject.tag == "Enemy" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyOff>().attack);
        }

        if (collision.gameObject.tag == "Enemy2" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy2>().attack);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealthClientRpc(currentHealth);

        if (currentHealth <= 0)
        {
            LevelManager.gameOver = true;
        }
    }

    
}
