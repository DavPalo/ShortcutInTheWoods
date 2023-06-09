using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GoosUpdate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goosText;

    private void Update()
    {
        updateGoosTextClientRpc();
    }

    [ClientRpc]
    public void updateGoosTextClientRpc()
    {
        if (goosText != null)
        {
            goosText.text = LevelManager.goos.Value.ToString();
        }
    }
}
