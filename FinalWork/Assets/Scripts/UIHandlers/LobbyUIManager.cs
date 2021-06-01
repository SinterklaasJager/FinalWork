using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyPlayerPrefab, placeToPutPlayers;
    [SerializeField] private TMP_Text txtAmountOfPlayers;
    [SerializeField] private TMP_Text txtMaxAmountOfPlayers;
    [SerializeField] private CanvasGroup canvasGroup;
    private int maxAmountOfPlayers;
    [SyncVar] private int currentAmountOfPlayers = 0;

    public void Awake()
    {
        gameObject.name = "LobbyUI";
    }
    [TargetRpc]
    public void ChangeOpacity(NetworkConnection target, float opacity)
    {
        Debug.Log(gameObject.name + ", opacity: " + opacity);
        //gameObject.SetActive(true);
        canvasGroup.alpha = opacity;
    }
    [Server]
    public void AddNewPlayer(string playerName)
    {
        currentAmountOfPlayers++;
        var newObj = Instantiate(lobbyPlayerPrefab, placeToPutPlayers.transform);
        NetworkServer.Spawn(newObj);
        NetworkServer.Spawn(newObj);
        ChangeCurrentPlayerAmount(currentAmountOfPlayers);
        AddPlayerToGrid(newObj);
    }

    [ClientRpc]
    public void AddPlayerToGrid(GameObject newObj)
    {
        newObj.GetComponent<Image>().color = Random.ColorHSV();
        newObj.GetComponentInChildren<TMP_Text>().text = name;
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
