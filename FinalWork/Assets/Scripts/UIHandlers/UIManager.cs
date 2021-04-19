using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
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
    public void IniateRoundUI()
    {
        RoundUI = Instantiate(RoundUIObj, transform);

    }

    public CardDealerUI StartAssistantCardDrawUI()
    {
        CardDealerUI = gameObject.AddComponent<CardDealerUI>();
        CardDealerUI.SetGameManager(gameManager);
        CardDealerUI.ShowAssistantCards();
        return CardDealerUI;
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
