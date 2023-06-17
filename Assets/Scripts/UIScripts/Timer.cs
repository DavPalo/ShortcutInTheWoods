using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Timer : MonoBehaviour
{
    private LevelManager levelManager;
    public Text message;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    void Update()
    {
        if (levelManager.interaction.Value != "")
        {
            StartCoroutine(HideText());
        }
    }

    IEnumerator HideText()
    {
        message.text = levelManager.interaction.Value.ToString();
        yield return new WaitForSeconds(5);
        message.text = levelManager.interaction.Value.ToString();
        levelManager.ChangeTxtServerRpc("");
    }

    
}
