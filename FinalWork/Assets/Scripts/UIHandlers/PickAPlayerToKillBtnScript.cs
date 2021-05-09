﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickAPlayerToKillBtnScript : MonoBehaviour
{
    private Player player;
    private Button button;

    [SerializeField]
    TextMeshProUGUI txtPlayerName;
    private GameObject playerKillerUIObj;

    public void SetPlayer(Player player, GameObject playerKillerUIObj, string playerName)
    {
        this.player = player;
        this.playerKillerUIObj = playerKillerUIObj;
        button = gameObject.GetComponent<Button>();

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 20);
        txtPlayerName.text = playerName;
    }

    public void OnButtonClick()
    {
        playerKillerUIObj.GetComponent<PlayerKillerUI>().OnButtonClick(gameObject);
    }
    public Player GetSelectedPlayer()
    {
        return player;
    }
}