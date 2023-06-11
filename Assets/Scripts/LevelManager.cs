using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public NetworkVariable<int> networkHealth;
    public NetworkVariable<int> networkGoos;

    public GameObject GoosText;
    public GameObject HealtBar;

    private void Start()
    {
        if (IsServer)
        {
            networkHealth.Value = 100;
            networkGoos.Value = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            updateLifeServerRpc(-10);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateGoosServerRpc(int value)
    {
        networkGoos.Value += value;
        GoosText.GetComponent<GoosUpdate>().updateGoosTextClientRpc(networkGoos.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateLifeServerRpc(int value)
    {
        networkHealth.Value += value;
        HealtBar.GetComponent<HealthBar>().SetHealthClientRpc(networkHealth.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void increaseLifeServerRpc(int value)
    {
        networkHealth.Value += value;
        HealtBar.GetComponent<HealthBar>().SetMaxHealthClientRpc(networkHealth.Value);
    }
}
