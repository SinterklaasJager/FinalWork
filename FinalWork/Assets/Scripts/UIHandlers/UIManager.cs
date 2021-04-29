using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIManager : NetworkBehaviour
{
    private GameObject gameManager;

    [Header("UI Objects")]
    [SerializeField] private GameObject RoundUIObj;
    [SerializeField] private GameObject PickAnAssistantUIObj;
    [SerializeField] private GameObject VoteTeamLeaderObj;

    [Header("UI Instances")]
    public GameObject AssistantCardUI;
    public GameObject ProjectManagerCardUI;
    public CardDealerUI CardDealerUI;
    public GameObject RoundUI;
    public GameObject PickAnAssistantUI;
    public GameObject VoteTeamLeaderUI;
    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;

    }

    public GameObject GetGameManager()
    {
        return gameManager;
    }
    public void IniateRoundUI(RoundManager rm)
    {
        RoundUI = Instantiate(RoundUIObj, transform);
        rm.SetRoundUI(RoundUI);
        Debug.Log("Iniate Round UI: " + RoundUI);
        NetworkServer.Spawn(RoundUI);

    }


    public CardDealerUI StartAssistantCardDrawUI()
    {
        CardDealerUI = gameObject.AddComponent<CardDealerUI>();
        CardDealerUI.SetGameManager(gameManager);
        CardDealerUI.ShowAssistantCards();
        return CardDealerUI;
    }
    public GameObject StartPickAnAssistantUI()
    {
        PickAnAssistantUI = Instantiate(PickAnAssistantUIObj, transform);
        PickAnAssistantUI.GetComponent<PickAnAssistantUI>().SetUiManager(gameObject, gameManager);
        return PickAnAssistantUI;
    }

    [ClientRpc]
    public void StartLeaderVotingUI(string tl, string pl, GameObject voteScript)
    {
        VoteTeamLeaderUI = Instantiate(VoteTeamLeaderObj, transform);
        VoteTeamLeaderUI.GetComponent<VoteForTeamLeaderUI>().SetNames(tl, pl, voteScript);
    }
}
