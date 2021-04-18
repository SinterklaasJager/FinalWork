using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonClick : MonoBehaviour
{
    private CardDealerUI cardDealerUI;
    private int cardNumber;

    public void SetCardDealerUI(CardDealerUI cdUI, int cardNumber)
    {
        cardDealerUI = cdUI;
        this.cardNumber = cardNumber;
    }

    public void OnBtnClick()
    {
        cardDealerUI.CardSelection(cardNumber);
    }
}
