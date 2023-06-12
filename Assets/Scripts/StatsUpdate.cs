using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class StatsUpdate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI health;
    public LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (IsServer)
        {
            updateHealthTextClientRpc(levelManager.networkHealth.Value);
            Debug.Log("HH");
        }
    }

    [ClientRpc]
    public void updateHealthTextClientRpc(int value)
    {
        if (health != null)
        {
            health.text = value.ToString();
        }
    }
}
