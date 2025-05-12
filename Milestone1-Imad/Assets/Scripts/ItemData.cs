using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackStep
{
    public string animationTrigger;
    public float lockoutDuration = 0.8f; 
    public float comboWindowStart;
    public float comboWindowEnd;
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string inHandObjectName;

    [Header("Combat Settings")]
    public List<AttackStep> comboSequence = new();

    
    [Header("Stats Settings")]
    public float hpRegenAmount = 0f;
    public float staminaRegenAmount = 0f;
    public string auraPrefab;
}


