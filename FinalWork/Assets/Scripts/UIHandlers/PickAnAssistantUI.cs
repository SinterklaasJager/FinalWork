using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickAnAssistantUI : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonContainer, pressedButton;
    public UIManager uIManager;
    private GameObject gameManager, btnPlayer;
    private List<Player> players;

    public Enums.EventHandlers Events;

    public void SetUiManager(GameObject uIManager, GameObject gameManager, Player currentPlayer)
    {
        this.uIManager = uIManager.GetComponent<UIManager>();
        this.gameManager = gameManager;
        btnPlayer = gameManager.GetComponent<SpawnableObjects>().btnPickAnAssistant;
        players = gameManager.GetComponent<GameManager>().GetPlayers();

        //  SpawnButtons(currentPlayer, gameManager.GetComponent<GameManager>());
    }

    public void SpawnButtons(GameManager gameManager, List<string> playerNames, List<int> playerIDs, int currentPlayerID, int prevPlayerID, int prevAssistantID)
    {
        btnPlayer = gameManager.gameObject.GetComponent<SpawnableObjects>().btnPickAnAssistant;
        var i = 0;
        foreach (var player in gameManager.syncedPlayers)
        {
            //if not player == currentplayer
            //else setteamleadercandidate(true)
            if (playerIDs[i] != currentPlayerID)
            {
                var btnObject = Instantiate(btnPlayer, buttonContainer.transform);
                btnObject.GetComponent<PickAssistantBtnScript>().SetPlayer(player, gameObject, playerNames[i], playerIDs[i]);
                //disable Previous councilors
                if (prevPlayerID != 000)
                {
                    if (gameManager.syncedPlayers.Count - gameManager.deathPlayerIds.Count > 5)
                    {
                        if (playerIDs[i] == prevPlayerID)
                        {
                            btnObject.GetComponent<Button>().interactable = false;
                        }
                    }
                    if (playerIDs[i] == prevAssistantID)
                    {
                        btnObject.GetComponent<Button>().interactable = false;
                    }
                }
                //Disable Death Players
                if (gameManager.deathPlayerIds.Count > 0)
                {
                    foreach (var deathPlayer in gameManager.deathPlayerIds)
                    {
                        if (playerIDs[i] == deathPlayer)
                        {
                            Debug.Log("Stop Interactable button");
                            btnObject.GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
            i++;
        }
    }
    public void OnButtonClick(GameObject btn)
    {
        var player = btn.GetComponent<PickAssistantBtnScript>().GetSelectedPlayer();
        var playerID = btn.GetComponent<PickAssistantBtnScript>().GetPlayerID();

        player.SetIsAssistantCandidate(true);
        //transition to voting ui
        // Events.onAssistantPicked?.Invoke(player);
        Events.onAssistantPickedInt(playerID);
        Destroy(gameObject);
    }
}
