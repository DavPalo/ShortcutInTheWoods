using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public GameObject shield;

    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

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
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealthClientRpc(currentHealth);

        if (currentHealth <= 0)
        {
            levelManager.LoadGameOverScene();
        }
    }
}
