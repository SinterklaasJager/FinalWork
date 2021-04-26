using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoteForTeamLeaderUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtTeamLeaderCandidate, txtAssistantCandidate;

    private VoteForOrganisers voteForLeaderScript;

    public void SetNames(string teamLeader, string assistant, GameObject voteLeader)
    {
        voteForLeaderScript = voteLeader.GetComponent<VoteForOrganisers>();
        txtTeamLeaderCandidate.text = teamLeader;
        txtAssistantCandidate.text = assistant;
    }
    public void OnYesClick()
    {
        voteForLeaderScript.AddVote(true);
        Dismiss();
    }
    public void OnNoClick()
    {
        voteForLeaderScript.AddVote(false);
        Dismiss();
    }

    public void Dismiss()
    {
        Destroy(gameObject);
    }
}
