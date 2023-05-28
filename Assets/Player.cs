using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string nickname;

    private void Start()
    {
        nickname = NetworkManagerUI.nickname;
    }
}
