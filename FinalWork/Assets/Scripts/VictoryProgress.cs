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
    private GameManager gameManager;
    private RoundManager roundManager;

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

    [Command(requiresAuthority = false)]
    public void SetGameManager(GameManager gameManager, RoundManager roundManager)
    {
        this.gameManager = gameManager;
        this.roundManager = roundManager;
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
        }
        else
        if (badPoints == 4)
        {
            MustKill();
        }
        else if (badPoints == 5)
        {
            VetoPowerEnabled();
            MustKill();
        }
        else if (badPoints == 6)
        {
            BadGuysWin(Enums.GameEndReason.enoughBadPoints);
        }

        if (!(badPoints > 3 || goodPoints == 5))
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
    }

    public void BadGuysWin(Enums.GameEndReason reason)
    {
        Debug.Log("Bad Guys Win because: " + reason);
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

    public int GetGoodPoints()
    {
        return goodPoints;
    }
    public int GetBadPoints()
    {
        return badPoints;
    }

    [Command(requiresAuthority = false)]
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

        AddVisualElement(cardType);
        CheckPoints();
    }

    [Command(requiresAuthority = false)]
    private void AddVisualElement(Enums.CardType cardType)
    {
        GameObject comp;

        if (cardType == Enums.CardType.good)
        {
            var pos = goodComponents[goodPoints - 1].gameObject.transform.localPosition;
            comp = Instantiate(gameManager.spawnableObjects.goodRocketComponent, pos, Quaternion.identity, transform);
        }
        else
        {
            var pos = badComponents[badPoints - 1].gameObject.transform.localPosition;
            comp = Instantiate(gameManager.spawnableObjects.badRocketComponent, pos, Quaternion.identity, transform);
        }

        NetworkServer.Spawn(comp);
    }
}
