using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : NetworkBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        startBtn.onClick.AddListener(() =>
        {
            LevelManager.ResumeGame();

            for(int i  = 0; i < LevelManager.players.Count; i++)
            {
                if (LevelManager.players[i].userType == "Client")
                {
                    Debug.Log(LevelManager.players[i].nickname);
                    LevelManager.players[i].readyCanvas.gameObject.SetActive(false);
                }
            }

            gameObject.SetActive(false);
        });
    }
}
