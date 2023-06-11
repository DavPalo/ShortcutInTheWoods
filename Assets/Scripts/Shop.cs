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
    LevelManager lobby;

    private void Start()
    {
        gameObject.SetActive(false);
        lobby = GameObject.Find("LevelManager").GetComponent<LevelManager>();

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
        lobby.updateLifeServerRpc(10);
        lobby.updateGoosServerRpc(-10);
    }

    public void Increase()
    {
        lobby.increaseLifeServerRpc(10);
        lobby.updateGoosServerRpc(-10);
    }
}
