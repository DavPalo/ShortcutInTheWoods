using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;

    private void Awake()
    {
        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            transform.parent.gameObject.SetActive(false);
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            transform.parent.gameObject.SetActive(false);
        });
    }

    
}
