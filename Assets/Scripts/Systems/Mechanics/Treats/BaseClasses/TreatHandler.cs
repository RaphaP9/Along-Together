using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected TreatConfigSO treatConfigSO;

    protected List<InventoryObjectSO> InventoryObjectsToActivate => treatConfigSO.activatorInventoryObjects;

    protected bool previouslyEnabled = false;

    private void Awake()
    {
        SetSingleton();
    }

    protected virtual void Update()
    {
        HandleTreatEnable();
    }

    protected abstract void SetSingleton();
    protected abstract void OnTreatEnableMethod();
    protected abstract void OnTreatDisableMethod();

    protected virtual bool TreatEnabled()
    {
        foreach(InventoryObjectSO inventoryObjectSO in treatConfigSO.activatorInventoryObjects)
        {
            switch (inventoryObjectSO.GetInventoryObjectType())
            {
                case InventoryObjectType.Object:
                    if (ObjectsInventoryManager.Instance.HasObjectSOInInventory(inventoryObjectSO as ObjectSO)) return true;
                    break;
                case InventoryObjectType.Treat:
                    if (TreatsInventoryManager.Instance.HasTreatSOInInventory(inventoryObjectSO as TreatSO)) return true;
                    break;
            }
        }

        return false;
    }

    private void HandleTreatEnable()
    {
        bool currentlyEnabled = TreatEnabled();

        if(!previouslyEnabled && currentlyEnabled)
        {
            OnTreatEnableMethod();
        }
        else if(previouslyEnabled && !currentlyEnabled)
        {
            OnTreatDisableMethod();
        }

        previouslyEnabled = currentlyEnabled;       
    }
}
