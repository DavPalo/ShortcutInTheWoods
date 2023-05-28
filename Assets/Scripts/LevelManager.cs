using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public static GameObject[] players;
    public static GameObject vehicle;
    public static GameObject[] weapons;
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        if (NetworkManagerUI.userType == "Server")
            NetworkManager.Singleton.StartServer();
        else if (NetworkManagerUI.userType == "Client")
            NetworkManager.Singleton.StartClient();
        else if (NetworkManagerUI.userType == "Host")
            NetworkManager.Singleton.StartHost();

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

    public static void LoadGameOverScene()
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
