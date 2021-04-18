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

    private Enums.CardPickerType currentCardPicker;
    public Enums.EventHandlers eventHandlers;

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
        currentCardPicker = Enums.CardPickerType.assistant;
        assistantCardUI = Instantiate(uIManager.AssistantCardUI, uIManager.gameObject.transform);
        cards = cardGeneration.GetTopThreeCards();

        int i = 1;

        foreach (var card in cards)
        {
            GameObject tempCard;
            if (card == Enums.CardType.good)
            {
                tempCard = Instantiate(prefabs.btnGood, assistantCardUI.transform.Find("CardSpot" + i));

            }
            else
            {
                tempCard = Instantiate(prefabs.btnBad, assistantCardUI.transform.Find("CardSpot" + i));
            }

            tempCard.GetComponent<CardButtonClick>().SetCardDealerUI(gameObject.GetComponent<CardDealerUI>(), i - 1);
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
        currentCardPicker = Enums.CardPickerType.teamLeader;

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

    public void CardSelection(int cardNumber)
    {

        if (currentCardPicker == Enums.CardPickerType.assistant)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == cardNumber)
                {
                    cards.Remove(cards[i]);
                }
            }

            ShowProjectManagerCards();
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == cardNumber)
                {
                    eventHandlers.OnCardSelected?.Invoke(cards[i]);
                }
            }
        }
    }
}
