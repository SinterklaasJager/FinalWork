using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManagerPlus : NetworkManager
{
    private List<Player> players = new List<Player>();

    [SyncVar] private int amountOfPlayers;
    public int MaxAmountOfPlayers;

    private GameManager gameManager;

    public override void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject go = Instantiate(playerPrefab);
        var player = new Player();
        player.SetName("player " + amountOfPlayers);
        player.SetPlayerID(conn.connectionId);
        players.Add(player);
        // increment the index after setting on player, so first player starts at 0
        amountOfPlayers++;
        Debug.Log("amountOfPlayers: " + amountOfPlayers);
        NetworkServer.AddPlayerForConnection(conn, go);

        if (amountOfPlayers == MaxAmountOfPlayers)
        {
            Debug.Log("Start Game! ");
            StartGame();
        }


    }

    public void StartGame()
    {
        gameManager.OnAllPlayersConnected(players);
    }
}
