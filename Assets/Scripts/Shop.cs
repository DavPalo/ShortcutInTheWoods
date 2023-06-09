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

    public TextMeshProUGUI goosText;

    public Vehicle vehicle;
    public LevelManager levelManager;

    private void Start()
    {
        vehicle = GameObject.Find("Vehicle").GetComponent<Vehicle>();
        goosText = GameObject.Find("N").GetComponent<TextMeshProUGUI>();
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
        levelManager.updateGoosClientRpc(-5);
    }

    
    public void Increase()
    {
        levelManager.updateGoosClientRpc(-10);
    }
}
