using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GoosUpdate : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI goos;

    [ClientRpc]
    public void updateGoosClientRpc(int value)
    {
        if (goos != null)
        {
            int actualGoos = int.Parse(goos.text);
            actualGoos += value;
            goos.text = actualGoos.ToString();
        }
    }
}
