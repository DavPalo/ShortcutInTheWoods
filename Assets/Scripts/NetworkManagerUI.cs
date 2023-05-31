using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    public TMP_InputField inputField;

    public static List<Player> players = new List<Player>();

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            Debug.Log("Server");
            LoadNextScene();
        });

        clientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            /*if (players.Exists(x => x.userType == "Host"))
            {
                Debug.Log("ESISTE");
            }*/
                Player player = new Player("Client", inputField.text);
                players.Add(player);
                LoadNextScene();
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            Player player = new Player("Host", inputField.text);
            players.Add(player);
            LoadNextScene();
        });
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
