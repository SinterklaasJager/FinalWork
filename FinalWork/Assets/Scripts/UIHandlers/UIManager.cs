using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIManager : NetworkBehaviour
{
    [SyncVar] private GameObject gameManager;
    [SyncVar(hook = nameof(UniversalCanvasHook))] private GameObject universalCanvasObj;
    private UniversalCanvasManager universalCanvas;

    [Header("UI Objects")]
    [SerializeField] private GameObject RoundUIObj;
    [SerializeField] public GameObject PickAnAssistantUIObj;
    [SerializeField] private GameObject VoteTeamLeaderObj;
    [SerializeField] private GameObject PlayerUI;
    [SerializeField] private GameObject EnterPlayerNameUI;
    [SerializeField] private GameObject ArHostUIObj;
    [SerializeField] private GameObject ARCloudAnchorObj;

    [Header("UI Instances")]
    public GameObject AssistantCardUI;
    public GameObject ProjectManagerCardUI;
    public CardDealerUI CardDealerUI;
    public GameObject RoundUI;
    public GameObject PickAnAssistantUI;
    public GameObject VoteTeamLeaderUI;
    public GameObject ArHostUI;
    private PlayerUIComponent playerUIScript;
    private GameObject GetPlayerNameUI;


    public void SetGameManager(GameObject gameManager, UniversalCanvasManager ucm)
    {
        this.gameManager = gameManager;
        universalCanvas = ucm;
        //universalCanvasObj = ucm.gameObject;

    }

    private void UniversalCanvasHook(GameObject oldUC, GameObject newUC)
    {
        universalCanvas = newUC.GetComponent<UniversalCanvasManager>();
    }
    // [Command(requiresAuthority = false)]
    // public void SetUpARHostUI(GameObject go, GameManager gm)
    // {
    //     InstantiateARHostUI(go.GetComponent<NetworkIdentity>().connectionToClient, gm);
    // }

    [TargetRpc]
    public void InstantiateARHostUI(NetworkConnection target, GameManager gm, GameObject ARObject)
    {
        Debug.Log(gm);
        Debug.Log("targetRPC: " + target);
        ArHostUI = Instantiate(ArHostUIObj, transform);
        ArHostUI.GetComponent<ARHostUI>().SetGameManager(gm);
        // ARObject.GetComponentInChildren<TapToPlaceObjects>().SetUp(gm, ArHostUI);
        //ARObject.GetComponentInChildren<CloudAnchorController>().SetUp(gm, ArHostUI);
        var arc = Instantiate(ARCloudAnchorObj, ARObject.transform);
        arc.GetComponent<CloudAnchorController>().SetUp(gm, ArHostUI);
        arc.GetComponent<CloudAnchorController>().OnEnterHostingModeClick();
    }

    [TargetRpc]
    public void SetARClient(NetworkConnection target, GameManager gm, GameObject ARObject)
    {
        var arc = Instantiate(ARCloudAnchorObj, ARObject.transform);
        arc.GetComponent<CloudAnchorController>().SetUp(gm, ArHostUI);
        arc.GetComponent<CloudAnchorController>().OnEnterResolvingModeClick();

    }

    [TargetRpc]
    public void InstantiatePlayerUI(NetworkConnection target, string playerName, int roleNum, GameManager gm)
    {
        var playerUI = Instantiate(PlayerUI, transform);
        playerUI.name = "playerUI";
        playerUIScript = playerUI.GetComponent<PlayerUIComponent>();
        playerUIScript.SetUI(playerName, roleNum, gm);

    }

    [TargetRpc]
    public void SetPlayerUIAllies(NetworkConnection target, int roleNum, string playerName)
    {
        playerUIScript.SetAllies(roleNum, playerName);
    }

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
        universalCanvas.StartLeaderVotingUI(tl, pl, voteScript, VoteTeamLeaderObj);
    }
    public void DisableLeaderVotingUI(NetworkConnection target)
    {
        universalCanvas.DisableLeaderVotingUI(target);
    }

    [TargetRpc]
    public void StartPlayerNameUI(NetworkConnection target, GameObject networkObj, GameManager gm)
    {
        GetPlayerNameUI = Instantiate(EnterPlayerNameUI);
        GetPlayerNameUI.transform.SetParent(transform, false);

        GetPlayerNameUI.GetComponent<AddPlayerNameUI>().SetNameUI(gm, networkObj);
    }
}
