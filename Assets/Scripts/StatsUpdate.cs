using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class StatsUpdate : NetworkBehaviour
{
    [SerializeField] Text health;
    [SerializeField] Text damage;
    public LevelManager levelManager;
    private string toShowH = "";
    private string toShowDmg = "";

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        toShowH = levelManager.networkHealth.Value.ToString() + " / " + levelManager.networkMaxHealth.Value.ToString();
        if(health)
            health.text = toShowH;

        toShowDmg = levelManager.damage.Value.ToString();
        if(damage)
            damage.text = toShowDmg;
    }
}
