using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSettingsSO", menuName = "ScriptableObjects/Shop/ShopSettings")]

public class ShopSettingsSO : ScriptableObject
{
    [Header("Shop Size")]
    [Range(3, 7)]public int shopSize;

    [Header("Type Settings")]
    public List<ShopInventoryObjectTypeSetting> shopInventoryObjectTypeSettings;

    [Header("Rarity Settings")]
    public List<StageShopInventoryObjectRaritySetting> shopInventoryObjectRaritySettings;

    [Header("Rerolls")]
    [Range(0, 100)] public int rerollBaseCost;
    [Range(0, 10)] public int rerollCostIncreasePerReroll;

    [Header("Pools")]
    public List<ObjectSO> objectsPool;
    public List<TreatSO> treatsPool;


    [Header("Other")]
    public List<InventoryObjectSO> randomBreakerInventoryObjectList;
}