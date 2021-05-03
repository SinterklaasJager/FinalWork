using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIManager : NetworkBehaviour
{
    [SyncVar] private GameObject gameManager;
    [SyncVar(hook = nameof(UniversalCanvasHook))] private GameObject universalCanvasObj;
    private UniversalCanvasManager univeralCanvas;

    [Header("UI Objects")]
    [SerializeField] private GameObject RoundUIObj;
    [SerializeField] public GameObject PickAnAssistantUIObj;
    [SerializeField] private GameObject VoteTeamLeaderObj;

    [Header("UI Instances")]
    public GameObject AssistantCardUI;
    public GameObject ProjectManagerCardUI;
    public CardDealerUI CardDealerUI;
    public GameObject RoundUI;
    public GameObject PickAnAssistantUI;
    public GameObject VoteTeamLeaderUI;


    public void SetGameManager(GameObject gameManager, UniversalCanvasManager ucm)
    {
        this.gameManager = gameManager;
        univeralCanvas = ucm;
        //universalCanvasObj = ucm.gameObject;

    }

    private void UniversalCanvasHook(GameObject oldUC, GameObject newUC)
    {
        univeralCanvas = newUC.GetComponent<UniversalCanvasManager>();
    }
    // public GameObject GetGameManager()
    // {
    //     return gameManager;
    // }


    // [ClientRpc]
    private void SendRoundUIToClients(GameObject rndui, RoundManager rm)
    {
        RoundUI = rndui;
        rm.SetRoundUI(rndui);
    }


    public void StartAssistantCardDrawUI(GameObject gm)
    {
        CardDealerUI = gameObject.GetComponent<CardDealerUI>();
        CardDealerUI.SetGameManager(gameManager);
        CardDealerUI.ShowAssistantCards();
        //return CardDealerUI;
    }

    public CardDealerUI StartTeamLeaderCardDrawUI(List<Enums.CardType> cards)
    {
        CardDealerUI = gameObject.GetComponent<CardDealerUI>();
        CardDealerUI.SetGameManager(gameManager);
        CardDealerUI.ShowProjectManagerCards(cards);
        return CardDealerUI;
    }
    public GameObject StartPickAnAssistantUI(Player currentPlayer)
    {
        PickAnAssistantUI = Instantiate(PickAnAssistantUIObj, transform);
        //PickAnAssistantUI.GetComponent<PickAnAssistantUI>().SetUiManager(gameObject, gameManager, currentPlayer);
        return PickAnAssistantUI;
    }

    private void SpawnPickAnAssistantUI()
    {
        PickAnAssistantUI = Instantiate(PickAnAssistantUIObj, transform);
        NetworkServer.Spawn(PickAnAssistantUI);
    }

    // [ClientRpc]
    public void StartLeaderVotingUI(string tl, string pl, GameObject voteScript)
    {
        // VoteTeamLeaderUI = Instantiate(VoteTeamLeaderObj, transform);
        // VoteTeamLeaderUI.GetComponent<VoteForTeamLeaderUI>().SetNames(tl, pl, voteScript);
        univeralCanvas.StartLeaderVotingUI(tl, pl, voteScript, VoteTeamLeaderObj);
    }
}
