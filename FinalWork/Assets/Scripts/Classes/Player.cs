using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    int playerID;
    int role;

    public void SetRole(int role)
    {
        this.role = role;
    }

    public int GetRole()
    {
        return role;
    }

    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }
    public int GetPlayerID()
    {
        return playerID;
    }

}
