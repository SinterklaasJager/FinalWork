using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int turn = 0, round = 0, failedElections = 0;
    public List<Player> players;
    private Player previousPlayer, currentPlayer, previousAssistant, currentAssistant, assistantCanditate;
    private GameObject PickAnAssistant;
    private VoteForOrganisers voteForOrganisers;
    private CardDealerUI cardDealer;
    public UIManager uIManager;
    public void StartTurn()
    {
        currentPlayer = players[turn];
        currentPlayer.SetIsTeamLeaderCandidate(true);
        UpdateUI();
        PickAnAssistant = uIManager.StartPickAnAssistantUI();
        PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
        //uIManager.GetComponent<CardDealerUI>().ShowAssistantCards();
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
                voteForOrganisers.StartNewVotingRound(uIManager, players);
            }

        }
    }

    public void ElectionFailed()
    {
        failedElections = 0;
        //play random card
        CardPicked(Enums.CardType.bad);
        EndTurn();
    }

    public void CardPicked(Enums.CardType selectedCard)
    {
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
        previousAssistant = currentAssistant;
        previousPlayer.SetIsTeamLeader(false);
        previousAssistant.SetWasAssistant(true);

    }

    private void UpdateUI()
    {
        var roundUI = uIManager.RoundUI.GetComponent<RoundUIHandler>();
        roundUI.UpdateRoundText(round);
        roundUI.UpdateCurrentPlayerText(players[turn].GetName());
    }

}
