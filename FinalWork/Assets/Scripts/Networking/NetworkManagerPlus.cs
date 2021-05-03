using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManagerPlus : NetworkManager
{
    private List<Player> players = new List<Player>();
    public List<GameObject> playerObjects = new List<GameObject>();

    public int amountOfPlayers = 0;
    public int MaxAmountOfPlayers;

    private GameManager gameManager;

    [SerializeField]
    private GameObject gameManagerObj;

    public override void OnStartServer()
    {
        gameManagerObj = Instantiate(gameManagerObj, new Vector3(0, 0, 0), Quaternion.identity);
        gameManagerObj.name = "GameManager";
        NetworkServer.Spawn(gameManagerObj);
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("connect player!");
        GameObject go = Instantiate(playerPrefab, new Vector3(Random.Range(-7.5f, 7.5f), 4, Random.Range(-7.5f, 7.5f)), Quaternion.identity);
        var player = new Player();
        var connectionId = conn.connectionId;

        player.SetName("player " + amountOfPlayers);
        player.SetPlayerID(connectionId);

        // players.Add(player);
        // playerObjects.Add(go);

        go.GetComponent<PlayerManager>().SetPlayerClass(player);
        gameManager.AddPlayer(player, go);
        amountOfPlayers = gameManager.GetPlayerCount();


        //Testing
        playerPrefab.GetComponent<PlayerDebugScript>().SetPlayerID(connectionId);

        Debug.Log("amountOfPlayers: " + amountOfPlayers);
        NetworkServer.AddPlayerForConnection(conn, go);

        if (amountOfPlayers == MaxAmountOfPlayers)
        {
            Debug.Log("Start Game! ");
            StartGame();
        }


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
        gameManager.OnAllPlayersConnected();
        players.Clear();
        playerObjects.Clear();
    }
}
