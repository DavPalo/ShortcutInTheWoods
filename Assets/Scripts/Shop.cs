using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button repairBtn;
    [SerializeField] private Button increaseHealthBtn;
    public int repairCost;
    public int increaseHealthCost;
    LevelManager levelManager;

    private void Start()
    {
        gameObject.SetActive(false);
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
        if(levelManager.networkGoos.Value >= repairCost && levelManager.networkHealth.Value < levelManager.networkMaxHealth.Value)
        {
            levelManager.updateHealthServerRpc(10);
            levelManager.updateGoosServerRpc(-repairCost);

        }
    }

    public void Increase()
    {
        if (levelManager.networkGoos.Value >= increaseHealthCost)
        {
            levelManager.increaseHealthServerRpc(10);
            levelManager.updateGoosServerRpc(-increaseHealthCost);

        }
    }
}
