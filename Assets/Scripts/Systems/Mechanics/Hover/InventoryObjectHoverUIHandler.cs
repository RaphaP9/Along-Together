using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObjectHoverUIHandler : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private GenericInventoryObjectIdentified currentGenericInventoryObjectIdentified;
    [SerializeField] private bool pressedLock;

    public event EventHandler<OnGenericInventoryObjectEventArgs> OnGenericInventoryObjectIdentifiedSet;

    public event EventHandler<OnGenericInventoryObjectEventArgs> OnHoverOpening;
    public event EventHandler<OnGenericInventoryObjectEventArgs> OnHoverClosing;
    public event EventHandler<OnGenericInventoryObjectEventArgs> OnPressEnabling;

    public class OnGenericInventoryObjectEventArgs : EventArgs
    {
        public GenericInventoryObjectIdentified genericInventoryObjectIdentified;
    }

    private void OnEnable()
    {
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEnter += ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverExit += ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryPressed += ObjectShopInventoryHoverHandler_OnObjectShopInventoryPressed;

        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEnter += TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverExit += TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryPressed += TreatShopInventoryHoverHandler_OnTreatShopInventoryPressed;

        //
    }

    private void OnDisable()
    {
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEnter -= ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverExit -= ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryPressed -= ObjectShopInventoryHoverHandler_OnObjectShopInventoryPressed;

        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEnter -= TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverExit -= TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryPressed -= TreatShopInventoryHoverHandler_OnTreatShopInventoryPressed;

        //
    }

    private void Start()
    {
        pressedLock = false;
    }

    #region Get & Set
    private void ClearCurrentGenericInventoryObjectIdentified() => currentGenericInventoryObjectIdentified = null;
    private void SetCurrentGenericInventoryObjectIdentified(string GUID, InventoryObjectSO inventoryObjectSO) => currentGenericInventoryObjectIdentified = new GenericInventoryObjectIdentified { assignedGUID = GUID, inventoryObjectSO = inventoryObjectSO };
    #endregion

    #region Utility Methods
    private bool ValuesCoincideWithRegisteredGenericInventoryObject(string GUID, InventoryObjectSO inventoryObjectSO)
    {
        if (!HasRegisteredGenericInventoryObject()) return false;
        if (currentGenericInventoryObjectIdentified.assignedGUID != GUID) return false;
        if (currentGenericInventoryObjectIdentified.inventoryObjectSO != inventoryObjectSO) return false;

        return true;
    }

    private bool HasRegisteredGenericInventoryObject()
    {
        if (currentGenericInventoryObjectIdentified == null) return false;
        if (currentGenericInventoryObjectIdentified.assignedGUID == "") return false;
        if (currentGenericInventoryObjectIdentified.inventoryObjectSO == null) return false;

        return true;
    }
    #endregion

    #region Method Handlers
    private void HandleHoverEnter(string GUID, InventoryObjectSO inventoryObjectSO)
    {
        if (pressedLock) return;

        if(ValuesCoincideWithRegisteredGenericInventoryObject(GUID, inventoryObjectSO)) return;

        SetCurrentGenericInventoryObjectIdentified(GUID, inventoryObjectSO);
        OnGenericInventoryObjectIdentifiedSet?.Invoke(this, new OnGenericInventoryObjectEventArgs { genericInventoryObjectIdentified = currentGenericInventoryObjectIdentified });
        OnHoverOpening?.Invoke(this, new OnGenericInventoryObjectEventArgs { genericInventoryObjectIdentified = currentGenericInventoryObjectIdentified });
    }

    private void HandleHoverExit(string GUID, InventoryObjectSO inventoryObjectSO)
    {
        if (pressedLock) return;

        if (!ValuesCoincideWithRegisteredGenericInventoryObject(GUID, inventoryObjectSO)) return;

        OnHoverClosing?.Invoke(this, new OnGenericInventoryObjectEventArgs {genericInventoryObjectIdentified=currentGenericInventoryObjectIdentified});
        ClearCurrentGenericInventoryObjectIdentified();
    }

    private void HandlePress(string GUID, InventoryObjectSO inventoryObjectSO)
    {
        if (!ValuesCoincideWithRegisteredGenericInventoryObject(GUID, inventoryObjectSO)) return;

        pressedLock = true;
        OnPressEnabling?.Invoke(this, new OnGenericInventoryObjectEventArgs {genericInventoryObjectIdentified = currentGenericInventoryObjectIdentified });
    }
    #endregion

    #region Object Subscriptions
    private void ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter(object sender, ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEventArgs e)
    {
        HandleHoverEnter(e.objectIdentified.assignedGUID, e.objectIdentified.objectSO);
    }

    private void ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit(object sender, ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEventArgs e)
    {
        HandleHoverExit(e.objectIdentified.assignedGUID, e.objectIdentified.objectSO);
    }

    private void ObjectShopInventoryHoverHandler_OnObjectShopInventoryPressed(object sender, ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEventArgs e)
    {
        HandlePress(e.objectIdentified.assignedGUID, e.objectIdentified.objectSO);
    }
    #endregion

    #region Treat Subscriptions
    private void TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter(object sender, TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEventArgs e)
    {
        HandleHoverEnter(e.treatIdentified.assignedGUID, e.treatIdentified.treatSO);
    }

    private void TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit(object sender, TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEventArgs e)
    {
        HandleHoverExit(e.treatIdentified.assignedGUID, e.treatIdentified.treatSO);
    }

    private void TreatShopInventoryHoverHandler_OnTreatShopInventoryPressed(object sender, TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEventArgs e)
    {
        HandlePress(e.treatIdentified.assignedGUID, e.treatIdentified.treatSO);
    }

    #endregion
}
