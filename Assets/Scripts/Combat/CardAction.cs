using System;
using UnityEngine;
using Alchemy.Inspector;

public enum ActionCategory { Attack, Defense }
public enum DamageAction { Slash, Pierce, Blunt }
public enum DefenseAction { Block, Counter, Parry }

[Serializable]
public class CardAction
{
    public ActionCategory category;

    [Header("Card Actions")] 
    [ShowIf(nameof(isAttack))] public DamageAction damageAction;
    [Header("Card Actions")]
    [ShowIf(nameof(isDefense))] public DefenseAction defenseAction;

    [Header("Card Roll Range")] 
    public int minPower;
    public int maxPower;
    
    private bool isAttack => category == ActionCategory.Attack;
    private bool isDefense => category == ActionCategory.Defense;
}
