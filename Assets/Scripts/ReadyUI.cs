using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ReadyUI : NetworkBehaviour
{
    [SerializeField] private Button readyBtn;

    private void Awake()
    {
        readyBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
