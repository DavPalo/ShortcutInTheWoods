using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public static List<Player> players;

    public GameObject vehicle;
    public static GameObject[] weapons;
    [SerializeField] GameObject enemyPrefab;

    public static bool startGame;
    public static bool gameOver;

    [SerializeField] ProjectSceneManager projectSceneManager;

    private void Start()
    {
        startGame = false;
        gameOver = false;
        vehicle = GameObject.Find("Vehicle");
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].transform.parent = vehicle.transform;
        }
        if(IsServer)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<NetworkObject>().Spawn(true);

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

}
