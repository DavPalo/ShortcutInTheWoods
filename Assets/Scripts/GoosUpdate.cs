using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class GoosUpdate : NetworkBehaviour
{
    [SerializeField] Text goos;

    [ClientRpc]
    public void updateGoosTextClientRpc(int value)
    {
        if (goos != null)
        {
            goos.text = value.ToString();
        }
    }
}
