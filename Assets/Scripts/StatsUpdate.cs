using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class StatsUpdate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI health;
    public LevelManager levelManager;
    private string toShow = "";

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        toShow = levelManager.networkHealth.Value.ToString() + " / " + levelManager.networkMaxHealth.Value.ToString();
        health.text = toShow;
    }
}
