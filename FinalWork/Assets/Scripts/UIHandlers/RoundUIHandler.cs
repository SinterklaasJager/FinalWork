using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
public class RoundUIHandler : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI round;
    [SerializeField]
    private TextMeshProUGUI currentPlayer;

    public void UpdateRoundText(int newRound)
    {
        round.text = newRound.ToString();
    }
    public void UpdateCurrentPlayerText(string newPlayer)
    {
        currentPlayer.text = newPlayer.ToString();
    }
}
