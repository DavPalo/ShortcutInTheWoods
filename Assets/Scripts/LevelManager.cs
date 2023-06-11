
using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public NetworkVariable<int> networkHealt;
    public NetworkVariable<int> networkGoos;

    public GameObject GoosText;
    public GameObject HealtBar;

    private void Start()
    {
        if (IsServer)
        {
            networkHealt.Value = 100;
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
        networkHealt.Value += value;
        HealtBar.GetComponent<HealthBar>().SetHealthClientRpc(networkHealt.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void increaseLifeServerRpc(int value)
    {
        networkHealt.Value += value;
        HealtBar.GetComponent<HealthBar>().SetMaxHealthClientRpc(networkHealt.Value);
    }
}
