using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    public Slider slider;

    [ClientRpc]
    public void SetHealthClientRpc(int health)
    {
        slider.value = health;
    }

    //[ClientRpc]
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
