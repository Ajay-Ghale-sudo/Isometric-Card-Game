using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFactory : MonoBehaviour
{
    [Header("Card Settings")] 
    public GameObject cardPrefab;
    public GameObject visualHolder;
    public GameObject cardVisualPrefab;
    public int initialCardCount = 5;
    
    [Header("Layout")]
    public CardLayoutController cardLayoutController;

    void Start()
    {
        for (int i = 0; i < initialCardCount; i++)
        {
            Card card = CreateCard();
            CardVisual cardVisual = CreateCardVisual();
            BindCardEvents(card, cardVisual);
            StartCoroutine(DelayedVisualPlacement(card, cardVisual));
            
        }
    }

    IEnumerator DelayedVisualPlacement(Card card, CardVisual cardVisual)
    {
        yield return new WaitForEndOfFrame();
        card.anchorPos = card.transform.position;
        cardVisual.transform.position = card.anchorPos;
        cardVisual.Initialize(card);
    }

    public Card CreateCard()
    {
        if (cardPrefab == null || cardLayoutController == null)
        {
            Debug.LogError("Card Prefab or Layout Group not assigned.");
            return null;
        }
        Card newCard = Instantiate(cardPrefab, cardLayoutController.transform).GetComponent<Card>();
        cardLayoutController.cardList.Add(newCard);
        return newCard;
    }


    private void BindCardEvents(Card card, CardVisual cardVisual)
    {
        card.DragEvent.AddListener(cardLayoutController.CheckNearestNeighbors);
        card.DragEvent.AddListener(cardVisual.OnDrag);
        card.BeginDragEvent.AddListener(cardVisual.OnBeginDrag);
        card.EndDragEvent.AddListener(cardVisual.OnEndDrag);
    }

    private CardVisual CreateCardVisual()
    {
        if (cardVisualPrefab == null)
        {
            Debug.LogError("Card Visual Prefab not assigned. Bad!");
            return null;
        }
        CardVisual newCardVisual = Instantiate(cardVisualPrefab, visualHolder.transform).GetComponent<CardVisual>();
        return newCardVisual;
    }
    
}
