using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreatShopInventoryHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private TreatShopInventorySingleUI treatShopInventorySingleUI;

    [Header("Runtime Filled")]
    [SerializeField] private bool isHovered;

    public bool IsHovered => isHovered;

    public static event EventHandler<OnTreatShopInventoryHoverEventArgs> OnTreatShopInventoryHoverEnter;
    public static event EventHandler<OnTreatShopInventoryHoverEventArgs> OnTreatShopInventoryHoverExit;

    public class OnTreatShopInventoryHoverEventArgs : EventArgs
    {
        public TreatIdentified objectIdentified;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        OnTreatShopInventoryHoverEnter?.Invoke(this, new OnTreatShopInventoryHoverEventArgs { objectIdentified = treatShopInventorySingleUI.TreatIdentified});
        Debug.Log("Treat Hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnTreatShopInventoryHoverExit?.Invoke(this, new OnTreatShopInventoryHoverEventArgs { objectIdentified = treatShopInventorySingleUI.TreatIdentified});
        Debug.Log("Treat Unhovered");
    }
}