using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryProgress
{
    private int goodPoints;
    private int badPoints;

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

    private void CheckPoints()
    {
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
    public int GeBadPoints()
    {
        return badPoints;
    }

    public void SetGoodPoints(int goodPoints)
    {
        this.goodPoints = goodPoints;
    }

    public void SetBadPoints(int badPoints)
    {
        this.badPoints = badPoints;
    }




}
