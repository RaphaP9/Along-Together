using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ObjectShopInventoryHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private ObjectShopInventorySingleUI objectShopInventorySingleUI;

    [Header("Runtime Filled")]
    [SerializeField] private bool isHovered;

    public bool IsHovered => isHovered;

    public static event EventHandler<OnObjectShopInventoryHoverEventArgs> OnObjectShopInventoryHoverEnter;
    public static event EventHandler<OnObjectShopInventoryHoverEventArgs> OnObjectShopInventoryHoverExit;

    public class OnObjectShopInventoryHoverEventArgs : EventArgs
    {
        public ObjectIdentified objectIdentified;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        OnObjectShopInventoryHoverEnter?.Invoke(this, new OnObjectShopInventoryHoverEventArgs { objectIdentified = objectShopInventorySingleUI.ObjectIdentified });
        Debug.Log("Object Hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnObjectShopInventoryHoverExit?.Invoke(this, new OnObjectShopInventoryHoverEventArgs { objectIdentified = objectShopInventorySingleUI.ObjectIdentified });
        Debug.Log("Object Unhovered");
    }
}
