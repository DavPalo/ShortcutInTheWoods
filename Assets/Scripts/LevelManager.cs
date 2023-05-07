using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        if (NetworkManagerUI.userType == "Server")
            NetworkManager.Singleton.StartServer();
        else if (NetworkManagerUI.userType == "Client")
            NetworkManager.Singleton.StartClient();
        else if (NetworkManagerUI.userType == "Host")
            NetworkManager.Singleton.StartHost();
    }
}
