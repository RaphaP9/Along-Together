using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject, IHasEmbeddedNumericStats
{
    [Header("InventoryObjectSO Settings")]
    public int id;
    public string _name;
    public Rarity objectRarity;
    [TextArea(3,10)] public string description;
    public Sprite sprite;

    [Header("Prices")]
    [Range(0, 1000)] public int price;
    [Range(0, 1000)] public int sellPrice;

    [Header("Numeric Embedded Stats")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;

    public abstract InventoryObjectType GetInventoryObjectType();
    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
