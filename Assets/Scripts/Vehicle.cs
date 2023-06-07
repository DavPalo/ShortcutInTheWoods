using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vehicle : NetworkBehaviour
{
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public HealthBar healthBar;

    public GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth.Value = 100;
        currentHealth.Value = maxHealth.Value;
        healthBar.SetMaxHealthClientRpc(maxHealth.Value);

        shield = GameObject.Find("Shield");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }

        if (collision.gameObject.tag == "Enemy" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy>().attack);
        }

        if (collision.gameObject.tag == "Enemy2" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy2>().attack);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth.Value -= damage;

        healthBar.SetHealthClientRpc(currentHealth.Value);

        if (currentHealth.Value <= 0)
        {
            LevelManager.gameOver = true;
        }
    }

    public void Repair(int healing)
    {
        currentHealth.Value += healing;

        if(currentHealth.Value > maxHealth.Value)
            currentHealth.Value = maxHealth.Value;
            
        healthBar.SetHealthClientRpc(currentHealth.Value);
    }

    public void Increase(int health)
    {
        maxHealth.Value += health;
        currentHealth.Value = maxHealth.Value;
        healthBar.SetMaxHealthClientRpc(maxHealth.Value);
    }
}
