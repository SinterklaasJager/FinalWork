using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> testPlayerList = new List<GameObject>();
    private List<Player> players = new List<Player>();
    public Helper helpers;
    public Enums enums;
    public RoundManager roundManager;
    public UIManager uIManager;

    void Start()
    {
        LoadPlayers();

        roundManager = gameObject.GetComponent<RoundManager>();
        uIManager.SetGameManager(gameObject);
        uIManager.IniateRoundUI();

        OnAllPlayersConnected();
    }


    void Update()
    {

    }

    private void LoadPlayers()
    {
        var i = 0;
        foreach (var player in testPlayerList)
        {
            var PlayerManager = player.AddComponent<PlayerManager>();
            PlayerManager.GetPlayerClass().SetName("player " + i);
            players.Add(PlayerManager.GetPlayerClass());

            i++;
        }
        RoleDivider();
    }

    public void OnAllPlayersConnected()
    {
        roundManager.players = players;
        roundManager.StartTurn();

    }


    private void RoleDivider()
    {
        RoleDivider _roleDivider = new RoleDivider();
        _roleDivider.GivePlayersRoles(players);

        foreach (var player in testPlayerList)
        {
            if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 0))
            {
                player.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 1))
            {
                player.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 2))
            {
                player.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
    }


}
