using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public int turn = 0, round = 0, failedElections = 0;
    public List<Player> players;
    private Player previousPlayer, currentPlayer, previousAssistant, currentAssistant, assistantCanditate;
    private GameObject PickAnAssistant;
    private GameManager gameManager;
    private VoteForOrganisers voteForOrganisers;
    private CardDealerUI cardDealer;
    public UIManager uIManager;
    private bool electionFailed;

    public void RoundSetUp(GameManager gm, UIManager um)
    {
        gameManager = gm;
        uIManager = um;
        voteForOrganisers = gameManager.voteForOrganisers;

        StartTurn();
    }

    public void StartTurn()
    {
        currentPlayer = players[turn];
        currentPlayer.SetIsTeamLeaderCandidate(true);
        UpdateUI();
        Debug.Log("currentPlayer: " + currentPlayer.GetPlayerID());
        Debug.Log("localPlayerID: " + ClientScene.localPlayer.netId);
        if (currentPlayer.GetPlayerID() == ClientScene.localPlayer.netId)
        {
            PickAnAssistant = uIManager.StartPickAnAssistantUI();
            PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
        }

    }

    public void OnAssistantPicked(Player assistantCandidate)
    {
        this.assistantCanditate = assistantCandidate;
        //start vote round
        voteForOrganisers.EventHandlers.OnVoteEnd = (succes) => OnVoteEnd(succes);
        voteForOrganisers.StartNewVotingRound(uIManager, players);
    }

    public void OnVoteEnd(bool passed)
    {
        if (passed)
        {
            failedElections = 0;
            currentAssistant = assistantCanditate;

            //start Card picker
            cardDealer = uIManager.StartAssistantCardDrawUI();
            cardDealer.eventHandlers.OnCardSelected = (selectedCard) => CardPicked(selectedCard);
        }
        else
        {
            failedElections++;
            if (failedElections > 2)
            {
                ElectionFailed();
            }
            else
            {
                StartTurn();
            }

        }
    }

    public void ElectionFailed()
    {
        failedElections = 0;
        //play random card
        var randomCard = gameManager.cardGeneration.GetTopCard();
        CardPicked(randomCard);
        EndTurn();
    }

    public void CardPicked(Enums.CardType selectedCard)
    {
        gameManager.victoryProgress.SetPoints(selectedCard);
        //Trigger card choice animation
        //
    }
    public void EndTurn()
    {
        turn++;
        if (turn > players.Count)
        {
            turn = 0;
            round++;
        }

        if (previousAssistant != null)
        {
            previousAssistant.SetWasAssistant(false);
        }
        previousPlayer = currentPlayer;
        previousPlayer.SetIsTeamLeader(false);
        if (!electionFailed)
        {
            previousAssistant = currentAssistant;
            previousAssistant.SetWasAssistant(true);
        }


    }

    private void UpdateUI()
    {
        var roundUI = uIManager.RoundUI.GetComponent<RoundUIHandler>();
        roundUI.UpdateRoundText(round);
        roundUI.UpdateCurrentPlayerText(players[turn].GetName());
    }

}
