using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public GameObject vehicle;
    public static GameObject[] weapons;
    [SerializeField] GameObject enemyPrefab;
    public static bool gameOver;

    [SerializeField] ProjectSceneManager projectSceneManager;

    public NetworkVariable<int> goos = new NetworkVariable<int>(0);
    [SerializeField] TextMeshProUGUI goosText;

    private void Start()
    {
        gameOver = false;
        vehicle = GameObject.Find("Vehicle");
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        if(IsServer)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].transform.parent = vehicle.transform;
            }
        }
        if (IsServer)
        {
            //GameObject enemy = Instantiate(enemyPrefab);
            //enemy.GetComponent<NetworkObject>().Spawn(true);

        }
    }

    private void Update()
    {
        if(gameOver)
            projectSceneManager.ChangeScene();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateGoosServerRpc(int value)
    {
        goos.Value += value;
        updateGoosTextClientRpc();
    }

    [ClientRpc]
    public void updateGoosTextClientRpc()
    {
        if (goosText != null)
        {
            goosText.text = goos.Value.ToString();
        }
    }
}
