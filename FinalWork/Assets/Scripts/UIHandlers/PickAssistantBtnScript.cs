using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickAssistantBtnScript : MonoBehaviour
{
    private Player player;
    private Button button;

    [SerializeField]
    TextMeshProUGUI txtPlayerName;
    private GameObject PickAnAssistantUI;

    public void SetPlayer(Player player, GameObject PickAnAssistantUI)
    {
        this.player = player;
        this.PickAnAssistantUI = PickAnAssistantUI;
        button = gameObject.GetComponent<Button>();

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 20);
        txtPlayerName.text = "test " + player.GetName();
    }

    public void OnButtonClick()
    {
        PickAnAssistantUI.GetComponent<PickAnAssistantUI>().OnButtonClick(gameObject);
    }
    public Player GetSelectedPlayer()
    {
        return player;
    }
}
