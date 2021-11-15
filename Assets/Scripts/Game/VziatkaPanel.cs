using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class VziatkaPanel : MonoBehaviour
{
    public List<CardInHand> Cards;

    public void ShowTake()
    {
        int i = 0;
        foreach (CardInHand card in Cards)
        {
            
            card.Take(i*0.3f);
            i++;
        }
    }

    private void Take()
    {
        
    }
    public void HideCards()
    {
        print("HideCards "+Cards.Count);
        foreach (CardInHand card in Cards)
        {
            card.HideCard();
            card.gameObject.SetActive(false);
        }
    }
}
