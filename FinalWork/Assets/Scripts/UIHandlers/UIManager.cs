using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject gameManager;
    public GameObject AssistantCardUI;
    public GameObject ProjectManagerCardUI;
    public GameObject CardDealerUI;
    public GameObject RoundUI;

    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
        CardDealerUI.GetComponent<CardDealerUI>().SetGameManager(gameManager);
    }

    public GameObject GetGameManager()
    {
        return gameManager;
    }

    public void IniateRoundUI()
    {
        RoundUI = Instantiate(RoundUI, transform);
        gameManager.GetComponent<GameManager>().roundManager.uIManager = gameObject.GetComponent<UIManager>();
    }

}
