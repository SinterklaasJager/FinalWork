using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerKillerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonContainer, pressedButton, confirmationButtonContainer, playerListContainer;
    [SerializeField]
    private Button btnStartKilling, btnIAmSure, btnIAmNotSure;
    public UIManager uIManager;
    private GameObject gameManager, btnPlayer;
    private List<Player> players;
    private Player selectedPlayer;

    public Enums.EventHandlers Events;

    public void SpawnButtons(GameManager gameManager, List<string> playerNames, List<int> playerIDs, int currentPlayerID)
    {
        btnPlayer = gameManager.gameObject.GetComponent<SpawnableObjects>().btnPickAPlayerToKill;
        var i = 0;
        foreach (var player in gameManager.syncedPlayers)
        {
            if (playerIDs[i] != currentPlayerID)
            {
                var btnObject = Instantiate(btnPlayer, buttonContainer.transform);
                btnObject.GetComponent<PickAPlayerToKillBtnScript>().SetPlayer(player, gameObject, playerNames[i]);
            }
            i++;
        }
    }

    public void OnStartTheKilling()
    {
        btnStartKilling.gameObject.SetActive(false);
        playerListContainer.SetActive(true);
    }

    private void OnPlayerSelected()
    {
        playerListContainer.SetActive(false);
        confirmationButtonContainer.SetActive(true);
    }

    public void OnChoiceMade(bool kill)
    {
        if (kill)
        {
            Events.onPlayerToKillPicked?.Invoke(selectedPlayer);
            Destroy(gameObject);
        }
        else
        {
            playerListContainer.SetActive(true);
            confirmationButtonContainer.SetActive(false);
        }
    }

    public void OnButtonClick(GameObject btn)
    {
        selectedPlayer = btn.GetComponent<PickAPlayerToKillBtnScript>().GetSelectedPlayer();
        OnPlayerSelected();
    }
}
