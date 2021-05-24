using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManagerPlus : NetworkManager
{
    [Scene] [SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string gameScene = string.Empty;
    private List<Player> players = new List<Player>();
    public List<GameObject> playerObjects = new List<GameObject>();
    public string userName;
    public bool gameLocationPicked;
    public int amountOfPlayers = 0;
    public int MaxAmountOfPlayers;

    [SerializeField]
    private NetworkManagerHubDoublePlus networkManagerHubDoublePlus;

    private GameManager gameManager;

    [SerializeField]
    private GameObject gameManagerObj;
    public static event System.Action onClientConnected;
    public static event System.Action onClientDisconnected;
    public Enums.AREvents AREvents;

    public void SetData(string networkAd)
    {
        networkAddress = networkAd;
    }

    public override void OnStartServer()
    {
        gameManagerObj = Instantiate(gameManagerObj, new Vector3(0, 0, 0), Quaternion.identity);
        gameManagerObj.name = "GameManager";
        NetworkServer.Spawn(gameManagerObj);
        gameManager = gameManagerObj.GetComponent<GameManager>();
        gameManager.SetNetworkManager(this);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        onClientConnected?.Invoke();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        onClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        onClientDisconnected?.Invoke();
        if (gameManager != null)
        {
            if (gameManager.playersInLobby > 0)
            {
                gameManager.playersInLobby--;
            }
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("connect player!");
        if (amountOfPlayers == 1)
        {
            maxConnections = 1;
        }

        if (amountOfPlayers == 1 || gameLocationPicked)
        {
            GameObject go = Instantiate(playerPrefab, new Vector3(Random.Range(-7.5f, 7.5f), 4, Random.Range(-7.5f, 7.5f)), Quaternion.identity);
            var player = new Player();
            var connectionId = conn.connectionId;
            player.SetPlayerID(connectionId);

            go.GetComponent<PlayerManager>().SetPlayerClass(player);
            gameManager.AddPlayer(conn, player, go);
            amountOfPlayers = gameManager.GetPlayerCount();

            Debug.Log("amountOfPlayers: " + amountOfPlayers);

            //Testing
            playerPrefab.GetComponent<PlayerDebugScript>().SetPlayerID(connectionId);

            NetworkServer.AddPlayerForConnection(conn, go);

            if (amountOfPlayers == 1)
            {
                gameManager.InstantiateARHostUI(go);
            }
            else
            {
                gameManager.SpawnNameGetUI(go);
            }

            StartGame();
        }
    }

    public void GameLocationPicked()
    {
        Debug.Log("GameLocation Picked, Spawn Name UI");
        gameLocationPicked = true;
        maxConnections = MaxAmountOfPlayers;
        //StartGame();
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        foreach (Transform transform in gameManager.uIManager.transform)
        {
            Destroy(transform.gameObject);
        }

        gameManager.RemovePlayer(conn);
        amountOfPlayers--;
        base.OnServerDisconnect(conn);
    }
    public void StartGame()
    {
        Debug.Log("Check If there are enough players: " + amountOfPlayers);
        if (amountOfPlayers == MaxAmountOfPlayers && gameLocationPicked)
        {
            Debug.Log("Start Game! ");
            gameManager.OnAllPlayersConnected();
            players.Clear();
            playerObjects.Clear();
        }
        else
        {
            if (amountOfPlayers != MaxAmountOfPlayers)
            {
                Debug.Log("waiting for all players to connect");
            }
            if (!gameLocationPicked)
            {
                Debug.Log("waiting for Host to pick a location");
            }
        }
    }

}
