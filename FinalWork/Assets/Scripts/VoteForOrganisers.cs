using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class VoteForOrganisers : NetworkBehaviour
{
    [SyncVar] private int totalVotesCast, yesVotes, totalVoters;
    private GameManager gameManager;
    private UIManager uiManager;
    private SyncList<Player> players = new SyncList<Player>();
    private Player teamLeader, assistant, currentPlayer;
    private bool isDead = false;

    public Enums.EventHandlers EventHandlers;

    public void StartNewVotingRound(UIManager um, SyncList<Player> pl, Player assistantCandidate, Player leaderCandidate)
    {
        assistant = assistantCandidate;
        teamLeader = leaderCandidate;
        currentPlayer = NetworkClient.localPlayer.gameObject.GetComponent<PlayerManager>().GetPlayerClass();
        gameManager = gameObject.GetComponent<GameManager>();
        uiManager = um;
        players = pl;

        foreach (var player in players)
        {
            if (!player.GetIsDead())
            {
                totalVoters++;
            }
        }

        if (!isDead)
        {
            CheckIfDead(currentPlayer);
            if (!isDead)
            {
                AddVoter();

                uiManager.StartLeaderVotingUI(teamLeader.GetName(), assistant.GetName(), gameObject);
            }
        }


    }

    private void CheckIfDead(Player player)
    {
        foreach (var id in gameManager.deathPlayerIds)
        {
            if (player.GetPlayerID() == id)
            {
                isDead = true;
            }
        }

    }

    [Command(requiresAuthority = false)]
    private void AddVoter()
    {
        //totalVoters++;

        Debug.Log("totalvoters: " + totalVoters);
    }
    [Command(requiresAuthority = false)]
    public void AddVote(bool vote)
    {
        totalVotesCast++;
        Debug.Log(totalVotesCast);
        if (vote)
        {
            yesVotes++;
        }
        if (totalVotesCast == totalVoters)
        {
            var voteSucceeds = false;
            if (yesVotes >= totalVotesCast / 2)
            {
                voteSucceeds = true;
            }


            FinishVotingRound(voteSucceeds);
        }
    }

    public void FinishVotingRound(bool voteSucceeds)
    {
        totalVotesCast = 0;
        yesVotes = 0;
        totalVoters = 0;

        if (EventHandlers.OnVoteEnd != null)
        {
            EventHandlers.OnVoteEnd?.Invoke(voteSucceeds);
        }
    }
}
