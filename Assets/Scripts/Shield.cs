using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shield : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool activated;
    public float shieldDuration;
    public float shieldCooldown;
    public bool activable;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activated = false;
        activable = true;
    }

    IEnumerator autoDisableShield()
    {
        yield return new WaitForSeconds(shieldDuration);
        disableShieldClientRpc();
        yield return new WaitForSeconds(shieldCooldown);
        activable = true;
    }

    [ClientRpc]
    public void disableShieldClientRpc()
    {
        activated = false;
        Color newColor = spriteRenderer.color;
        newColor.a = 0f;
        spriteRenderer.color = newColor;
    }

    [ClientRpc]
    public void activateShieldClientRpc()
    {
        activated = true;
        activable = false;
        Color newColor = spriteRenderer.color;
        newColor.a = 0.8f;
        spriteRenderer.color = newColor;
        StartCoroutine(autoDisableShield());
    }

    [ServerRpc(RequireOwnership = false)]
    public void activateShieldServerRpc()
    {
        activateShieldClientRpc();
    }
}
