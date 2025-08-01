using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutController : MonoBehaviour
{
    public Vector3 grabbedCardOriginalPosition;
    public Card grabbedCard;
    public Card closestCard;
    
    public List<Card> cardList;

    public void SwapCards(int indexA, int indexB)
    {
        if (indexA < 0 || indexB < 0 || indexA >= cardList.Count || indexB >= cardList.Count)
        {
            Debug.Log("Swap indices out of range. Bad!");
            return;
        }
        (cardList[indexA], cardList[indexB]) = (cardList[indexB], cardList[indexA]);
        

        cardList[indexA].transform.SetSiblingIndex(indexA);
        cardList[indexB].transform.SetSiblingIndex(indexB);
    }

    public void CheckNearestNeighbors(Card draggingCard)
    {
        float swapThreshold = 25.0f;
        Vector2 originPos = draggingCard.anchorPos;
        Vector2 dragPos = (Vector2)draggingCard.transform.position;
        
        if (draggingCard == null) return;
        foreach (Card card in cardList)
        {
            if (card == draggingCard) continue;

            if (Mathf.Abs(dragPos.x - card.transform.position.x) <= swapThreshold)
            {
                closestCard = card;
            }
        }

        if (closestCard != null)
        {
            int indexA = cardList.IndexOf(draggingCard);
            int indexB = cardList.IndexOf(closestCard);

            draggingCard.anchorPos = closestCard.transform.position;
            print($"AnchorPos of LayoutController is: {draggingCard.anchorPos}");
            SwapCards(indexA, indexB);
            closestCard = null;
        }
    }
}
