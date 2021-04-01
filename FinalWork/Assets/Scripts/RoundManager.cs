using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int turn = 0, round = 0;
    public List<Player> players;
    private Player previousPlayer, currentPlayer;
    public UIManager uIManager;
    public void StartTurn()
    {
        currentPlayer = players[turn];
        currentPlayer.SetIsTeamLeader(true);
        UpdateUI();
    }

    public void EndTurn()
    {
        turn++;
        if (turn > players.Count)
        {
            turn = 0;
            round++;
        }

        previousPlayer.SetWasTeamLeader(false);
        previousPlayer = currentPlayer;
        previousPlayer.SetIsTeamLeader(false);
        previousPlayer.SetWasTeamLeader(true);
    }

    private void UpdateUI()
    {
        Debug.Log("Update RoundsUI");
        var roundUI = uIManager.RoundUI.GetComponent<RoundUIHandler>();
        roundUI.UpdateRoundText(round);
        roundUI.UpdateCurrentPlayerText(players[turn].GetName());
    }

}
