using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string userType;
    public string nickname;

    public Player(string userType, string nickname)
    {
        this.userType = userType;
        this.nickname = nickname;
    }
}
