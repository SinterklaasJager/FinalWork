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
    [SyncVar] private GameManager gameManager;

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
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    private void CheckPoints()
    {
        Debug.Log("goodpoints: " + badPoints);
        Debug.Log("goodpoints: " + goodPoints);

        if (goodPoints == 5)
        {
            GoodGuysWin();
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
            BadGuysWin();
        }

    }

    public void GoodGuysWin()
    {

    }

    public void BadGuysWin()
    {

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
        Debug.Log("Points: ");
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
        // GameObject comp;

        if (cardType == Enums.CardType.good)
        {
            var comp = Instantiate(gameManager.spawnableObjects.goodRocketComponent, goodComponents[goodPoints - 1].gameObject.transform);
            NetworkServer.Spawn(comp);
        }
        else
        {
            var comp = Instantiate(gameManager.spawnableObjects.badRocketComponent, badComponents[badPoints - 1].gameObject.transform);
            NetworkServer.Spawn(comp);
        }

        // NetworkServer.Spawn(comp);
    }
}
