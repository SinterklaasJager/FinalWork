using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    [SerializeField] private PopulateGrid populateGrid;
    [SerializeField] private TMP_Text txtAmountOfPlayers;
    [SerializeField] private TMP_Text txtMaxAmountOfPlayers;
    private int maxAmountOfPlayers;
    private int currentAmountOfPlayers = 0;

    public void AddNewPlayer(string playerName)
    {
        currentAmountOfPlayers++;
        populateGrid.AddPlayerToGrid(playerName);
        ChangeCurrentPlayerAmount(currentAmountOfPlayers);
    }

    [ClientRpc]
    public void SetMaxAmountOfPlayers(int maxAmount)
    {
        Debug.Log("MaxAmountOfPlayers: " + maxAmount);
        txtMaxAmountOfPlayers.text = maxAmount.ToString();
    }

    [ClientRpc]
    private void ChangeCurrentPlayerAmount(int amount)
    {
        txtAmountOfPlayers.text = amount.ToString();
    }
}
