using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGenerator : MonoBehaviour
{
    public static ShopGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const int MAX_OBJECT_GENERATION_ITERATIONS = 20;

    private void Awake()
    {
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopGenerator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }



    #region Check Type & Rarity
    private bool IsInventoryObjectOfType(InventoryObjectSO inventoryObjectSO, InventoryObjectType inventoryObjectType)
    {
        if (inventoryObjectSO.GetInventoryObjectType() == inventoryObjectType) return true;
        return false;
    }

    private bool IsInventoryObjectOfRarity(InventoryObjectSO inventoryObjectSO, Rarity inventoryObjectRarity)
    {
        if (inventoryObjectSO.objectRarity == inventoryObjectRarity) return true;
        return false;
    }
    #endregion
}

