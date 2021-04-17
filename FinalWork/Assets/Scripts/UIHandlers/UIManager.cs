using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject gameManager;

    [Header("UI Objects")]
    [SerializeField] private GameObject AssistantCardUIObj;
    [SerializeField] private GameObject ProjectManagerCardUIObj;
    [SerializeField] private GameObject CardDealerUIObj;
    [SerializeField] private GameObject RoundUIObj;
    [SerializeField] private GameObject PickAnAssistantUIObj;
    [SerializeField] private GameObject VoteTeamLeaderObj;

    [Header("UI Instances")]
    public GameObject AssistantCardUI;
    public GameObject ProjectManagerCardUI;
    public GameObject CardDealerUI;
    public GameObject RoundUI;
    public GameObject PickAnAssistantUI;
    public GameObject VoteTeamLeaderUI;
    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
        CardDealerUI.GetComponent<CardDealerUI>().SetGameManager(gameManager);
    }

    public GameObject GetGameManager()
    {
        return gameManager;
    }

    public void IniateRoundUI()
    {
        RoundUI = Instantiate(RoundUIObj, transform);

    }

    public void StartAssistantCardDrawUI()
    {
        CardDealerUI.GetComponent<CardDealerUI>().ShowAssistantCards();
    }
    public void StartLeaderCardDrawUI()
    {
        CardDealerUI.GetComponent<CardDealerUI>().ShowProjectManagerCards();
    }
    public GameObject StartPickAnAssistantUI()
    {
        PickAnAssistantUI = Instantiate(PickAnAssistantUIObj, transform);
        PickAnAssistantUI.GetComponent<PickAnAssistantUI>().SetUiManager(gameObject, gameManager);
        return PickAnAssistantUI;
    }

    public void StartLeaderVotingUI(string tl, string pl, GameObject voteScript)
    {
        VoteTeamLeaderUI = Instantiate(VoteTeamLeaderObj, transform);
        VoteTeamLeaderUI.GetComponent<VoteForTeamLeaderUI>().SetNames(tl, pl, voteScript);
    }
}
