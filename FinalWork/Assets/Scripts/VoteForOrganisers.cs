using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteForOrganisers : MonoBehaviour
{
    private int totalVotesCast, yesVotes, totalVoters;
    private GameManager gameManager;
    private UIManager uiManager;
    private List<Player> players;
    private Player teamLeader, assistant;

    public Enums.EventHandlers EventHandlers;

    public void StartNewVotingRound(UIManager um, List<Player> pl)
    {
        gameManager = gameObject.GetComponent<GameManager>();
        uiManager = um;
        players = pl;

        totalVotesCast = 0;
        yesVotes = 0;
        totalVoters = 0;

        foreach (var player in players)
        {
            if (!player.GetIsDead())
            {
                if (player.GetIsTeamLeaderCandidate())
                {
                    teamLeader = player;
                }

                if (player.GetisAssistantCandidate())
                {
                    assistant = player;
                }

                totalVoters++;
                //trigger ui;
                uiManager.StartLeaderVotingUI(teamLeader.GetName(), assistant.GetName(), gameObject);
            }
        }
    }

    public void AddVote(bool vote)
    {
        totalVotesCast++;
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
        if (EventHandlers.OnVoteEnd != null)
        {
            EventHandlers.OnVoteEnd?.Invoke(voteSucceeds);
        }

    }
}
