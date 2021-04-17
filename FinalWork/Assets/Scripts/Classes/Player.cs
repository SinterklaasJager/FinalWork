using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    string name;
    int playerID, role;
    bool wasAssistant, isTeamLeader, isAssistant, isDead, isAssistantCandidate, isTeamLeaderCandidate;


    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }

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
    public void SetWasAssistant(bool wasAssistant)
    {
        this.wasAssistant = wasAssistant;
    }

    public bool GetWasAssistant()
    {
        return wasAssistant;
    }
    public void SetIsTeamLeader(bool isTeamLeader)
    {
        this.isTeamLeader = isTeamLeader;
    }

    public bool GetIsTeamLeader()
    {
        return isTeamLeader;
    }

    public void SetIsAssistant(bool isAssistant)
    {
        this.isAssistant = isAssistant;
    }

    public bool GetIsAssistant()
    {
        return isAssistant;
    }

    public void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
    public void SetIsAssistantCandidate(bool isAssistantCandidate)
    {
        this.isAssistantCandidate = isAssistantCandidate;
    }

    public bool GetisAssistantCandidate()
    {
        return isAssistantCandidate;
    }

    public void SetIsTeamLeaderCandidate(bool IsTeamLeaderCandidate)
    {
        this.isTeamLeaderCandidate = IsTeamLeaderCandidate;
    }

    public bool GetIsTeamLeaderCandidate()
    {
        return isTeamLeaderCandidate;
    }


}
