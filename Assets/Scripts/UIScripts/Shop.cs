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
    public int repairValue;
    public int increaseHealthCost;
    public int increaseHealthValue;
    private LevelManager levelManager;

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
        if(levelManager.networkGloos.Value >= repairCost && levelManager.networkHealth.Value < levelManager.networkMaxHealth.Value)
        {
            levelManager.UpdateHealthServerRpc(repairValue);
            levelManager.UpdateGloosServerRpc(-repairCost);

        }
    }

    public void Increase()
    {
        if (levelManager.networkGloos.Value >= increaseHealthCost)
        {
            levelManager.IncreaseHealthServerRpc(increaseHealthValue);
            levelManager.UpdateGloosServerRpc(-increaseHealthCost);

        }
    }
}
