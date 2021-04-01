using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGeneration : MonoBehaviour
{
    private GameManager gameManager;
    private Helper helpers;

    private List<Enums.CardType> cardDeck;

    private void Awake()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        helpers = gameObject.GetComponent<Helper>();
    }

    public void GenerateCardDeck()
    {
        cardDeck = new List<Enums.CardType>();

        for (int i = 0; i < 11; i++)
        {
            cardDeck.Add(Enums.CardType.bad);
        }
        for (int i = 0; i < 7; i++)
        {
            cardDeck.Add(Enums.CardType.good);
        }

        List<int> randomNumbers = helpers.GenerateRandomNumbers(cardDeck.Count);

        for (int i = 0; i < cardDeck.Count; i++)
        {
            cardDeck[i] = cardDeck[randomNumbers[i]];
        }

    }

    public List<Enums.CardType> GetTopThreeCards()
    {
        List<Enums.CardType> topThreeCards = new List<Enums.CardType>();

        if (cardDeck.Count < 3)
        {
            GenerateCardDeck();
        }

        for (int i = 0; i < 3; i++)
        {
            topThreeCards[i] = cardDeck[i];
            cardDeck.Remove(cardDeck[i]);
        }
        return topThreeCards;
    }

    public List<Enums.CardType> LookAtTopThreeCards()
    {
        if (cardDeck.Count < 3)
        {
            GenerateCardDeck();
        }

        List<Enums.CardType> topThreeCards = new List<Enums.CardType>();
        for (int i = 0; i < 3; i++)
        {
            topThreeCards[i] = cardDeck[i];
        }
        return topThreeCards;
    }
}
