using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class AddName : NetworkBehaviour
{
    Enums.EventHandlers events;

    private GameManager gameManager;

    private string userName;
    [SyncVar] private int amountOfNamesAdded = 0;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    [Command(requiresAuthority = false)]
    public void AddUserName(string name, GameObject connObj)
    {
        foreach (var playerConn in gameManager.syncedPlayerObjects)
        {
            if (playerConn.GetComponent<NetworkIdentity>().netId == connObj.GetComponent<NetworkIdentity>().netId)
            {
                foreach (var player in gameManager.syncedPlayers)
                {
                    if (player == playerConn.GetComponent<PlayerManager>().GetPlayerClass())
                    {
                        player.SetName(name);
                        amountOfNamesAdded++;
                    }
                }

            }
        }

        if (amountOfNamesAdded == gameManager.GetPlayerCount())
        {
            //names ready
            gameManager.AllPlayersReady();
        }
    }
    public void OnNameReady()
    {

        events.OnNameEntered?.Invoke(userName);

    }
}
