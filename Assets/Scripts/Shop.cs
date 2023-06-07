using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button repairBtn;
    [SerializeField] private Button increaseHealthBtn;
    public int repairCost;
    public int increaseHealthCost;

    public TextMeshProUGUI goos;

    public Vehicle vehicle;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle").GetComponent<Vehicle>();
        goos = GameObject.Find("N").GetComponent<TextMeshProUGUI>();

        repairBtn.onClick.AddListener(() =>
        {
            RepairClientRpc();
        });

        increaseHealthBtn.onClick.AddListener(() =>
        {
            IncreaseClientRpc(); 
        });
    }

    [ClientRpc]
    public void RepairClientRpc()
    {
        int actualGoos = int.Parse(goos.text);
        if (actualGoos >= repairCost && vehicle.currentHealth.Value < vehicle.maxHealth.Value)
        {
            vehicle.Repair(50);
            actualGoos -= repairCost;
            goos.text = actualGoos.ToString();
        }
    }

    [ClientRpc]
    public void IncreaseClientRpc()
    {
        int actualGoos = int.Parse(goos.text);
        if (actualGoos >= increaseHealthCost)
        {
            vehicle.Increase(50);
            actualGoos -= increaseHealthCost;
            goos.text = actualGoos.ToString();
        }
    }
}
