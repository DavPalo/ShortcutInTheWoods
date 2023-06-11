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
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            //levelManager.enabled = true;
            transform.parent.gameObject.SetActive(false);
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            //levelManager.enabled = true;
            transform.parent.gameObject.SetActive(false);
        });
    }
}
