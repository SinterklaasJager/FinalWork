using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PickAnAssistantUI : NetworkBehaviour
{
    [SerializeField]
    private GameObject buttonContainer, pressedButton;
    public UIManager uIManager;
    private GameObject gameManager, btnPlayer;
    private List<Player> players;

    public Enums.EventHandlers Events;

    [Command(requiresAuthority = false)]
    public void SetUiManager(GameObject uIManager, GameObject gameManager, Player currentPlayer)
    {
        this.uIManager = uIManager.GetComponent<UIManager>();
        this.gameManager = gameManager;
        btnPlayer = gameManager.GetComponent<SpawnableObjects>().btnPickAnAssistant;
        players = gameManager.GetComponent<GameManager>().GetPlayers();

        //  SpawnButtons(currentPlayer, gameManager.GetComponent<GameManager>());
    }

    public void SpawnButtons(Player currentPlayer, GameManager gameManager)
    {
        Debug.Log("spawnableObjects: " + gameManager.spawnableObjects);
        Debug.Log("btnPickAnAssistant: " + gameManager.spawnableObjects.btnPickAnAssistant);
        btnPlayer = gameManager.gameObject.GetComponent<SpawnableObjects>().btnPickAnAssistant;
        foreach (var player in gameManager.syncedPlayers)
        {
            Debug.Log("players pick assistant: " + player.GetName());
            //if not player == currentplayer
            //else setteamleadercandidate(true)
            if (player.GetPlayerID() != currentPlayer.GetPlayerID())
            {
                var btnObject = Instantiate(btnPlayer, buttonContainer.transform);
                btnObject.GetComponent<PickAssistantBtnScript>().SetPlayer(player, gameObject);
            }
        }
    }

    public void OnButtonClick(GameObject btn)
    {
        var player = btn.GetComponent<PickAssistantBtnScript>().GetSelectedPlayer();
        player.SetIsAssistantCandidate(true);
        //transition to voting ui
        Events.onAssistantPicked?.Invoke(player);
        Destroy(gameObject);
    }


}
