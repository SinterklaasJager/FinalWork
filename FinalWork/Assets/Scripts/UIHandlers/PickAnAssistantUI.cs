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

    public void SetUiManager(GameObject uIManager, GameObject gameManager)
    {
        this.uIManager = uIManager.GetComponent<UIManager>();
        this.gameManager = gameManager;
        btnPlayer = gameManager.GetComponent<SpawnableObjects>().btnPickAnAssistant;
        players = gameManager.GetComponent<GameManager>().GetPlayers();

        SpawnButtons();
    }

    public void SpawnButtons()
    {
        foreach (var player in players)
        {
            //if not player == currentplayer
            //else setteamleadercandidate(true)
            if (!player.GetIsTeamLeaderCandidate())
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
