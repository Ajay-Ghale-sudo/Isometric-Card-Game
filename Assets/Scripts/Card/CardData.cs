using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data",  fileName = "New Card Data")]
public class CardData : ScriptableObject
{
    public Sprite sprite;
    public string  cardName;
    public string description;
    public int cost;
    
    public List<CardAction> cardActions;
}
