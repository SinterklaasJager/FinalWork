using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDivider
{
    //0 is goodguy
    //1 is badguy
    //2 is saboteur

    public void GivePlayersRoles(List<Player> playerList)
    {
        playerList = ShufflePlayerList(playerList);

        int amountOfBadGuys = 0;
        int amountOfSaboteurs = 0;

        foreach (var player in playerList)
        {
            if (amountOfBadGuys < 1)
            {
                player.SetRole(1);
                amountOfBadGuys += 1;
            }
            else if (amountOfSaboteurs < 1)
            {
                player.SetRole(2);
                amountOfSaboteurs += 1;
            }
            else if (amountOfSaboteurs >= 1 && amountOfBadGuys >= 1)
            {
                player.SetRole(0);
            }
        }
    }

    public List<Player> ShufflePlayerList(List<Player> playerList)
    {

        for (int i = 0; i < playerList.Count; i++)
        {
            var temp = playerList[i];
            int randomIndex = Random.Range(i, playerList.Count);
            playerList[i] = playerList[randomIndex];
            playerList[randomIndex] = temp;
        }

        return playerList;
    }
}