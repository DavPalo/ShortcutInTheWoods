using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerDebug : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private LevelManager levelManager;

    public static string userType;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            Debug.Log("Server");
            NetworkManager.Singleton.StartServer();
        });

        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            levelManager.GetComponent<LevelManager>().enabled = true;
        });
    }
}
