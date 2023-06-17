using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void MainMenu()
    {
        GameObject networkManager = GameObject.Find("NetworkManager");
        Destroy(networkManager);
        SceneManager.LoadScene(0);
    }
}
