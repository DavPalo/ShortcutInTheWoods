using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        startBtn.onClick.AddListener(() =>
        {
            levelManager.enabled = true;
            LevelManager.ResumeGame();
            gameObject.SetActive(false);
        });
    }
}
