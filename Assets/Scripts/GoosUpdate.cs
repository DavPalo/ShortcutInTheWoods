using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GoosUpdate : NetworkBehaviour
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
