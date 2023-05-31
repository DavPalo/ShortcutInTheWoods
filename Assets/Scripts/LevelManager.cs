using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public GameObject[] players;
    public static GameObject vehicle;
    public GameObject[] weapons;
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        for(int i = 0; i < NetworkManagerUI.players.Count; i++)
        {
            if (NetworkManagerUI.players[i].userType == "Host")
                NetworkManager.Singleton.StartHost();
            else if(NetworkManagerUI.players[i].userType == "Client")
                NetworkManager.Singleton.StartClient();
        }

        Debug.Log("Is server ? - " + IsServer);

        if(IsServer)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            vehicle = GameObject.Find("Vehicle");
            weapons = GameObject.FindGameObjectsWithTag("Weapon");
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].transform.parent = vehicle.transform;
            }
        }

        //GameObject enemy = Instantiate(enemyPrefab);
        //enemy.GetComponent<NetworkObject>().Spawn(true);

    }

    public void LoadGameOverScene()
    {
        for(int i = 0; i < players.Length; i++)
        {
            Destroy(players[i]);
        }

        for(int i = 0; i < weapons.Length; i++)
        {
            Destroy(weapons[i]);
        }

        SceneManager.LoadScene("Game Over");
    }

}
