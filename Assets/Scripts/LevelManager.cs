using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public NetworkVariable<int> networkHealth;
    public NetworkVariable<int> networkGoos;
    public NetworkVariable<int> networkMaxHealth;
    public NetworkVariable<int> damage;
    public NetworkVariable<bool> Key;

    public GameObject GoosText;
    public GameObject HealthBar;
    public GameObject HealthText;

    public ProjectSceneManager projectSceneManager;

    private void Start()
    {
        Key.Value = false;
        projectSceneManager = GetComponent<ProjectSceneManager>();
    }

    private void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateGoosServerRpc(int value)
    {
        networkGoos.Value += value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateHealthServerRpc(int value)
    {
        networkHealth.Value += value;
        if(networkHealth.Value > networkMaxHealth.Value)
        {
            networkHealth.Value = networkMaxHealth.Value;
        }
        else if(networkHealth.Value < 0)
        {
            networkHealth.Value = 0;
        }

        HealthBar.GetComponent<HealthBar>().SetHealthClientRpc(networkHealth.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void increaseHealthServerRpc(int value)
    {
        networkMaxHealth.Value += value;
        networkHealth.Value = networkMaxHealth.Value;

        HealthBar.GetComponent<HealthBar>().SetMaxHealthClientRpc(networkHealth.Value);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void increaseDmgServerRpc(int value)
    {
        damage.Value += value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void KeyGainedServerRpc()
    {
        Key.Value = true;
    }
}
