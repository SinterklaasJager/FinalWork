using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameManager : NetworkBehaviour
{
    public SyncList<Player> syncedPlayers = new SyncList<Player>();
    public SyncList<GameObject> syncedPlayerObjects = new SyncList<GameObject>();

    /* private List<Player> players = new List<Player>();
     public List<GameObject> playerObjects = new List<GameObject>();
     */
    public Helper helpers;
    public Enums enums;
    public RoundManager roundManager;
    public UIManager uIManager;
    public VoteForOrganisers voteForOrganisers;
    public VictoryProgress victoryProgress = new VictoryProgress();
    public CardGeneration cardGeneration;


    void Start()
    {
        roundManager = gameObject.GetComponent<RoundManager>();
        voteForOrganisers = gameObject.GetComponent<VoteForOrganisers>();
        cardGeneration = gameObject.GetComponent<CardGeneration>();
        uIManager.SetGameManager(gameObject);

    }

    public int AddPlayer(Player newPlayer, GameObject go)
    {
        syncedPlayers.Add(newPlayer);
        syncedPlayerObjects.Add(go);

        Debug.Log("playersynclist: " + syncedPlayers);

        return syncedPlayers.Count;
    }


    public void OnAllPlayersConnected()
    {

        RoleDivider();

        roundManager.RoundSetUp(gameObject.GetComponent<GameManager>(), uIManager);

    }


    private void RoleDivider()
    {
        Debug.Log("Start RoleDivider");
        RoleDivider _roleDivider = new RoleDivider();

        var players = new List<Player>();

        for (int i = 0; i < syncedPlayers.Count; i++)
        {
            players.Add(syncedPlayers[i]);
        }

        _roleDivider.GivePlayersRoles(players);

        for (int i = 0; i < syncedPlayers.Count; i++)
        {
            syncedPlayers[i] = players[i];
        }

        //Testing Roles

        TestRoleDivider();

    }

    // [ClientRpc]
    private void TestRoleDivider()
    {
        Debug.Log("Test RoleDivider");

        foreach (var playerObj in syncedPlayerObjects)
        {
            var player = playerObj.gameObject.GetComponent<PlayerManager>().GetPlayerClass();
            foreach (var pl in syncedPlayers)
            {
                if (player == pl)
                {
                    if (pl.GetRole() == 0)
                    {
                        rpcTestRoleDivider(playerObj, Color.green);
                    }
                    else if (pl.GetRole() == 1)
                    {
                        rpcTestRoleDivider(playerObj, Color.yellow);
                    }
                    else if (pl.GetRole() == 2)
                    {
                        rpcTestRoleDivider(playerObj, Color.red);
                    }
                }
            }
        }
    }
    [ClientRpc]
    private void rpcTestRoleDivider(GameObject go, Color col)
    {
        go.GetComponent<MeshRenderer>().material.color = col;
    }
    public List<Player> GetPlayers()
    {
        var players = new List<Player>();

        for (int i = 0; i < syncedPlayers.Count; i++)
        {
            players.Add(syncedPlayers[i]);
        }
        return players;
    }

}
