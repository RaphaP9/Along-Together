using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectHoverUIButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectHoverUIHandler inventoryObjectHoverUIHandler;
    [Space]
    [SerializeField] private Button sellButton;
    [SerializeField] private Button backButton;

    public event EventHandler OnBackButtonPressed;
    public event EventHandler OnSellButtonPressed;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        sellButton.onClick.AddListener(HandleSell);
        backButton.onClick.AddListener(HandleBack);
    }

    private void HandleBack()
    {
        OnBackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void HandleSell()
    {
        if (inventoryObjectHoverUIHandler.HasRegisteredGenericInventoryObject())
        {
            switch (inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.inventoryObjectSO.GetInventoryObjectType())
            {
                case InventoryObjectType.Object:
                    HandleObjectSell(inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified);
                    break;
                case InventoryObjectType.Treat:
                    HandleTreatSell(inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified);
                    break;
            }
        }

        OnSellButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void HandleObjectSell(GenericInventoryObjectIdentified genericInventoryObjectIdentified)
    {
        GoldManager.Instance.AddGold(genericInventoryObjectIdentified.inventoryObjectSO.sellPrice);
        ObjectsInventoryManager.Instance.RemoveObjectFromInventoryByGUID(genericInventoryObjectIdentified.assignedGUID);
    }

    private void HandleTreatSell(GenericInventoryObjectIdentified genericInventoryObjectIdentified)
    {
        GoldManager.Instance.AddGold(genericInventoryObjectIdentified.inventoryObjectSO.sellPrice);
        TreatsInventoryManager.Instance.RemoveTreatFromInventoryByGUID(genericInventoryObjectIdentified.assignedGUID);
    }

}
