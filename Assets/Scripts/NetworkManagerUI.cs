using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private Canvas startCanvas;
    [SerializeField] private Canvas readyCanvas;

    private void Awake()
    {
        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            readyCanvas.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            LevelManager.PauseGame();
            startCanvas.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        });
    }
}
