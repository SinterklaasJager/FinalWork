using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDivider
{
    //0 is goodguy
    //1 is badguy
    //2 is saboteur

    private Helper Helpers = new Helper();
    // List<Roles> rolesList = new List<Roles>();

    void Start()
    {

    }

    public void GivePlayersRoles(List<Player> playerList)
    {
        playerList = GenerateRandomNumbers(playerList);

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

    private List<Player> ShufflePlayerList(List<Player> playerList)
    {

        List<int> randomNumbers = new List<int>();

        for (int i = 0; i < playerList.Count; i++)
        {
            randomNumbers.Add(i);
            Debug.Log("random number: " + randomNumbers[i]);
        }

        randomNumbers = Helpers.GenerateRandomNumbers(randomNumbers);

        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i] = playerList[randomNumbers[i]];
            Debug.Log("random playerlist: " + playerList[randomNumbers[i]]);
        }

        return playerList;
    }

    public List<Player> GenerateRandomNumbers(List<Player> playerList)
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