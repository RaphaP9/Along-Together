using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject
{
    [Header("InventoryObjectSO Settings")]
    public int id;
    public string _name;
    public Rarity objectRarity;
    [TextArea(3,10)] public string description;
    public Sprite sprite;
    [Space]
    [Range(0, 1000)] public int price;

    [Header("Numeric Embedded Stats")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;

    public abstract InventoryObjectType GetInventoryObjectType();

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
