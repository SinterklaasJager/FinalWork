using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    int playerID;
    int role;

    bool wasTeamLeader;
    bool isTeamLeader;

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
    public void SetWasTeamLeader(bool wasTeamLeader)
    {
        this.wasTeamLeader = wasTeamLeader;
    }

    public bool GetWasTeamLeader()
    {
        return wasTeamLeader;
    }
    public void SetIsTeamLeader(bool isTeamLeader)
    {
        this.isTeamLeader = isTeamLeader;
    }

    public bool GetIsTeamLeader()
    {
        return isTeamLeader;
    }

}
