using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;

    public static string userType;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            Debug.Log("Server");
            userType = "Server";
            LoadNextScene();
        });

        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            userType = "Client";
            LoadNextScene();
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            userType = "Host";
            LoadNextScene();
        });
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
