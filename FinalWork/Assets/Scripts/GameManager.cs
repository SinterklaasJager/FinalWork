using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameManager : NetworkBehaviour
{
    public SyncList<Player> syncedPlayers = new SyncList<Player>();
    public SyncList<GameObject> syncedPlayerObjects = new SyncList<GameObject>();
    public SyncList<int> deathPlayerIds = new SyncList<int>();

    public Helper helpers;
    public Enums enums;
    private GameObject roundManagerObj, cardGenerationObj, universalCanvasObj, uIManagerObj, ARManagerObj, lobbyUIObj;
    public UniversalCanvasManager universalCanvas;
    private GameManager gameManager;
    public RoundManager roundManager;
    public UIManager uIManager;
    public NetworkManagerPlus networkManager;
    public VoteForOrganisers voteForOrganisers;
    private GameObject victoryProgressObj;
    public VictoryProgress victoryProgress;
    public CardGeneration cardGeneration;
    public AddName addName;
    public SpawnableObjects spawnableObjects;
    public LobbyUIManager lobbyUIManager;
    public GameObject gameLocationObject;

    public Enums.AREvents AREvents;

    public void SetNetworkManager(NetworkManagerPlus nwp)
    {
        networkManager = nwp;
    }

    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        spawnableObjects = gameObject.GetComponent<SpawnableObjects>();
        voteForOrganisers = gameObject.GetComponent<VoteForOrganisers>();
        addName = gameObject.GetComponent<AddName>();

        universalCanvasObj = Instantiate(spawnableObjects.universalCanvas, transform);
        universalCanvas = universalCanvasObj.GetComponent<UniversalCanvasManager>();
        universalCanvas.SetUp(gameManager);

        uIManagerObj = Instantiate(spawnableObjects.UIManager, transform);
        uIManager = uIManagerObj.GetComponent<UIManager>();
        uIManager.SetGameManager(gameObject, universalCanvas);

        cardGenerationObj = Instantiate(spawnableObjects.cardGeneration, gameObject.transform);
        cardGeneration = cardGenerationObj.GetComponent<CardGeneration>();
        cardGeneration.SetUp(gameManager, helpers);

        AREvents.OnHostGameLocation = (gameLocationPos, rotation) => SetGameLocationPosition(gameLocationPos, rotation);
    }

    public void AddPlayer(NetworkConnection conn, Player newPlayer, GameObject go)
    {
        // newPlayer.SetName(beforeGameStart.UserName);
        syncedPlayers.Add(newPlayer);
        syncedPlayerObjects.Add(go);
        //SetPlayerName(beforeGameStart);
        Debug.Log("player: " + syncedPlayers[syncedPlayers.Count - 1].GetName() + " connected");

        if (syncedPlayers.Count == 1)
        {
            FirstPlayerConnected();
        }

        lobbyUIManager.AddNewPlayer(newPlayer.GetName());
    }

    public void FirstPlayerConnected()
    {
        NetworkServer.Spawn(uIManagerObj);
        lobbyUIObj = Instantiate(spawnableObjects.LobbyUI, uIManager.transform);
        lobbyUIManager = lobbyUIObj.GetComponent<LobbyUIManager>();
        NetworkServer.Spawn(lobbyUIObj);
        Debug.Log("networkManager.MaxAmountOfPlayers " + networkManager.MaxAmountOfPlayers);
        lobbyUIManager.SetMaxAmountOfPlayers(networkManager.MaxAmountOfPlayers);
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
        NetworkServer.Spawn(cardGenerationObj);
        NetworkServer.Spawn(universalCanvasObj);


        SpawnNameGetUI();
    }

    public void SpawnNameGetUI()
    {
        foreach (var playerObj in syncedPlayerObjects)
        {
            uIManager.StartPlayerNameUI(playerObj.GetComponent<NetworkIdentity>().connectionToClient, playerObj, gameManager);
        }
    }

    public void AllPlayersReady()
    {
        RoleDivider();

        roundManagerObj = Instantiate(spawnableObjects.roundManager, gameObject.transform);
        roundManager = roundManagerObj.GetComponent<RoundManager>();
        NetworkServer.Spawn(roundManagerObj, syncedPlayerObjects[0]);
        roundManager.RoundSetUp(gameManager, uIManager.gameObject);

        victoryProgressObj = Instantiate(spawnableObjects.victoryProgress, gameLocationObject.transform);
        victoryProgress = victoryProgressObj.GetComponent<VictoryProgress>();
        NetworkServer.Spawn(victoryProgressObj);
        victoryProgress.SetGameManager(gameManager, roundManager);

        SetPlayerUI();
    }

    public void StartAR()
    {
        ARManagerObj = Instantiate(spawnableObjects.ARManagerObject, transform);
        NetworkServer.Spawn(ARManagerObj);
    }
    public void InstantiateARHostUI(GameObject go)
    {
        StartAR();
        Debug.Log("InstantiateARHostUI");

        uIManager.InstantiateARHostUI(go.GetComponent<NetworkIdentity>().connectionToClient, gameManager, ARManagerObj);
    }

    [Command(requiresAuthority = false)]
    public void SetGameLocationPosition(Vector3 gameLocationPosition, Quaternion rotation, NetworkConnectionToClient sender = null)
    {
        gameLocationObject = Instantiate(spawnableObjects.gameLocationObject, gameLocationPosition, rotation);
        NetworkServer.Spawn(gameLocationObject);
        networkManager.GameLocationPicked();
    }

    private void SetPlayerUI()
    {
        foreach (var player in syncedPlayers)
        {
            GameObject playerObject = null;

            foreach (var playerObj in syncedPlayerObjects)
            {
                if (playerObj.GetComponent<PlayerManager>().GetPlayerClass() == player)
                {
                    playerObject = playerObj;
                }
            }
            uIManager.InstantiatePlayerUI(playerObject.GetComponent<NetworkIdentity>().connectionToClient, player.GetName(), player.GetRole(), gameManager);

            foreach (var pl in syncedPlayers)
            {
                if (pl != player)
                {
                    if (pl.GetRole() != 0)
                    {
                        uIManager.SetPlayerUIAllies(playerObject.GetComponent<NetworkIdentity>().connectionToClient, pl.GetRole(), pl.GetName());
                    }
                }
            }
        }

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
