using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteForOrganisers : MonoBehaviour
{
    private int totalVotesCast, yesVotes, totalVoters;
    private GameObject gameManager, uiManager;
    private List<Player> players;
    private Player teamLeader, assistant;

    public void StartNewVotingRound(GameObject gm, GameObject um, List<Player> pl)
    {
        gameManager = gm;
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
                uiManager.GetComponent<UIManager>().StartLeaderVotingUI(teamLeader.GetName(), assistant.GetName(), gameObject);
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
            FinishVotingRound();
        }
    }

    public void FinishVotingRound()
    {

    }
}
