using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Player playerClass;

    public void SetPlayerClass(Player player)
    {
        playerClass = player;
    }
    public Player GetPlayerClass()
    {
        return playerClass;
    }

}
