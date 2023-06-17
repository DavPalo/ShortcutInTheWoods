using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vehicle : NetworkBehaviour
{
    private LevelManager levelManager;
    private GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
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
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                TakeDamage(collision.gameObject.GetComponent<Enemy>().attack);
            else
                TakeDamage(collision.gameObject.GetComponent<Boss>().attack);
        }

        if (collision.gameObject.tag == "Enemy2" && shield.GetComponent<Shield>().activated == false)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy2>().attack);
        }
    }

    void TakeDamage(int damage)
    {
        levelManager.UpdateHealthServerRpc(-damage);

        // Game Over
        if (levelManager.networkHealth.Value <= 0)
        {
          //  levelManager.projectSceneManager.ChangeScene();
        }
    }
}
