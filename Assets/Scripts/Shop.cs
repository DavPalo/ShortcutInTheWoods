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

    public Vehicle vehicle;
    public LevelManager levelManager;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle").GetComponent<Vehicle>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        repairBtn.onClick.AddListener(() =>
        {
            RepairServerRpc();
        });

        increaseHealthBtn.onClick.AddListener(() =>
        {
            Increase(); 
        });
    }


    [ServerRpc(RequireOwnership = false)]
    public void RepairServerRpc()
    {
        Debug.Log("RepairServerRpc called");
        if (vehicle.currentHealth.Value < vehicle.maxHealth.Value)
        {
            Debug.Log("Inside if");
            levelManager.updateGoosServerRpc(-5);
            Debug.Log("netvar updated");
            vehicle.RepairClientRpc(50);
        }
    }

    
    public void Increase()
    {
        Debug.Log("Increase called");
        levelManager.updateGoosServerRpc(-10);
        vehicle.IncreaseClientRpc(50);
    }
}
