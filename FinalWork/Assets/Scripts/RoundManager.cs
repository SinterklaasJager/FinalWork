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
    [SyncVar] private bool electionFailed = false;

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

    // [Command]
    [Server]
    public void RoundSetUp(GameManager gm, GameObject um)
    {
        Debug.Log("Round Setup");
        gameManager = gm;
        uIManagerObj = um;
        uIManager = uIManagerObj.GetComponent<UIManager>();
        turn = 0;
        round = 0;
        //  ClientSideRoundSetUp();
        voteForOrganisers = gameManager.voteForOrganisers;
        // InstantiateRoundUI();

        // foreach (var player in gameManager.syncedPlayers)
        // {
        //     if (player == NetworkClient.localPlayer.gameObject.GetComponent<PlayerManager>().GetPlayerClass())
        //     {
        //         connectedClientObj = NetworkClient.localPlayer.gameObject;
        //         connectedClient = player;
        //         Debug.Log("connected client: " + connectedClient);
        //     }
        // }
        StartTurn();
    }
    private void CheckIfDead()
    {
        Debug.Log("Check If Dead");

        while (gameManager.syncedPlayers[turn].GetIsDead())
        {
            Debug.Log("Current Player is dead");

            turn++;

            if (turn >= gameManager.syncedPlayers.Count)
            {
                turn = 0;
                round++;
            }
        }
    }

    //[Command]
    public void StartTurn()
    {
        Debug.Log("Start Turn");
        CheckIfDead();
        Debug.Log("Continue Turn");

        //the player whose turn it is.
        currentPlayer = gameManager.syncedPlayers[turn];
        currentPlayer.SetIsTeamLeaderCandidate(true);

        //UpdateUI();
        SetUpAssistantPicker();
    }

    // [Command]
    private void SetUpAssistantPicker()
    {
        List<string> playerNames = new List<string>();
        List<int> playerIDs = new List<int>();
        List<int> deathPlayerIDs = new List<int>();

        foreach (var player in gameManager.syncedPlayers)
        {
            playerNames.Add(player.GetName());
            playerIDs.Add(player.GetPlayerID());
            if (player.GetIsDead())
            {
                deathPlayerIDs.Add(player.GetPlayerID());
            }
        }

        var currentPlayerID = currentPlayer.GetPlayerID();
        var prevPlayerID = 000;
        var prevAssistantID = 000;

        if (previousPlayer != null)
        {
            Debug.Log("prevplayer is not null");
            prevPlayerID = previousPlayer.GetPlayerID();
            prevAssistantID = previousAssistant.GetPlayerID();
        }

        foreach (var playerObj in gameManager.syncedPlayerObjects)
        {
            var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
            Debug.Log(player.GetName());

            if (player == currentPlayer)
            {
                Debug.Log("targetrpc player: " + player.GetName());
                Debug.Log(uIManager);
                StartAssistantPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient, gameManager.spawnableObjects.PickAnAssistantUI, uIManager, currentPlayer, gameManager, playerNames, playerIDs, currentPlayerID, prevPlayerID, prevAssistantID, deathPlayerIDs);
            }
        }
    }

    [TargetRpc]
    private void StartAssistantPicker(NetworkConnection target, GameObject pickAnAssistantUIObj, UIManager um, Player currentPlayer, GameManager gm, List<string> playerNames, List<int> playerIDs, int currentPlayerID, int prevPlayerID, int prevAssistantID, List<int> deathPlayerIDs)
    {
        //PickAnAssistant = um.StartPickAnAssistantUI(currentPlayer);
        Debug.Log("UM:" + um);
        Debug.Log(um.PickAnAssistantUIObj);
        Debug.Log(pickAnAssistantUIObj);
        PickAnAssistant = Instantiate(um.PickAnAssistantUIObj, um.gameObject.transform);
        Debug.Log("pickanassitantuiObj" + PickAnAssistant);
        //  PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPicked = (assistantCandidate) => OnAssistantPicked(assistantCandidate);
        PickAnAssistant.GetComponent<PickAnAssistantUI>().Events.onAssistantPickedInt = (assistantCandidate) => OnAssistantPickedID(assistantCandidate);
        PickAnAssistant.GetComponent<PickAnAssistantUI>().SpawnButtons(gm, playerNames, playerIDs, currentPlayerID, prevPlayerID, prevAssistantID, deathPlayerIDs);
    }
    // public void OnAssistantPicked(Player assistantCandidate)
    // {
    //     var playerID = assistantCandidate.GetPlayerID();
    //     //start vote round
    //     SetAssistantCandidate(playerID);
    //     cmdStartVotingRound();
    // }
    public void OnAssistantPickedID(int assistantCandidate)
    {
        var playerID = assistantCandidate;
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
        uIManager.DestroyElectionFailedUI();

        var totalVoters = 0;
        foreach (var player in gameManager.syncedPlayers)
        {
            if (!player.GetIsDead())
            {
                totalVoters++;
            }
        }
        List<int> deathPlayerIDs = new List<int>();

        foreach (var player in gameManager.syncedPlayers)
        {
            if (player.GetIsDead())
            {
                deathPlayerIDs.Add(player.GetPlayerID());
            }
        }

        voteForOrganisers.EventHandlers.OnVoteEnd = (succes) => OnVoteEnd(succes);
        voteForOrganisers.StartNewVotingRound(uIManager, gameManager.syncedPlayers, assistantCandidate, currentPlayer, totalVoters, deathPlayerIDs);
    }
    [Server]
    public void OnVoteEnd(bool passed)
    {
        Debug.Log("On Vote End");

        assistantCandidate.SetIsAssistantCandidate(false);

        if (passed)
        {
            Debug.Log("Election Succes!");
            failedElections = 0;
            currentAssistant = assistantCandidate;

            foreach (var playerObj in gameManager.syncedPlayerObjects)
            {
                Debug.Log("on vote end player obj list: " + gameManager.syncedPlayerObjects);
                var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
                if (player == currentAssistant)
                {
                    Debug.Log("player assistant role: " + player.GetRole());
                    Debug.Log("gameManager.victoryProgress.GetBadPoints: " + gameManager.victoryProgress.GetBadPoints());
                    if (player.GetRole() == 2 && gameManager.victoryProgress.GetBadPoints() >= 3)
                    {
                        Debug.Log("The Saboteur is in Place!");
                        gameManager.victoryProgress.SaboteurElectedAssistant();
                    }
                    else
                    {
                        Debug.Log("targetrpc player: " + player.GetName());
                        StartAssistantCardPicker(playerObj.GetComponent<NetworkIdentity>().connectionToClient, uIManager);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Election Failed!");
            failedElections++;

            uIManager.InstantiateElectionFailedUI(failedElections);

            if (failedElections > 2)
            {
                ElectionFailed();
            }
            else
            {
                // StartTurn();
                SetUpAssistantPicker();
            }

        }
    }
    // [Command(requiresAuthority = false)]
    public void ElectionFailed()
    {
        uIManager.DestroyElectionFailedUI();
        electionFailed = true;
        failedElections = 0;
        //play random card
        Debug.Log("Play random card");
        var randomCard = gameManager.cardGeneration.GetTopCard();
        Debug.Log("random card: " + randomCard);
        CardPicked(randomCard);
        //  EndTurn();
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
        cardDealer.eventHandlers.OnCardSelected = (selectedCard) => cmdCardPicked(selectedCard);
    }


    [Command(requiresAuthority = false)]
    public void cmdCardPicked(Enums.CardType selectedCard)
    {
        gameManager.victoryProgress.SetPoints(selectedCard);
    }

    public void CardPicked(Enums.CardType selectedCard)
    {
        Debug.Log("Selected Card: " + selectedCard);
        gameManager.victoryProgress.SetPoints(selectedCard);
    }
    //[ClientRpc]
    // [Command(requiresAuthority = false)]
    public void EndTurn()
    {
        Debug.Log("Good boi Points: " + gameManager.victoryProgress.GetGoodPoints());
        Debug.Log("Bad boi Points: " + gameManager.victoryProgress.GetBadPoints());

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
        currentPlayer.SetIsTeamLeader(false);
        previousPlayer = currentPlayer;
        Debug.Log("electionFailed: " + electionFailed);
        if (!electionFailed)
        {
            currentAssistant.SetWasAssistant(true);
            previousAssistant = currentAssistant;
        }
        else
        {
            previousAssistant = null;
            previousPlayer = null;
        }
        electionFailed = false;
        StartTurn();
    }

    // [Command(requiresAuthority = false)]
    public void SetUpDeathPicker()
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

            if (player == currentPlayer)
            {
                Debug.Log("targetrpc player that gets to do the killing: " + player.GetName());

                TargetKillAPlayerKiller(playerObj.GetComponent<NetworkIdentity>().connectionToClient, uIManager, currentPlayer, gameManager, playerNames, playerIDs, currentPlayerID);
            }
        }
    }

    [TargetRpc]
    public void TargetKillAPlayerKiller(NetworkConnection target, UIManager um, Player currentPlayer, GameManager gm, List<string> playerNames, List<int> playerIDs, int currentPlayerID)
    {
        PickAnAssistant = Instantiate(gm.spawnableObjects.KillAPlayerUI, um.gameObject.transform);
        PickAnAssistant.GetComponent<PlayerKillerUI>().Events.onPlayerToKillPicked = (playerToKill) => OnTargetToKillSelected(playerToKill);
        PickAnAssistant.GetComponent<PlayerKillerUI>().SpawnButtons(gm, playerNames, playerIDs, currentPlayerID);
    }

    private void OnTargetToKillSelected(int playerToKill)
    {
        var playerID = playerToKill;
        //start vote round
        cmdKillAPlayer(playerID);
    }


    [Command(requiresAuthority = false)]
    public void cmdKillAPlayer(int playerToKillID)
    {
        Player playerToKill = null;

        foreach (var player in gameManager.syncedPlayers)
        {
            if (player.GetPlayerID() == playerToKillID)
            {
                playerToKill = player;
            }
        }

        playerToKill.SetIsDead(true);
        gameManager.deathPlayerIds.Add(playerToKillID);
        Debug.Log("playerToKill: " + playerToKill.GetName());

        if (!CheckEndPrematurely(playerToKill))
        {
            foreach (var playerObj in gameManager.syncedPlayerObjects)
            {
                var player = playerObj.GetComponent<PlayerManager>().GetPlayerClass();
                if (player == playerToKill)
                {
                    TargetKillAPlayerVictim(playerObj.GetComponent<NetworkIdentity>().connectionToClient, player, gameManager, uIManager);
                }
            }

            EndTurn();
        }
        else
        {
            Debug.Log("END PREMATURELY");
        }
    }

    private bool CheckEndPrematurely(Player playerToKill)
    {
        var endQuicker = false;
        var aliveGoodGuys = 0;
        var aliveDeadGuys = 0;

        if (playerToKill.GetRole() == 2)
        {
            //KILLED SABOTEUR
            Debug.Log("SABOTEUR FOUND DEAD");
            gameManager.victoryProgress.GoodGuysWin(Enums.GameEndReason.saboteurDead);
            endQuicker = true;
        }

        //Check if one team is completely wiped out
        foreach (var player in gameManager.syncedPlayers)
        {
            if (!player.GetIsDead())
            {
                if (player.GetRole() == 0)
                {
                    aliveGoodGuys++;
                }
                else
                {
                    aliveDeadGuys++;
                }
            }
        }

        if (aliveGoodGuys == 0)
        {
            gameManager.victoryProgress.BadGuysWin(Enums.GameEndReason.goodGuysDead);
            endQuicker = true;
        }
        if (aliveDeadGuys == 0)
        {
            gameManager.victoryProgress.GoodGuysWin(Enums.GameEndReason.rebelsDead);
            endQuicker = true;
        }

        return endQuicker;
    }

    [TargetRpc]
    public void TargetKillAPlayerVictim(NetworkConnection conn, Player player, GameManager gm, UIManager um)
    {
        //do kill player visuals
        Debug.Log("You Have Died");
        Debug.Log(um);
        Instantiate(gm.spawnableObjects.YouAreDeadUI, um.gameObject.transform);
    }

    [Server]
    public void StopGame()
    {
        NetworkServer.Destroy(gameObject);
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
