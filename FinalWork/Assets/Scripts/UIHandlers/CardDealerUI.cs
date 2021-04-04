using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealerUI : MonoBehaviour
{
    //On new round, start Card Dealer
    //Show UI for Assistant en TeamLeader
    //Generate Correct Buttons
    private GameObject gameManager, assistantCardUI, projectManagerCardUI;
    private UIManager uIManager;
    private SpawnableObjects prefabs;
    private CardGeneration cardGeneration;
    private List<Enums.CardType> cards;

    private void Start()
    {
        uIManager = gameObject.GetComponent<UIManager>();
        uIManager.CardDealerUI = gameObject;


    }

    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
        prefabs = gameManager.GetComponent<SpawnableObjects>();
        cardGeneration = gameManager.GetComponent<CardGeneration>();
    }

    public void ShowAssistantCards()
    {
        //if player is assistant;

        assistantCardUI = Instantiate(uIManager.AssistantCardUI, uIManager.gameObject.transform);
        cards = cardGeneration.GetTopThreeCards();

        int i = 1;

        foreach (var card in cards)
        {
            if (card == Enums.CardType.good)
            {
                Instantiate(prefabs.btnGood, assistantCardUI.transform.Find("CardSpot" + i));
            }
            else
            {
                Instantiate(prefabs.btnBad, assistantCardUI.transform.Find("CardSpot" + i));
            }

            i++;
        }

    }

    public void HideAssistantCards()
    {
        GameObject.Destroy(assistantCardUI);
    }
    public void ShowProjectManagerCards()
    {
        //if player is assistant;

        projectManagerCardUI = Instantiate(uIManager.ProjectManagerCardUI, uIManager.gameObject.transform);

        foreach (var card in cards)
        {
            if (card == Enums.CardType.good)
            {
                Instantiate(prefabs.btnGood, projectManagerCardUI.transform);
            }
            else
            {
                Instantiate(prefabs.btnBad, projectManagerCardUI.transform);
            }
        }

    }
    public void HideProjectManagerCards()
    {
        GameObject.Destroy(projectManagerCardUI);
        cards.Clear();
    }
}
