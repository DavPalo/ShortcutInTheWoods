using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public NetworkVariable<int> networkHealth;
    public NetworkVariable<int> networkGloos;
    public NetworkVariable<int> networkMaxHealth;
    public NetworkVariable<int> damage;
    public NetworkVariable<bool> key;
    public NetworkVariable<FixedString128Bytes> interaction = new NetworkVariable<FixedString128Bytes>("");

    public GameObject HealthBar;

    public ProjectSceneManager projectSceneManager;

    private void Start()
    {
        key.Value = false;
        projectSceneManager = GetComponent<ProjectSceneManager>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateGloosServerRpc(int value)
    {
        networkGloos.Value += value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(int value)
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
    public void IncreaseHealthServerRpc(int value)
    {
        networkMaxHealth.Value += value;
        networkHealth.Value = networkMaxHealth.Value;

        HealthBar.GetComponent<HealthBar>().SetMaxHealthClientRpc(networkHealth.Value);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void IncreaseDmgServerRpc(int value)
    {
        damage.Value += value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void KeyGainedServerRpc()
    {
        key.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeTxtServerRpc(string txt)
    {
        interaction.Value = txt;
    }
}
