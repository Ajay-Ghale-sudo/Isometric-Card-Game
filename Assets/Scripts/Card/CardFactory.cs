using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory  : MonoBehaviour
{
    [Header("Card Objects")] 
    public GameObject cardPrefab;
    public GameObject visualHolder;
    public GameObject cardVisualPrefab;
    
    [Header("Card Layout Controls")]
    public CardLayoutController cardLayoutController;

    [Header("Card Data Controls")] 
    public List<CardData> cardData;

    public void SpawnHand()
    {
        foreach (CardData data in cardData)
        {
            Card card = CreateCard();
            CardVisual cardVisual = CreateCardVisual();
            BindCardEvents(card, cardVisual);
            AssignCardDataToVisual(cardVisual, data);
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

    private Card CreateCard()
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
        card.PointerEnterEvent.AddListener(cardVisual.OnHover);
        card.PointerExitEvent.AddListener(cardVisual.OnEndHover);
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

    private void AssignCardDataToVisual(CardVisual cardVisual, CardData data)
    {
        cardVisual.cardName.text = data.cardName;
        cardVisual.cardCost.text = data.cost.ToString();
        cardVisual.cardDescription.text = data.description;
        cardVisual.cardImage.sprite = data.sprite;
        for (int i = 0; i < data.cardActions.Count; i++)
        {
            cardVisual.cardPowerRange.text += data.cardActions[i].minPower + " - " +  data.cardActions[i].maxPower + "\n"; 
        }
    }
}
