using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickAssistantBtnScript : MonoBehaviour
{
    private Player player;
    private Button button;
    private int playerID;

    [SerializeField]
    TextMeshProUGUI txtPlayerName;
    private GameObject PickAnAssistantUI;

    public void SetPlayer(Player player, GameObject PickAnAssistantUI, string playerName, int playerID)
    {
        this.playerID = playerID;
        this.player = player;
        this.PickAnAssistantUI = PickAnAssistantUI;
        button = gameObject.GetComponent<Button>();

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 20);
        txtPlayerName.text = playerName;
        txtPlayerName.fontSize = 10;
    }

    public void OnButtonClick()
    {
        PickAnAssistantUI.GetComponent<PickAnAssistantUI>().OnButtonClick(gameObject);
    }
    public Player GetSelectedPlayer()
    {
        return player;
    }
    public int GetPlayerID()
    {
        return playerID;
    }
}
