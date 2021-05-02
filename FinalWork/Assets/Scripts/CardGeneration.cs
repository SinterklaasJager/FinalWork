using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardGeneration : NetworkBehaviour
{
    private GameManager gameManager;
    private Helper helpers;

    private SyncList<Enums.CardType> cardDeck = new SyncList<Enums.CardType>();

    public void SetUp(GameManager gm, Helper hlp)
    {
        gameManager = gm;
        helpers = hlp;
    }

    public void GenerateCardDeck()
    {
        cardDeck = new SyncList<Enums.CardType>();

        for (int i = 0; i < 11; i++)
        {
            cardDeck.Add(Enums.CardType.bad);
        }
        for (int i = 0; i < 7; i++)
        {
            cardDeck.Add(Enums.CardType.good);
        }
        List<int> randomNumbers = new List<int>();
        randomNumbers = helpers.GenerateRandomNumbers(cardDeck.Count);

        for (int i = 0; i < cardDeck.Count; i++)
        {
            cardDeck[i] = cardDeck[randomNumbers[i]];
        }

    }
    public Enums.CardType GetTopCard()
    {
        Enums.CardType topCard;

        if (cardDeck == null || cardDeck.Count < 3)
        {
            GenerateCardDeck();
        }

        topCard = cardDeck[0];
        cardDeck.Remove(cardDeck[0]);

        return topCard;
    }

    public List<Enums.CardType> GetTopThreeCards()
    {
        List<Enums.CardType> topThreeCards = new List<Enums.CardType>();

        if (cardDeck == null || cardDeck.Count < 3)
        {
            GenerateCardDeck();
        }

        for (int i = 0; i < 3; i++)
        {
            topThreeCards.Add(cardDeck[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            cardDeck.Remove(cardDeck[i]);
        }
        return topThreeCards;
    }

    public List<Enums.CardType> LookAtTopThreeCards()
    {
        List<Enums.CardType> topThreeCards = new List<Enums.CardType>();

        if (cardDeck == null || cardDeck.Count < 3)
        {
            GenerateCardDeck();
        }

        for (int i = 0; i < 3; i++)
        {
            topThreeCards[i] = cardDeck[i];
        }
        return topThreeCards;
    }
}
