using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    [SyncVar] public int turn = 0, round = 0, failedElections = 0;
    public SyncList<Player> players = new SyncList<Player>();
    [SyncVar] private Player previousPlayer, previousAssistant, currentAssistant, assistantCanditate;
    [SyncVar(hook = nameof(SyncVarTest))] private Player currentPlayer;
    private Player ConnectedClient;
    private GameObject PickAnAssistant, ConnectedClientObj;
    private GameManager gameManager;
    private VoteForOrganisers voteForOrganisers;
    private CardDealerUI cardDealer;
    private RoundUIHandler roundUI;
    public UIManager uIManager;
    private bool electionFailed;

    private void SyncVarTest(Player oldPlayer, Player newPlayer)
    {
        Debug.Log("|Sync Var Hook| current player: " + newPlayer.GetName());
    }
    public void RoundSetUp(GameManager gm, UIManager um)
    {
        Debug.Log("Round Setup");
        gameManager = gm;
        uIManager = um;
        this.players = gameManager.syncedPlayers;
        voteForOrganisers = gameManager.voteForOrganisers;
        turn = 0;

        InstantiateRoundUI();

        foreach (var player in this.players)
        {
            if (player == NetworkClient.localPlayer.gameObject.GetComponent<PlayerManager>().GetPlayerClass())
            {
                ConnectedClientObj = NetworkClient.localPlayer.gameObject;
                ConnectedClient = player;

            }
        }

        StartTurn();
    }

    public void StartTurn()
    {
        Debug.Log("Start Turn");
        currentPlayer = players[turn];
        currentPlayer.SetIsTeamLeaderCandidate(true);
        UpdateUI();
        Debug.Log("currentPlayer: " + currentPlayer.GetPlayerID());
        Debug.Log("localPlayerID: " + NetworkClient.localPlayer.netId);
        /*
               if (currentPlayer == ConnectedClient)
               {
                   PickAnAssistant = uIManager.StartPickAnAssistantUI();
                   PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
               }
                foreach (var playerObj in gameManager.syncedPlayerObjects)
                 {
                     var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
                     if (player == currentPlayer)
                     {
                         Debug.Log("targetrpc player: " + player.GetName());
                         StartAssistantPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient);
                     }
                 }*/
    }

    //[TargetRpc]
    /*
    private void StartAssistantPicker(NetworkConnection target)
    {
        PickAnAssistant = uIManager.StartPickAnAssistantUI();
        PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
    }
    */
    public void OnAssistantPicked(Player assistantCandidate)
    {
        this.assistantCanditate = assistantCandidate;
        //start vote round
        cmdStartVotingRound();
    }

    [Command(requiresAuthority = false)]
    public void cmdStartVotingRound()
    {
        voteForOrganisers.EventHandlers.OnVoteEnd = (succes) => OnVoteEnd(succes);
        voteForOrganisers.StartNewVotingRound(uIManager, players, assistantCanditate, currentPlayer);
    }

    // [ClientRpc]
    public void OnVoteEnd(bool passed)
    {
        Debug.Log("On Vote End");

        if (passed)
        {
            Debug.Log("Election Succes!");
            failedElections = 0;
            currentAssistant = assistantCanditate;

            foreach (var playerObj in gameManager.syncedPlayerObjects)
            {
                var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
                if (player == currentAssistant)
                {
                    Debug.Log("targetrpc player: " + player.GetName());
                    StartAssistantCardPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient);
                }
            }
            //start Card picker
            /* if (currentAssistant == NetworkClient.localPlayer.gameObject.GetComponent<PlayerManager>().GetPlayerClass())
             {
                 Debug.Log("Start Current Assistan Card Draw");
                 StartAssistantCardPicker();
             }
             */
        }
        else
        {
            Debug.Log("Election Failed!");
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

    [TargetRpc]
    private void StartAssistantCardPicker(NetworkConnection target)
    {
        cardDealer = uIManager.StartAssistantCardDrawUI();
        cardDealer.eventHandlers.OnAssistantCardsPicked = (pickedCards) => AssistantCardsPicked(pickedCards);
    }

    [TargetRpc]
    public void targetGiveCardsToTeamLeader(NetworkConnection target, List<Enums.CardType> pickedCards)
    {
        Debug.Log("Give Cards To TeamLeader");
        cardDealer.ShowProjectManagerCards(pickedCards);
        cardDealer.eventHandlers.OnCardSelected = (selectedCard) => CardPicked(selectedCard);
    }
    public void AssistantCardsPicked(List<Enums.CardType> pickedCards)
    {
        Debug.Log("Assistant Cards Picked");
        foreach (var playerObj in gameManager.syncedPlayerObjects)
        {

            var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
            if (player == currentPlayer)
            {
                targetGiveCardsToTeamLeader(playerObj.GetComponent<NetworkIdentity>().connectionToClient, pickedCards);
            }
        }
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

    [ClientRpc]
    private void InstantiateRoundUI()
    {
        uIManager.IniateRoundUI(gameObject.GetComponent<RoundManager>());
    }
    [ClientRpc]
    private void UpdateUI()
    {
        Debug.Log("round UI:" + roundUI);
        roundUI.UpdateRoundText(round);
        roundUI.UpdateCurrentPlayerText(players[turn].GetName());
    }

    public void SetRoundUI(GameObject roundui)
    {
        this.roundUI = roundui.GetComponent<RoundUIHandler>();
    }

}
