using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameManager : NetworkBehaviour
{
    public SyncList<Player> syncedPlayers = new SyncList<Player>();
    public SyncList<GameObject> syncedPlayerObjects = new SyncList<GameObject>();

    public Helper helpers;
    public Enums enums;
    private GameObject roundManagerObj, cardGenerationObj, universalCanvasObj, uIManagerObj;
    public UniversalCanvasManager universalCanvas;
    private GameManager gameManager;
    public RoundManager roundManager;
    public UIManager uIManager;
    public VoteForOrganisers voteForOrganisers;
    private GameObject victoryProgressObj;
    public VictoryProgress victoryProgress;
    public CardGeneration cardGeneration;
    public SpawnableObjects spawnableObjects;


    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        spawnableObjects = gameObject.GetComponent<SpawnableObjects>();
        voteForOrganisers = gameObject.GetComponent<VoteForOrganisers>();

        universalCanvasObj = Instantiate(spawnableObjects.universalCanvas, transform);
        universalCanvas = universalCanvasObj.GetComponent<UniversalCanvasManager>();
        universalCanvas.SetUp(gameManager);

        uIManagerObj = Instantiate(spawnableObjects.UIManager, transform);
        uIManager = uIManagerObj.GetComponent<UIManager>();
        uIManager.SetGameManager(gameObject, universalCanvas);

        cardGenerationObj = Instantiate(spawnableObjects.cardGeneration, gameObject.transform);
        cardGeneration = cardGenerationObj.GetComponent<CardGeneration>();
        cardGeneration.SetUp(gameManager, helpers);

    }

    public void AddPlayer(Player newPlayer, GameObject go)
    {
        syncedPlayers.Add(newPlayer);
        syncedPlayerObjects.Add(go);

        Debug.Log("player: " + syncedPlayers[syncedPlayers.Count - 1].GetName() + " connected");
    }

    public int GetPlayerCount()
    {
        return syncedPlayers.Count;
    }

    public void RemovePlayer(NetworkConnection conn)
    {
        Debug.Log("Player disconnect, remove player");
        foreach (var player in syncedPlayers)
        {
            if (player.GetPlayerID() == conn.connectionId)
            {
                syncedPlayers.Remove(player);
                foreach (var playerObj in syncedPlayerObjects)
                {
                    if (playerObj.GetComponent<PlayerManager>().GetPlayerClass() == player)
                    {
                        syncedPlayerObjects.Remove(playerObj);
                    }
                }
            }
        }
    }

    public void OnAllPlayersConnected()
    {
        RoleDivider();

        NetworkServer.Spawn(cardGenerationObj);
        NetworkServer.Spawn(universalCanvasObj);
        NetworkServer.Spawn(uIManagerObj);

        roundManagerObj = Instantiate(spawnableObjects.roundManager, gameObject.transform);
        roundManager = roundManagerObj.GetComponent<RoundManager>();
        NetworkServer.Spawn(roundManagerObj, syncedPlayerObjects[0]);
        roundManager.RoundSetUp(gameManager, uIManager.gameObject);

        victoryProgressObj = Instantiate(spawnableObjects.victoryProgress, gameObject.transform);
        victoryProgress = victoryProgressObj.GetComponent<VictoryProgress>();
        NetworkServer.Spawn(victoryProgressObj);
        victoryProgress.SetGameManager(gameManager);
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
