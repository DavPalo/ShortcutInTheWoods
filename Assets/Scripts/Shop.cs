using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class Shop : NetworkBehaviour
{
    [SerializeField] private Button repairBtn;
    [SerializeField] private Button increaseHealthBtn;
    public int repairCost;
    public int increaseHealthCost;

    public Vehicle vehicle;
    public LevelManager levelManager;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle").GetComponent<Vehicle>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        repairBtn.onClick.AddListener(() =>
        {
            Repair();
        });

        increaseHealthBtn.onClick.AddListener(() =>
        {
            Increase(); 
        });
    }


    
    public void Repair()
    {
        if (vehicle.currentHealth.Value < vehicle.maxHealth.Value)
        {
            levelManager.updateGoosServerRpc(repairCost);
            vehicle.RepairClientRpc(50);
        }
    }

    
    public void Increase()
    {
        levelManager.updateGoosServerRpc(increaseHealthCost);
        vehicle.IncreaseClientRpc(50);
    }
}
