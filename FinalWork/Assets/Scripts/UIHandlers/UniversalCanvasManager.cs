using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UniversalCanvasManager : NetworkBehaviour
{
    private GameObject voteTeamLeaderUI;
    [SyncVar] private GameManager gameManager;
    [SyncVar] private GameObject RoundUI;

    public void SetUp(GameManager gm)
    {
        gameManager = gm;
    }

    [ClientRpc]
    public void StartLeaderVotingUI(string tl, string pl, GameObject voteScript, GameObject VoteTeamLeaderObj)
    {
        voteTeamLeaderUI = Instantiate(gameManager.spawnableObjects.VoteTeamLeader, transform);
        voteTeamLeaderUI.GetComponent<VoteForTeamLeaderUI>().SetNames(tl, pl, voteScript);
    }
    public void IniateRoundUI(RoundManager rm)
    {
        if (RoundUI == null)
        {
            RoundUI = Instantiate(gameManager.spawnableObjects.roundUI, transform);
            Debug.Log("Iniate Round UI: " + RoundUI);
            NetworkServer.Spawn(RoundUI);
        }
    }
    [ClientRpc]
    public void UpdateRoundUI()
    {

    }
}
