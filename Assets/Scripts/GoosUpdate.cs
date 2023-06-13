using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class GoosUpdate : NetworkBehaviour
{
    [SerializeField] Text goos;
    public LevelManager levelManager;
    private string toShow = "";

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        toShow = levelManager.networkGoos.Value.ToString();
        goos.text = toShow;
    }

    // Si può rimuovere
    [ClientRpc]
    public void updateGoosTextClientRpc(int value)
    {
        if (goos != null)
        {
            goos.text = value.ToString();
        }
    }
}
