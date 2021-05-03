using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    [SyncVar] public int turn = 0, round = 0, failedElections = 0;
    [SyncVar] private Player previousPlayer, previousAssistant, currentAssistant, assistantCandidate;
    [SyncVar(hook = nameof(CurrentPlayerHook))] private Player currentPlayer;
    private Player connectedClient;
    private GameObject PickAnAssistant;
    private GameObject connectedClientObj;
    [SyncVar(hook = nameof(gameManagerHook))] private GameManager gameManager;
    [SyncVar] private VoteForOrganisers voteForOrganisers;
    private CardDealerUI cardDealer;
    private RoundUIHandler roundUI;
    public UIManager uIManager;
    [SyncVar] private GameObject uIManagerObj;
    private bool electionFailed;

    private void gameManagerHook(GameManager oldGm, GameManager newGm)
    {
        uIManager = newGm.uIManager;
        if (gameManager == null)
        {
            gameManager = newGm;
        }
    }

    private void CurrentPlayerHook(Player oldPlayer, Player newPlayer)
    {
        Debug.Log("|Sync Var Hook| current player: " + newPlayer.GetName());
        if (currentPlayer != newPlayer)
        {
            currentPlayer = newPlayer;
        }
        /* if (roundUI != null)
         {
             UpdateUI();
         }
         */
    }

    [Command]
    public void RoundSetUp(GameManager gm, GameObject um)
    {
        Debug.Log("Round Setup");
        gameManager = gm;
        uIManagerObj = um;
        turn = 0;
        round = 0;
        //  ClientSideRoundSetUp();
        voteForOrganisers = gameManager.voteForOrganisers;
        // InstantiateRoundUI();

        foreach (var player in gameManager.syncedPlayers)
        {
            if (player == NetworkClient.localPlayer.gameObject.GetComponent<PlayerManager>().GetPlayerClass())
            {
                connectedClientObj = NetworkClient.localPlayer.gameObject;
                connectedClient = player;
                Debug.Log("connected client: " + connectedClient);
            }
        }

        StartTurn();
    }

    [Command]
    public void StartTurn()
    {
        Debug.Log("Start Turn");

        //the player whose turn it is.
        currentPlayer = gameManager.syncedPlayers[turn];
        currentPlayer.SetIsTeamLeaderCandidate(true);

        //UpdateUI();
        SetUpAssistantPicker();
        /* 
             if (currentPlayer == ConnectedClient)
             {
                 PickAnAssistant = uIManager.StartPickAnAssistantUI();
                 PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
             }

              */
    }
    //[Command]
    private void SetUpAssistantPicker()
    {
        List<string> playerNames = new List<string>();
        List<int> playerIDs = new List<int>();

        foreach (var player in gameManager.syncedPlayers)
        {
            playerNames.Add(player.GetName());
            playerIDs.Add(player.GetPlayerID());
        }

        var currentPlayerID = currentPlayer.GetPlayerID();
        foreach (var playerObj in gameManager.syncedPlayerObjects)
        {
            var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
            Debug.Log(player.GetName());

            if (player == currentPlayer)
            {
                Debug.Log("targetrpc player: " + player.GetName());
                Debug.Log(uIManager);


                StartAssistantPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient, uIManager, currentPlayer, gameManager, playerNames, playerIDs, currentPlayerID);
            }
        }
    }

    [TargetRpc]
    private void StartAssistantPicker(NetworkConnection target, UIManager um, Player currentPlayer, GameManager gm, List<string> playerNames, List<int> playerIDs, int currentPlayerID)
    {
        //PickAnAssistant = um.StartPickAnAssistantUI(currentPlayer);
        PickAnAssistant = Instantiate(um.PickAnAssistantUIObj, um.gameObject.transform);
        PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
        PickAnAssistant.GetComponent<PickAnAssistantUI>().SpawnButtons(gm, playerNames, playerIDs, currentPlayerID);
    }
    public void OnAssistantPicked(Player assistantCandidate)
    {
        var playerID = assistantCandidate.GetPlayerID();
        //start vote round
        SetAssistantCandidate(playerID);
        cmdStartVotingRound();

    }
    [Command(requiresAuthority = false)]
    private void SetAssistantCandidate(int playerid)
    {
        foreach (var player in gameManager.syncedPlayers)
        {
            if (player.GetPlayerID() == playerid)
            {
                assistantCandidate = player;
                player.SetIsAssistantCandidate(true);
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void cmdStartVotingRound()
    {
        Debug.Log("assistantCandidate: " + assistantCandidate.GetName());

        voteForOrganisers.EventHandlers.OnVoteEnd = (succes) => OnVoteEnd(succes);
        voteForOrganisers.StartNewVotingRound(uIManager, gameManager.syncedPlayers, assistantCandidate, currentPlayer);
    }

    // [ClientRpc]
    [Command(requiresAuthority = false)]
    public void OnVoteEnd(bool passed)
    {
        Debug.Log("On Vote End");
        assistantCandidate.SetIsAssistantCandidate(false);

        if (passed)
        {
            Debug.Log("Election Succes!");
            failedElections = 0;
            currentAssistant = assistantCandidate;

            Debug.Log("currentAssistant: " + currentAssistant.GetPlayerID());
            Debug.Log("currentAssistant: " + currentAssistant.GetName());
            Debug.Log("currentPlayer: " + currentPlayer.GetPlayerID());
            Debug.Log("currentPlayer: " + currentPlayer.GetName());

            foreach (var playerObj in gameManager.syncedPlayerObjects)
            {
                Debug.Log("on vote end player obj list: " + gameManager.syncedPlayerObjects);
                var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
                if (player == currentAssistant)
                {
                    Debug.Log("targetrpc player: " + player.GetName());
                    StartAssistantCardPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient, uIManager);
                }
            }
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
    private void StartAssistantCardPicker(NetworkConnection target, UIManager um)
    {
        Debug.Log(um);
        um.StartAssistantCardDrawUI(gameManager.gameObject);
        cardDealer = um.gameObject.GetComponent<CardDealerUI>();
        cardDealer.eventHandlers.OnAssistantCardsPicked = (pickedCards) => AssistantCardsPicked(pickedCards);
    }

    [Command(requiresAuthority = false)]
    public void AssistantCardsPicked(List<Enums.CardType> pickedCards)
    {
        Debug.Log("Assistant Cards Picked");

        foreach (var playerObj in gameManager.syncedPlayerObjects)
        {

            var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
            if (player == currentPlayer)
            {
                targetGiveCardsToTeamLeader(playerObj.GetComponent<NetworkIdentity>().connectionToClient, pickedCards, uIManager);
            }
        }
    }

    [TargetRpc]
    public void targetGiveCardsToTeamLeader(NetworkConnection target, List<Enums.CardType> pickedCards, UIManager um)
    {
        Debug.Log("Give Cards To TeamLeader");
        cardDealer = um.StartTeamLeaderCardDrawUI(pickedCards);
        //cardDealer.ShowProjectManagerCards(pickedCards);
        cardDealer.eventHandlers.OnCardSelected = (selectedCard) => CardPicked(selectedCard);
    }


    [Command(requiresAuthority = false)]
    public void CardPicked(Enums.CardType selectedCard)
    {
        gameManager.victoryProgress.SetPoints(selectedCard);
        //Trigger card choice animation
        //
        EndTurn();
    }
    //[ClientRpc]
    public void EndTurn()
    {
        turn++;
        if (turn >= gameManager.syncedPlayers.Count)
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

        StartTurn();
    }
    [Command(requiresAuthority = false)]
    private void InstantiateRoundUI()
    {
        Debug.Log("uIManager: " + uIManager);
        gameManager.universalCanvas.IniateRoundUI(gameObject.GetComponent<RoundManager>());

    }

    private void UpdateUI()
    {
        //make a shared ui canvas?
        Debug.Log("round UI:" + roundUI);
        roundUI = uIManager.RoundUI.GetComponent<RoundUIHandler>();
        roundUI.UpdateRoundText(round);
        roundUI.UpdateCurrentPlayerText(currentPlayer.GetName());
    }

    [ClientRpc]
    public void SetRoundUI(GameObject roundui)
    {
        Debug.Log("set rount ui: " + roundui);
        this.roundUI = roundui.GetComponent<RoundUIHandler>();
    }

}
