using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string nickname;
    public string userType;
    public Canvas readyCanvas;

    public Player(string nickname, string userType)
    {
        this.nickname = nickname;
        this.userType = userType;
    }
}
