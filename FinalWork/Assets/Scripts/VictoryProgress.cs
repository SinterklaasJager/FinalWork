using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VictoryProgress : NetworkBehaviour
{
    [SerializeField]
    List<GameObject> goodComponents = new List<GameObject>();
    [SerializeField]
    List<GameObject> badComponents = new List<GameObject>();
    [SyncVar] private int goodPoints;
    [SyncVar] private int badPoints;
    [SyncVar] private int rocketHeight;

    private GameManager gameManager;
    private RoundManager roundManager;

    private GameObject anchorController;
    private bool hasKilledOnce, hasKilledTwice;

    //3 Badpoints
    //TeamLead can look at top 3 cards

    //4 badpoints
    //Teamlead needs to kill someone

    //5 badpoints
    //teamlead needs to kill someone and veto power unlocked

    //6 bad points
    //Saboteur wins

    //5 Good points
    //Goodguys win

    // [Command(requiresAuthority = false)]

    public void Update()
    {
        if (anchorController == null)
        {
            return;
        }

        if (transform.position != anchorController.transform.position)
        {
            Debug.Log("UpdatePosition vic progress");
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    public void SetGameManager(GameManager gameManager, RoundManager roundManager)
    {
        this.gameManager = gameManager;
        this.roundManager = roundManager;

        anchorController = GameObject.Find("AnchorController");
        transform.SetParent(anchorController.transform);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    private void CheckPoints()
    {
        Debug.Log("goodpoints: " + badPoints);
        Debug.Log("goodpoints: " + goodPoints);

        if (goodPoints == 5)
        {
            GoodGuysWin(Enums.GameEndReason.enoughGoodPoints);
        }
        if (badPoints == 3)
        {
            PolicyPeekEnabled();
            NextRound();
        }
        else
        if (badPoints == 4 && !hasKilledOnce)
        {
            MustKill();
            hasKilledOnce = true;
        }
        else if (badPoints == 5 && !hasKilledTwice)
        {
            VetoPowerEnabled();
            MustKill();
            hasKilledTwice = true;
        }
        else if (badPoints == 6)
        {
            BadGuysWin(Enums.GameEndReason.enoughBadPoints);
        }
        else
        {
            NextRound();
        }
    }

    private void NextRound()
    {
        roundManager.EndTurn();
    }

    public void GoodGuysWin(Enums.GameEndReason reason)
    {
        Debug.Log("Good Guys Win because: " + reason);
        VictoryAchieved(reason);
    }

    public void BadGuysWin(Enums.GameEndReason reason)
    {
        Debug.Log("Bad Guys Win because: " + reason);
        VictoryAchieved(reason);
    }

    public void SaboteurElectedAssistant()
    {
        Debug.Log("The Saboteur Placed the BOMB");
        VictoryAchieved(Enums.GameEndReason.bombPlaced);
    }
    private void PolicyPeekEnabled()
    {
        //allow team leader to peek at top 3 cards
    }

    private void VetoPowerEnabled()
    {
        //allows team leader and product owner to veto change
    }

    private void MustKill()
    {
        // teamleader must kill player
        roundManager.SetUpDeathPicker();
    }
    [Server]
    private void VictoryAchieved(Enums.GameEndReason reason)
    {
        gameManager.uIManager.InstantiateVictoryScreen(reason, goodPoints, badPoints, gameManager);
        roundManager.StopGame();
    }

    public int GetGoodPoints()
    {
        return goodPoints;
    }
    public int GetBadPoints()
    {
        return badPoints;
    }

    // [Command(requiresAuthority = false)]
    public void SetPoints(Enums.CardType cardType)
    {
        if (cardType == Enums.CardType.good)
        {
            goodPoints++;
        }
        else
        {
            badPoints++;
        }

        anchorController.GetComponent<AnchorController>().ActivateRocketComponent(cardType);
        // AddVisualElement(cardType);
        SetProgressOnUI(goodPoints, badPoints);
        CheckPoints();
    }

    // [Command(requiresAuthority = false)]
    [Server]
    private void AddVisualElement(Enums.CardType cardType)
    {
        GameObject comp;

        if (cardType == Enums.CardType.good)
        {
            var pos = goodComponents[goodPoints - 1].gameObject.transform.position;
            comp = Instantiate(gameManager.spawnableObjects.goodRocketComponent, pos, Quaternion.identity, transform);
        }
        else
        {
            var pos = badComponents[badPoints - 1].gameObject.transform.position;
            comp = Instantiate(gameManager.spawnableObjects.badRocketComponent, pos, Quaternion.identity, transform);
        }

        NetworkServer.Spawn(comp);

    }

    [ClientRpc]
    private void SetProgressOnUI(int goodP, int badP)
    {
        var ui = GameObject.Find("ExtraInfoUI").GetComponent<ShowMoreInfoManager>();
        ui.SetCurrentScore(goodP, badP);
    }
}
