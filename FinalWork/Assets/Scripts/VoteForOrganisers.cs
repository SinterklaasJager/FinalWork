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

        if (!currentPlayer.GetIsDead())
        {
            AddVoter();

            uiManager.StartLeaderVotingUI(teamLeader.GetName(), assistant.GetName(), gameObject);
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
